using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Czeum.Application.Services.Lobby;
using Czeum.Core.ClientCallbacks;
using Czeum.Core.Domain;
using Czeum.Core.DTOs;
using Czeum.Core.DTOs.Abstractions;
using Czeum.Core.DTOs.Achivement;
using Czeum.Core.DTOs.Wrappers;
using Czeum.Core.Services;
using Czeum.DAL;
using Czeum.DAL.Extensions;
using Czeum.Domain.Entities;
using Czeum.Domain.Services;
using Microsoft.EntityFrameworkCore;

namespace Czeum.Application.Services
{
    public class MatchService : IMatchService
    {
        private readonly IServiceContainer serviceContainer;
        private readonly CzeumContext context;
        private readonly IMapper mapper;
        private readonly IIdentityService identityService;
        private readonly IMatchConverter matchConverter;
        private readonly INotificationService notificationService;
        private readonly ILobbyStorage lobbyStorage;
        private readonly IAchivementCheckerService achivementService;

        public MatchService(IServiceContainer serviceContainer, CzeumContext context,
            IMapper mapper, IIdentityService identityService, IMatchConverter matchConverter,
            INotificationService notificationService, ILobbyStorage lobbyStorage,
            IAchivementCheckerService achivementService)
        {
            this.serviceContainer = serviceContainer;
            this.context = context;
            this.mapper = mapper;
            this.identityService = identityService;
            this.matchConverter = matchConverter;
            this.notificationService = notificationService;
            this.lobbyStorage = lobbyStorage;
            this.achivementService = achivementService;
        }

        public async Task<MatchStatus> CreateMatchAsync(Guid lobbyId)
        {
            var currentUser = identityService.GetCurrentUserName();
            var lobby = lobbyStorage.GetLobby(lobbyId);
            if (lobby.Host != currentUser)
            {
                throw new UnauthorizedAccessException("Only the host can create a match.");
            }
            
            if (!lobby.Validate())
            {
                throw new InvalidOperationException("The lobby is not in a valid state to start a match.");
            }

            var service = serviceContainer.FindBoardCreator(lobby);
            var board = service.CreateBoard(lobby);

            var statuses = await CreateMatchWithBoardAsync(lobby.Guests.Append(lobby.Host), board, false);
            lobbyStorage.RemoveLobby(lobbyId);
            
            await notificationService.NotifyEachAsync(statuses
                .Where(s => s.Key != currentUser)
                .Select(x => new KeyValuePair<string, Func<ICzeumClient, Task>>(x.Key, client => client.MatchCreated(x.Value))));

            return statuses.Single(s => s.Key == currentUser).Value;
        }
        
        public async Task CreateRandomMatchAsync(IEnumerable<string> players)
        {
            var service = serviceContainer.GetRandomBoardCreator();
            var board = (SerializedBoard)service.CreateDefaultBoard();

            var statues = await CreateMatchWithBoardAsync(players, board, true);
            await notificationService.NotifyEachAsync(statues
                .Select(x => new KeyValuePair<string,Func<ICzeumClient, Task>>(x.Key, client => client.MatchCreated(x.Value))));
        }

        private async Task<Dictionary<string, MatchStatus>> CreateMatchWithBoardAsync(IEnumerable<string> players, SerializedBoard board, bool isQuickMatch)
        {
            var users = await context.Users.Where(u => players.Any(p => p == u.UserName))
                .ToListAsync();
            
            var match = new Match
            {
                Board = board,
                CurrentPlayerIndex = 0,
                IsQuickMatch = isQuickMatch
            };

            match.Users = Enumerable.Range(0, users.Count)
                .Select(x => new UserMatch { User = users[x], Match = match, PlayerIndex = x })
                .ToList();

            context.Matches.Add(match);
            context.Boards.Add(board);
            await context.SaveChangesAsync();

            return match.Users.Select(um => new { Player = um.User.UserName, Status = matchConverter.ConvertFor(match, um.User.UserName) })
                .ToDictionary(x => x.Player, x => x.Status);
        }

        public async Task<MatchStatus> HandleMoveAsync(MoveData moveData)
        {
            var currentUserId = identityService.GetCurrentUserId();
            var match = await context.Matches.Include(m => m.Users)
                    .ThenInclude(um => um.User)
                .CustomSingleAsync(m => m.Id == moveData.MatchId, "No match with the given id was found.");

            var playerIndex = match.Users.SingleOrDefault(um => um.UserId == currentUserId)?.PlayerIndex;
            if (playerIndex == null)
            {
                throw new UnauthorizedAccessException("You are not a player in this match.");
            }
            
            if (playerIndex != match.CurrentPlayerIndex)
            {
                throw new InvalidOperationException("Not your turn.");
            }

            var service = serviceContainer.FindMoveHandler(moveData);
            var result = await service.HandleAsync(moveData, playerIndex.Value);
            match.Users.Single(x => x.UserId == currentUserId).User.MoveCount++;

            switch (result.Status)
            {
                case Status.Success:
                    match.NextTurn();
                    break;
                case Status.Win:
                    match.CurrentPlayerWon();
                    break;
                case Status.Draw:
                    match.Draw();
                    break;
            }

            var unlockedAchivements = await achivementService.CheckUnlockedAchivementsAsync(match.Users.Select(x => x.User));
            context.UserAchivements.AddRange(unlockedAchivements);

            await context.SaveChangesAsync();

            await notificationService.NotifyEachAsync(match.Users
                .Where(um => um.UserId != currentUserId)
                .Select(um => new KeyValuePair<string, Func<ICzeumClient, Task>>(um.User.UserName, 
                    client => client.ReceiveResult(matchConverter.ConvertFor(match, um.User.UserName)))));

            await notificationService.NotifyEachAsync(unlockedAchivements
                .Select(x => new KeyValuePair<string, Func<ICzeumClient, Task>>(x.User.UserName,
                    client => client.AchivementUnlocked(mapper.Map<AchivementDto>(x)))));

            return matchConverter.ConvertFor(match, 
                match.Users.Single(um => um.UserId == currentUserId).User.UserName);
        }

        public async Task<IEnumerable<MatchStatus>> GetMatchesAsync()
        {
            var currentUserName = identityService.GetCurrentUserName();

            return (await context.Matches.Include(m => m.Users)
                    .ThenInclude(um => um.User)
                .Include(m => m.Board)
                .Where(m => m.Users.Any(um => um.User.UserName == currentUserName))
                .ToListAsync())
                .Select(m => matchConverter.ConvertFor(m, currentUserName));
        }

        public async Task<IEnumerable<string>> GetOthersInMatchAsync(Guid matchId)
        {
            var currentUserId = identityService.GetCurrentUserId();
            return await context.UserMatches.Where(um => um.MatchId == matchId && um.UserId != currentUserId)
                .Select(um => um.User.UserName)
                .ToListAsync();
        }

        public async Task<MoveResultWrapper> GetBoardByMatchIdAsync(Guid matchId)
        {
            var board = await context.Boards.SingleAsync(b => b.MatchId == matchId);
            var service = serviceContainer.FindBoardConverter(board);
            return mapper.Map<MoveResultWrapper>(service.Convert(board));
        }
    }
}