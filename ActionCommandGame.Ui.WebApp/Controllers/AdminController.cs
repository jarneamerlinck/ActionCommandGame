using ActionCommandGame.Sdk.Abstractions;
using ActionCommandGame.Services.Model.Requests;
using ActionCommandGame.Services.Model.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ActionCommandGame.Ui.WebApp.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        private readonly INegativeEventApi _negativeEventApi;
        public AdminController(INegativeEventApi negativeEventApi)
        {
            _negativeEventApi = negativeEventApi;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> NegativeEvent()
        {
            var events = await _negativeEventApi.FindAsync();
            if (!events.IsSuccess && events.Data is  null)
            {
                return RedirectToAction("index");
            }
            return View(events.Data);


        }
        [HttpPost]
        public async Task<IActionResult> NegativeEventEdit([FromRoute] int id, [FromForm] NegativeGameEventRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(new NegativeGameEventResult
                {
                    Id = id,
                    DefenseLoss = request.DefenseLoss,
                    DefenseWithGearDescription = request.DefenseWithGearDescription,
                    Description = request.Description,
                    Name = request.Name,
                    DefenseWithoutGearDescription = request.DefenseWithoutGearDescription,
                    Probability = request.Probability
                });
            }

            request.Id = id;
            var result = await _negativeEventApi.EditAsync(id, request);
            if (!result.IsSuccess)
            {
                return View(new NegativeGameEventResult
                {
                    Id = id,
                    DefenseLoss = request.DefenseLoss,
                    DefenseWithGearDescription = request.DefenseWithGearDescription,
                    Description = request.Description,
                    Name = request.Name,
                    DefenseWithoutGearDescription = request.DefenseWithoutGearDescription,
                    Probability = request.Probability
                });
            }
            return RedirectToAction("NegativeEvent");
        }
        [HttpGet]
        public async Task<IActionResult> NegativeEventEdit(int id)
        {
            var events = await _negativeEventApi.FindAsync();
            
            if (!events.IsSuccess && events.Data is null)
            {
                return RedirectToAction("index");
            }
            var negEvent = events.Data.SingleOrDefault(e => e.Id == id);
            if (negEvent is null)
            {
                return RedirectToAction("index");
            }
            return View(negEvent);
        }
        [HttpGet]
        public IActionResult NegativeEventCreate()
        {
            var negEvent = new NegativeGameEventResult();
            return View(negEvent);
        }
        [HttpPost]
        public async Task<IActionResult> NegativeEventCreate([FromForm] NegativeGameEventRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(new NegativeGameEventResult
                {
                    
                    DefenseLoss = request.DefenseLoss,
                    DefenseWithGearDescription = request.DefenseWithGearDescription,
                    Description = request.Description,
                    Name = request.Name,
                    DefenseWithoutGearDescription = request.DefenseWithoutGearDescription,
                    Probability = request.Probability
                });
            }

            var result = await _negativeEventApi.CreateAsync(request);
            if (!result.IsSuccess)
            {
                return View(new NegativeGameEventResult
                {
                    
                    DefenseLoss = request.DefenseLoss,
                    DefenseWithGearDescription = request.DefenseWithGearDescription,
                    Description = request.Description,
                    Name = request.Name,
                    DefenseWithoutGearDescription = request.DefenseWithoutGearDescription,
                    Probability = request.Probability
                });
            }
            return RedirectToAction("NegativeEvent");
        }
        [HttpGet]
        public IActionResult NegativeEventDelete(int eventId)
        {
            return View();
        }
        [HttpPost("Admin/Delete/{id?}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            
            return Ok();
        }
    }
}
