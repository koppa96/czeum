﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Czeum.Abstractions.DTO;
using Czeum.Abstractions.DTO.Lobbies;
using Czeum.DTO;
using Czeum.DTO.Lobbies;
using Czeum.DTO.Wrappers;

namespace Czeum.Application.Services.Lobby
{
    /// <summary>
    /// Interface for lobby handling services.
    /// </summary>
    public interface ILobbyService
    {
        /// <summary>
        /// Joins a player to the specified lobby.
        /// </summary>
        /// <param name="lobbyId">The target lobby's identifier</param>
        /// <returns>Whether the joining was successful</returns>
        Task JoinToLobbyAsync(Guid lobbyId);

        /// <summary>
        /// Disconnects the current user from their current lobby.
        /// </summary>
        void DisconnectFromCurrentLobby();

        /// <summary>
        /// Disconnects the player from their current lobby.
        /// </summary>
        /// <param name="username">The name of the player</param>
        void DisconnectPlayerFromLobby(string username);

        /// <summary>
        /// Adds a player to the invited players of the given lobby.
        /// </summary>
        /// <param name="lobbyId">The identifier of the lobby</param>
        /// <param name="player">The player to be invited</param>
        void InvitePlayerToLobby(Guid lobbyId, string player);

        /// <summary>
        /// Removes a player from the invited players of the given lobby.
        /// </summary>
        /// <param name="lobbyId">The identifier of the lobby</param>
        /// <param name="player">The player to be removed</param>
        void CancelInviteFromLobby(Guid lobbyId, string player);

        /// <summary>
        /// Kicks the current guest of the lobby.
        /// </summary>
        /// <param name="lobbyId">The identifier of the lobby</param>
        /// <returns>The kicked guest's name</returns>
        string KickGuest(Guid lobbyId);

        /// <summary>
        /// Finds the lobby to which the given user is currently joined.
        /// </summary>
        /// <param name="user">The name of the user</param>
        /// <returns>The lobby</returns>
        LobbyData? GetLobbyOfUser(string user);

        /// <summary>
        /// Gets a list of the current lobbies stored in the LobbyStorage.
        /// </summary>
        /// <returns>The list of lobbies</returns>
        List<LobbyDataWrapper> GetLobbies();

        /// <summary>
        /// Updates the settings of a lobby.
        /// </summary>
        /// <param name="lobbyData">The lobby with the updated settings</param>
        void UpdateLobbySettings(LobbyDataWrapper lobbyData);

        /// <summary>
        /// Gets a lobby with the given identifier.
        /// </summary>
        /// <param name="lobbyId">The identifier of the lobby</param>
        /// <returns>The lobby</returns>
        LobbyDataWrapper GetLobby(Guid lobbyId);
        
        /// <summary>
        /// Determines whether there is a lobby with the given identifier.
        /// </summary>
        /// <param name="lobbyId">The identifier of the lobby</param>
        /// <returns>Whether the lobby exists</returns>
        bool LobbyExists(Guid lobbyId);

        /// <summary>
        /// Creates a lobby of the desired type and adds it to the lobby storage.
        /// </summary>
        /// <param name="type">The type of the new lobby (must be a LobbyData subclass)</param>
        /// <param name="host">The name of the player that hosts the lobby</param>
        /// <param name="access">The access type of the lobby</param>
        /// <param name="name">The name of the lobby</param>
        /// <returns>The created lobby</returns>
        LobbyDataWrapper CreateAndAddLobby(GameType type, string host, LobbyAccess access, string name);

        /// <summary>
        /// Returns the other user of the lobby.
        /// </summary>
        /// <param name="lobbyId">The identifier of the lobby</param>
        /// <param name="player">The name of the player</param>
        /// <returns>The other user's name</returns>
        string GetOtherPlayer(Guid lobbyId, string player);

        /// <summary>
        /// Removes a lobby from the storage.
        /// </summary>
        /// <param name="id">The identifier of the lobby</param>
        void RemoveLobby(Guid id);
    }
}
