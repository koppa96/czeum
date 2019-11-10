using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Czeum.Api.Common;
using Czeum.Core.DTOs;
using Czeum.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Czeum.Api.Controllers.Messages
{
    [Route(ApiResources.Messages.Lobby.BasePath)]
    [ApiController]
    public class LobbyMessagesController : ControllerBase
    {
        private readonly IMessageService messageService;
        public LobbyMessagesController(IMessageService messageService)
        {
            this.messageService = messageService;
        }

        [HttpGet("{lobbyId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public ActionResult<IEnumerable<Message>> GetMessages(Guid lobbyId)
        {
            return Ok(messageService.GetMessagesOfLobby(lobbyId));
        }

        [HttpPost("{lobbyId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<Message>> SendMessage(Guid lobbyId, [FromBody] string message)
        {
            return Ok(await messageService.SendToLobbyAsync(lobbyId, message));
        }
    }
}