﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Czeum.Abstractions.DTO;
using Czeum.Abstractions.DTO.Lobbies;
using Czeum.Application.Services.SoloQueue;
using Czeum.DAL;
using Czeum.DAL.Exceptions;
using Czeum.Domain.Services;
using Czeum.DTO;
using Czeum.DTO.Extensions;
using Czeum.DTO.Lobbies;
using Czeum.DTO.Wrappers;
using Microsoft.EntityFrameworkCore;

namespace Czeum.Application.Services.Lobby {
	public class LobbyService : ILobbyService
	{
		private readonly ILobbyStorage lobbyStorage;
		private readonly ApplicationDbContext context;
		private readonly IMapper mapper;
        private readonly IIdentityService identityService;
        private readonly ISoloQueueService soloQueueService;

        public LobbyService(ILobbyStorage lobbyStorage, 
			ApplicationDbContext context,
			IMapper mapper,
            IIdentityService identityService,
            ISoloQueueService soloQueueService)
		{
			this.lobbyStorage = lobbyStorage;
			this.context = context;
			this.mapper = mapper;
            this.identityService = identityService;
            this.soloQueueService = soloQueueService;
        }

		public async Task JoinToLobbyAsync(Guid lobbyId)
		{
            var currentUser = identityService.GetCurrentUser();
            var userLobby = lobbyStorage.GetLobbyOfUser(currentUser);
            if (userLobby != null || soloQueueService.IsQueuing(currentUser))
            {
                throw new InvalidOperationException("You can only join a lobby if you are not queuing and not in an other lobby.");
            }

			var lobby = lobbyStorage.GetLobby(lobbyId);

			var friends = await context.Friendships
				.Where(f => f.User1.UserName == currentUser || f.User2.UserName == currentUser)
				.Select(f => f.User1.UserName == currentUser ? f.User2.UserName : f.User1.UserName)
				.ToListAsync();
			
			lobby.JoinGuest(currentUser, friends);
		}

		public void DisconnectFromCurrentLobby()
		{
            var currentUser = identityService.GetCurrentUser();
            DisconnectPlayerFromLobby(currentUser);
		}

        public void DisconnectPlayerFromLobby(string username)
        {
            var currentLobby = lobbyStorage.GetLobbyOfUser(username);
            if (currentLobby != null)
            {
                currentLobby.DisconnectPlayer(username);
                if (currentLobby.Empty)
                {
                    lobbyStorage.RemoveLobby(currentLobby.Id);
                }
            }
            else
            {
                throw new InvalidOperationException("You are not in a lobby.");
            }
        }

        public void InvitePlayerToLobby(Guid lobbyId, string player)
		{
			var lobby = lobbyStorage.GetLobby(lobbyId);
			if (lobby.Host != identityService.GetCurrentUser())
			{
				throw new UnauthorizedAccessException("Not authorized to invite to this lobby.");
			}

			if (!lobby.InvitedPlayers.Contains(player))
			{
				lobby.InvitedPlayers.Add(player);
                lobby.LastModified = DateTime.UtcNow;
			}
            else
            {
                throw new InvalidOperationException("This player has already been invited.");
            }
		}

		public string KickGuest(Guid lobbyId)
		{
			var lobby = lobbyStorage.GetLobby(lobbyId);
			if (lobby.Host != identityService.GetCurrentUser())
			{
				throw new UnauthorizedAccessException("Not authorized to kick a player from this lobby.");
			}

			var guestName = lobby.Guest;
			if (lobby.Guest != null) 
			{
				lobby.DisconnectPlayer(guestName);
			}

			return guestName;
		}

		public LobbyData? GetLobbyOfUser(string user) {
			return lobbyStorage.GetLobbyOfUser(user);
		}

		public List<LobbyDataWrapper> GetLobbies()
		{
			return lobbyStorage.GetLobbies().Select(mapper.Map<LobbyDataWrapper>).ToList();
		}

		public void UpdateLobbySettings(LobbyDataWrapper lobbyData)
		{
			var oldLobby = lobbyStorage.GetLobby(lobbyData.Content.Id);
			if (oldLobby == null)
			{
				throw new ArgumentOutOfRangeException(nameof(lobbyData.Content.Id), "Lobby does not exist.");
			}

			if (identityService.GetCurrentUser() != oldLobby.Host)
			{
				throw new UnauthorizedAccessException("Not authorized to update this lobby's settings.");
			}

			lobbyData.Content.Host = oldLobby.Host;
			lobbyData.Content.Guest = oldLobby.Guest;
			lobbyData.Content.InvitedPlayers = oldLobby.InvitedPlayers;
            lobbyData.Content.Created = oldLobby.Created;
            lobbyData.Content.LastModified = DateTime.UtcNow;
			
			lobbyStorage.UpdateLobby(lobbyData.Content);
		}

		public LobbyDataWrapper GetLobby(Guid lobbyId)
		{
            var lobby = lobbyStorage.GetLobby(lobbyId);
			return mapper.Map<LobbyDataWrapper>(lobby);
		}

		public bool LobbyExists(Guid lobbyId)
		{
			return lobbyStorage.GetLobby(lobbyId) != null;
		}

		public LobbyDataWrapper CreateAndAddLobby(GameType type, string host, LobbyAccess access, string name)
		{
			var lobbyType = type.GetLobbyType();
			if (!lobbyType.IsSubclassOf(typeof(LobbyData)))
			{
				throw new ArgumentException("Invalid lobby type.");
			}
			
			var lobby = (LobbyData) Activator.CreateInstance(lobbyType)!;
			lobby.Host = host;
			lobby.Access = access;
			lobby.Name = string.IsNullOrEmpty(name) ? host + "'s lobby" : name;
			lobbyStorage.AddLobby(lobby);
			
			return mapper.Map<LobbyDataWrapper>(lobby);
		}

		public void AddMessageNow(Guid lobbyId, Message message)
		{
			message.Timestamp = DateTime.UtcNow;
			lobbyStorage.AddMessage(lobbyId, message);
		}

		public List<Message> GetMessages(Guid lobbyId)
		{
			return lobbyStorage.GetMessages(lobbyId);
		}

		public string GetOtherPlayer(Guid lobbyId, string player)
		{
			var lobby = lobbyStorage.GetLobby(lobbyId);
			return player == lobby.Host ? lobby.Guest : lobby.Host;
		}

		public void CancelInviteFromLobby(Guid lobbyId, string player)
        { 
			var lobby = lobbyStorage.GetLobby(lobbyId);
            if (identityService.GetCurrentUser() != lobby.Host)
            {
                throw new UnauthorizedAccessException("Only the host can modify the lobby.");
            }

			lobby.InvitedPlayers.Remove(player);
		}

		public void RemoveLobby(Guid id)
		{
			lobbyStorage.RemoveLobby(id);
		}
    }
}
