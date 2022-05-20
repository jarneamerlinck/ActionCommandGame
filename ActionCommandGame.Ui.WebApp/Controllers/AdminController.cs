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
        private readonly IPositiveEventApi _positiveEventApi;
        public AdminController(INegativeEventApi negativeEventApi,
            IPositiveEventApi positiveEventApi)
        {
            _negativeEventApi = negativeEventApi;
            _positiveEventApi = positiveEventApi;
        }

        public IActionResult Index()
        {
            return View();
        }
        // Start negative
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
        public async Task<IActionResult> NegativeEventDelete(int id)
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
        [HttpPost("Admin/DeleteNegative/{id}")]
        public async Task<IActionResult> ConfirmDeleteNegative([FromRoute] int id)
        {
            var result = await _negativeEventApi.DeleteAsync(id);
            return RedirectToAction("NegativeEvent");
        }
        //End Negative
        // Start Positive
        public async Task<IActionResult> PositiveEvent()
        {
            var events = await _positiveEventApi.FindAsync();
            if (!events.IsSuccess && events.Data is null)
            {
                return RedirectToAction("index");
            }
            return View(events.Data);


        }
        [HttpPost]
        public async Task<IActionResult> PositiveEventEdit([FromRoute] int id, [FromForm] PositiveGameEventRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(new PositiveGameEventResult()
                {
                    Id = id,
                    Money = request.Money,
                    Experience = request.Experience,
                    Description = request.Description,
                    Name = request.Name,
                    Probability = request.Probability
                });
            }

            request.Id = id;
            var result = await _positiveEventApi.EditAsync(id, request);
            if (!result.IsSuccess)
            {
                return View(new PositiveGameEventResult()
                {
                    Id = id,
                    Money = request.Money,
                    Experience = request.Experience,
                    Description = request.Description,
                    Name = request.Name,
                    Probability = request.Probability
                });
            }
            return RedirectToAction("PositiveEvent");
        }
        [HttpGet]
        public async Task<IActionResult> PositiveEventEdit(int id)
        {
            var events = await _positiveEventApi.FindAsync();

            if (!events.IsSuccess && events.Data is null)
            {
                return RedirectToAction("index");
            }
            var posEvent = events.Data.SingleOrDefault(e => e.Id == id);
            if (posEvent is null)
            {
                return RedirectToAction("index");
            }
            return View(posEvent);
        }
        [HttpGet]
        public IActionResult PositiveEventCreate()
        {
            var posEvent = new PositiveGameEventResult();
            return View(posEvent);
        }
        [HttpPost]
        public async Task<IActionResult> PositiveEventCreate([FromForm] PositiveGameEventRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(new PositiveGameEventResult
                {

                    Money = request.Money,
                    Experience = request.Experience,
                    Description = request.Description,
                    Name = request.Name,
                    Probability = request.Probability
                });
            }

            var result = await _positiveEventApi.CreateAsync(request);
            if (!result.IsSuccess)
            {
                return View(new PositiveGameEventResult
                {

                    Money = request.Money,
                    Experience = request.Experience,
                    Description = request.Description,
                    Name = request.Name,
                    Probability = request.Probability
                });
            }
            return RedirectToAction("PositiveEvent");
        }
        [HttpGet]
        public async Task<IActionResult> PositiveEventDelete(int id)
        {
            var events = await _positiveEventApi.FindAsync();

            if (!events.IsSuccess && events.Data is null)
            {
                return RedirectToAction("index");
            }
            var posEvent = events.Data.SingleOrDefault(e => e.Id == id);
            if (posEvent is null)
            {
                return RedirectToAction("index");
            }

            return View(posEvent);
        }
        [HttpPost("Admin/DeletePositive/{id}")]
        public async Task<IActionResult> ConfirmDeletePositive([FromRoute] int id)
        {
            var result = await _positiveEventApi.DeleteAsync(id);
            return RedirectToAction("PositiveEvent");
        }
        //End Positive
    }
}
