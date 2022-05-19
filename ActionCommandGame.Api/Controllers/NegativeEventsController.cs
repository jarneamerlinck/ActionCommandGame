using ActionCommandGame.Api.Authentication.Extensions;
using ActionCommandGame.Services.Abstractions;
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
    }
}
