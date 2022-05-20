using ActionCommandGame.Api.Authentication.Extensions;
using ActionCommandGame.Services.Abstractions;
using ActionCommandGame.Services.Model.Requests;
using Microsoft.AspNetCore.Mvc;

namespace ActionCommandGame.Api.Controllers
{
    public class PositiveEventsController : ApiBaseController
    {
        private readonly IPositiveGameEventService _positiveGameEventService;

        public PositiveEventsController(IPositiveGameEventService positiveEventsService)
        {
            _positiveGameEventService = positiveEventsService;
        }

        [HttpGet("positive_events")]
        public async Task<IActionResult> Find()
        {
            var result = await _positiveGameEventService.FindAsync(User.GetId());
            return Ok(result);
        }
        [HttpPost("positive_events/{id}")]
        public async Task<IActionResult> Edit([FromBody] PositiveGameEventRequest request)
        {
            var result = await _positiveGameEventService.EditAsync(request.Id, request, User.GetId());
            return Ok(result);
        }
        [HttpPost("positive_events/")]
        public async Task<IActionResult> Create([FromBody] PositiveGameEventRequest request)
        {
            var result = await _positiveGameEventService.CreateAsync(request, User.GetId());
            return Ok(result);
        }
        [HttpPost("positive_events/Delete/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var result = await _positiveGameEventService.DeleteAsync(id, User.GetId());
            return Ok(result);
        }
    }
}
