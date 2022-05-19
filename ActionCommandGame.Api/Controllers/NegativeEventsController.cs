using ActionCommandGame.Api.Authentication.Extensions;
using ActionCommandGame.Services.Abstractions;
using ActionCommandGame.Services.Model.Requests;
using Microsoft.AspNetCore.Mvc;

namespace ActionCommandGame.Api.Controllers
{
    public class NegativeEventsController : ApiBaseController
    {
        private readonly INegativeGameEventService _negativeGameEventService;

        public NegativeEventsController(INegativeGameEventService negativeEventsService)
        {
            _negativeGameEventService = negativeEventsService;
        }

        [HttpGet("negative_events")]
        public async Task<IActionResult> Find()
        {
            var result = await _negativeGameEventService.FindAsync(User.GetId());
            return Ok(result);
        }
        [HttpPost("negative_events/{id}")]
        public async Task<IActionResult> Edit([FromBody] NegativeGameEventRequest request)
        {
            var result = await _negativeGameEventService.EditAsync(request.Id, request, User.GetId());
            return Ok(result);
        }
    }
}
