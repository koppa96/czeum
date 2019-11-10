using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Czeum.Api.Common;
using Czeum.Application.Services.Lobby;
using Czeum.Core.DTOs.Lobbies;
using Czeum.Core.DTOs.Wrappers;
using Czeum.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Czeum.Api.Controllers
{
    [Route(ApiResources.Lobbies.BasePath)]
    [ApiController]
    [Authorize]
    public class LobbiesController : ControllerBase
    {
        private readonly ILobbyService lobbyService;

        public LobbiesController(ILobbyService lobbyService)
        {
            this.lobbyService = lobbyService;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<IEnumerable<LobbyDataWrapper>>> GetLobbies()
        {
            return await lobbyService.GetLobbies();
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<LobbyDataWrapper>> CreateLobbyAsync([FromBody] CreateLobbyDto dto)
        {
            return StatusCode(201, await lobbyService.CreateAndAddLobbyAsync(
                dto.GameType,  
                dto.LobbyAccess, 
                dto.Name));
        }

        [HttpPut("{lobbyId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<LobbyDataWrapper>> UpdateLobbyAsync([FromBody] LobbyDataWrapper wrapper)
        {
            return Ok(await lobbyService.UpdateLobbySettingsAsync(wrapper));
        }

        [HttpPost("current/leave")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> LeaveLobbyAsync()
        {
            await lobbyService.DisconnectFromCurrentLobbyAsync();
            return Ok();
        }

        [HttpPost("{lobbyId}/invite")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<LobbyDataWrapper>> InvitePlayerAsync(Guid lobbyId, [FromQuery] string playerName)
        {
            return Ok(await lobbyService.InvitePlayerToLobby(lobbyId, playerName));
        }

        [HttpDelete("{lobbyId}/invite")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<LobbyDataWrapper>> CancelInvitationAsync(Guid lobbyId, [FromQuery] string playerName)
        {
            return Ok(await lobbyService.CancelInviteFromLobby(lobbyId, playerName));
        }

        [HttpPost("{lobbyId}/join")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<LobbyDataWrapper>> JoinLobbyAsync(Guid lobbyId)
        {
            return Ok(await lobbyService.JoinToLobbyAsync(lobbyId));
        }

        [HttpPost("{lobbyId}/kick/{guestName}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<LobbyDataWrapper>> KickGuestAsync(Guid lobbyId, string guestName)
        {
            return Ok(await lobbyService.KickGuestAsync(lobbyId, guestName));
        }
    }
}