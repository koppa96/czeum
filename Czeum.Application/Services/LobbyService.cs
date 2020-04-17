﻿using AutoMapper;
using Czeum.Application.Extensions;
using Czeum.Application.Services.Lobby;
using Czeum.Core.DTOs;
using Czeum.Core.DTOs.Abstractions.Lobbies;
using Czeum.Core.DTOs.Notifications;
using Czeum.Core.DTOs.Wrappers;
using Czeum.Core.Enums;
using Czeum.Core.GameServices.ServiceMappings;
using Czeum.Core.Services;
using Czeum.DAL;
using Czeum.DAL.Extensions;
using Czeum.Domain.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Czeum.Application.Services
{
	public class LobbyService : ILobbyService
	{
		private readonly ILobbyStorage lobbyStorage;
		private readonly CzeumContext context;
		private readonly IMapper mapper;
        private readonly IIdentityService identityService;
        private readonly ISoloQueueService soloQueueService;
        private readonly INotificationService notificationService;
		private readonly INotificationPersistenceService notificationPersistenceService;
		private readonly IGameTypeMapping gameTypeMapping;

		public LobbyService(ILobbyStorage lobbyStorage, 
			CzeumContext context,
			IMapper mapper,
            IIdentityService identityService,
            ISoloQueueService soloQueueService,
			INotificationService notificationService,
			INotificationPersistenceService notificationPersistenceService,
			IGameTypeMapping gameTypeMapping)
		{
			this.lobbyStorage = lobbyStorage;
			this.context = context;
			this.mapper = mapper;
            this.identityService = identityService;
            this.soloQueueService = soloQueueService;
            this.notificationService = notificationService;
			this.notificationPersistenceService = notificationPersistenceService;
			this.gameTypeMapping = gameTypeMapping;
		}

		public async Task<LobbyDataWrapper> JoinToLobbyAsync(Guid lobbyId)
		{
            var currentUser = identityService.GetCurrentUserName();
            var userLobby = lobbyStorage.GetLobbyOfUser(currentUser);
            if (userLobby != null || soloQueueService.IsQueuing(currentUser))
            {
                throw new InvalidOperationException("You can only join a lobby if you are not queuing and not in an other lobby.");
            }

			var lobby = lobbyStorage.GetLobby(lobbyId);

			var friends = await context.Friendships
				.Where(f => f.User1.UserName == lobby.Host || f.User2.UserName == lobby.Host)
				.Select(f => f.User1.UserName == lobby.Host ? f.User2.UserName : f.User1.UserName)
				.ToListAsync();

			var wrapper = mapper.Map<LobbyDataWrapper>(lobby);
			
			lobby.JoinGuest(currentUser, friends);
			await notificationService.NotifyAllExceptAsync(currentUser,
				client => client.LobbyChanged(wrapper));

			return wrapper;
		}

		public Task DisconnectFromCurrentLobbyAsync()
		{
            var currentUser = identityService.GetCurrentUserName();
            return DisconnectPlayerFromLobby(currentUser);
		}

        public async Task DisconnectPlayerFromLobby(string username)
        {
            var currentLobby = lobbyStorage.GetLobbyOfUser(username);
            if (currentLobby != null)
            {
                currentLobby.DisconnectPlayer(username);
                if (currentLobby.Empty)
                {
                    lobbyStorage.RemoveLobby(currentLobby.Id);
                    await notificationService.NotifyAllAsync(client => client.LobbyDeleted(currentLobby.Id));
                }
                else
                {
	                await notificationService.NotifyAllAsync(client =>
		                client.LobbyChanged(mapper.Map<LobbyDataWrapper>(currentLobby)));
                }
            }
            else
            {
                throw new InvalidOperationException("You are not in a lobby.");
            }
        }

        public async Task<LobbyDataWrapper> InvitePlayerToLobby(Guid lobbyId, string player)
		{
			var lobby = lobbyStorage.GetLobby(lobbyId);
			if (lobby.Host != identityService.GetCurrentUserName())
			{
				throw new UnauthorizedAccessException("Not authorized to invite to this lobby.");
			}

			if (lobby.InvitedPlayers.Contains(player))
			{
				throw new InvalidOperationException("This player has already been invited.");
			}

			var invitedUser = await context.Users.CustomSingleAsync(x => x.UserName == player, "No such player found.");

			lobby.InvitedPlayers.Add(player);
			lobby.LastModified = DateTime.UtcNow;

			var wrapper = mapper.Map<LobbyDataWrapper>(lobby);
			await notificationService.NotifyAsync(player,
				client => client.ReceiveLobbyInvite(wrapper));

			await notificationService.NotifyAllExceptAsync(identityService.GetCurrentUserName(),
				client => client.LobbyChanged(wrapper));

			await notificationPersistenceService.PersistNotificationAsync(NotificationType.InviteReceived,
				invitedUser.Id,
				identityService.GetCurrentUserId(),
				lobby.Id);

			return wrapper;
		}

		public async Task<LobbyDataWrapper> KickGuestAsync(Guid lobbyId, string guestName)
		{
			var lobby = lobbyStorage.GetLobby(lobbyId);
			if (lobby.Host != identityService.GetCurrentUserName())
			{
				throw new UnauthorizedAccessException("Not authorized to kick a player from this lobby.");
			}

			await DisconnectPlayerFromLobby(guestName);
			await notificationService.NotifyAsync(guestName,
				client => client.KickedFromLobby());

			return mapper.Map<LobbyDataWrapper>(lobby);
		}

		public Task<LobbyData?> GetLobbyOfUser(string user) {
			return Task.FromResult(lobbyStorage.GetLobbyOfUser(user));
		}

		public Task<List<LobbyDataWrapper>> GetLobbies()
		{
			return Task.FromResult(lobbyStorage.GetLobbies().Select(mapper.Map<LobbyDataWrapper>).ToList());
		}

		public async Task<LobbyDataWrapper> UpdateLobbySettingsAsync(LobbyDataWrapper lobbyData)
		{
			var currentUserName = identityService.GetCurrentUserName();
			var oldLobby = lobbyStorage.GetLobby(lobbyData.Content.Id);
			if (oldLobby == null)
			{
				throw new ArgumentOutOfRangeException(nameof(lobbyData.Content.Id), "Lobby does not exist.");
			}

			if (currentUserName != oldLobby.Host)
			{
				throw new UnauthorizedAccessException("Not authorized to update this lobby's settings.");
			}

			if (!lobbyData.Content.ValidateSettings())
			{
				throw new InvalidOperationException("Invalid settings for this lobby.");
			}

			lobbyData.Content.Host = oldLobby.Host;
			lobbyData.Content.Guests = oldLobby.Guests;
			lobbyData.Content.InvitedPlayers = oldLobby.InvitedPlayers;
            lobbyData.Content.Created = oldLobby.Created;
            lobbyData.Content.LastModified = DateTime.UtcNow;
			
			lobbyStorage.UpdateLobby(lobbyData.Content);
			var updatedLobby = mapper.Map<LobbyDataWrapper>(lobbyStorage.GetLobby(lobbyData.Content.Id));

			await notificationService.NotifyAllExceptAsync(currentUserName,
				client => client.LobbyChanged(updatedLobby));

			return updatedLobby;
		}

		public Task<LobbyDataWrapper> GetLobby(Guid lobbyId)
		{
            var lobby = lobbyStorage.GetLobby(lobbyId);
			return Task.FromResult(mapper.Map<LobbyDataWrapper>(lobby));
		}

		public Task<bool> LobbyExists(Guid lobbyId)
		{
            return Task.FromResult(lobbyStorage.LobbyExitsts(lobbyId));
		}

		public async Task<LobbyDataWrapper> CreateAndAddLobbyAsync(int gameIdentifier, LobbyAccess access, string name)
		{
            var currentUser = identityService.GetCurrentUserName();
            if (lobbyStorage.GetLobbyOfUser(currentUser) != null)
            {
                throw new InvalidOperationException("To create a new lobby, leave your current lobby first.");
            }

			var lobbyType = gameTypeMapping.GetLobbyDataType(gameIdentifier);
			if (!lobbyType.IsSubclassOf(typeof(LobbyData)))
			{
				throw new ArgumentException("Invalid lobby type.");
			}
			
			var lobby = (LobbyData) Activator.CreateInstance(lobbyType)!;
			lobby.Host = currentUser;
			lobby.Access = access;
			lobby.Name = name;
			lobbyStorage.AddLobby(lobby);
			var wrapper = mapper.Map<LobbyDataWrapper>(lobby);

			await notificationService.NotifyAllExceptAsync(currentUser,
				client => client.LobbyAdded(wrapper));

			return wrapper;
		}

		public Task<List<Message>> GetMessages(Guid lobbyId)
		{
			return Task.FromResult(lobbyStorage.GetMessages(lobbyId));
		}

		public async Task<LobbyDataWrapper> CancelInviteFromLobby(Guid lobbyId, string player)
        { 
			var lobby = lobbyStorage.GetLobby(lobbyId);
            if (identityService.GetCurrentUserName() != lobby.Host)
            {
                throw new UnauthorizedAccessException("Only the host can modify the lobby.");
            }

			lobby.InvitedPlayers.Remove(player);

			var wrapper = mapper.Map<LobbyDataWrapper>(lobby);
			await notificationService.NotifyAllExceptAsync(identityService.GetCurrentUserName(),
				client => client.LobbyChanged(wrapper));

			return wrapper;
        }

		public void RemoveLobby(Guid id)
		{
			lobbyStorage.RemoveLobby(id);
		}

		public Task<IEnumerable<string>> GetOthersInLobby(Guid lobbyId)
		{
			var lobby = lobbyStorage.GetLobby(lobbyId);
			var currentUser = identityService.GetCurrentUserName();

			return Task.FromResult(lobby.Guests.Append(lobby.Host)
				.Where(u => u != currentUser));
		}
    }
}
