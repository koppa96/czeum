using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Czeum.Core.DTOs;
using Czeum.Core.DTOs.Paging;
using Czeum.Core.DTOs.Wrappers;
using Czeum.Core.Services;
using Czeum.Web.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Czeum.Web.Controllers
{
    [Route(ApiResources.Matches.BasePath)]
    [ApiController]
    [Authorize]
    public class MatchesController : ControllerBase
    {
        private readonly IMatchService matchService;

        public MatchesController(IMatchService matchService)
        {
            this.matchService = matchService;
        }
        
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<IEnumerable<MatchStatus>>> GetMatchesAsync()
        {
            return Ok(await matchService.GetMatchesAsync());
        }

        [HttpGet("current")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<RollListDto<MatchStatus>>> GetCurrentMatchesAsync(Guid? oldestId, int count = 25)
        {
            return Ok(await matchService.GetCurrentMatchesAsync(oldestId, count));
        }

        [HttpGet("finished")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<RollListDto<MatchStatus>>> GetFinishedMatchesAsync(Guid? oldestId, int count = 25)
        {
            return Ok(await matchService.GetFinishedMatchesAsync(oldestId, count));
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<MatchStatus>> CreateMatchAsync([FromQuery] Guid lobbyId)
        {
            return StatusCode(201, await matchService.CreateMatchAsync(lobbyId));
        }

        [HttpPut("moves")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<MatchStatus>> MoveAsync([FromBody] MoveDataWrapper moveDataWrapper)
        {
            return Ok(await matchService.HandleMoveAsync(moveDataWrapper.Content));
        }
    }
}