using ActionCommandGame.Ui.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using ActionCommandGame.Sdk.Abstractions;
using ActionCommandGame.Services.Model.Filters;
using ActionCommandGame.Services.Model.Results;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;


namespace ActionCommandGame.Ui.WebApp.Controllers
{
    [Authorize]
    public class GameController : Controller
    {
        private readonly IIdentityApi _identityApi;
        private readonly ITokenStore _tokenStore;
        private readonly IPlayerStore _playerStore;
        private readonly IPlayerApi _playerApi;
        private readonly IItemApi _itemApi;
        private readonly IGameApi _gameApi;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string AuthSchemes = CookieAuthenticationDefaults.AuthenticationScheme;

        public GameController(IIdentityApi identityApi, 
                                ITokenStore tokenStore,
                                IPlayerStore playerStore,
                                IPlayerApi playerApi,
                                IItemApi itemApi,
                                IGameApi gameApi,
                                IHttpContextAccessor httpContextAccessor)
        {
            _identityApi = identityApi;
            _playerApi = playerApi;
            _itemApi = itemApi;
            _tokenStore = tokenStore;
            _playerStore = playerStore;
            _gameApi = gameApi;
            _httpContextAccessor = httpContextAccessor;
        }
        


        public async Task<IActionResult> Index()
        {
            var playerId = await _playerStore.GetTokenAsync();
            var player = await _playerApi.GetAsync(playerId);
            return View(player.Data);
        }

        [Route("/shop")]
        public async Task<IActionResult> Shop()
        {   

            var itemsRequest = await _itemApi.FindAsync();
            if (!itemsRequest.IsSuccess)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(itemsRequest.Data);
        }
        public async Task<IActionResult> Buy(ItemResult shopItem)
        {
            var playerId = await _playerStore.GetTokenAsync();
            var buyResult = await _gameApi.BuyAsync(playerId, shopItem.Id);
            return RedirectToAction("index");
        }
        public async Task<IActionResult> Mine()
        {
            var playerId = await _playerStore.GetTokenAsync();
            await _gameApi.PerformActionAsync(playerId);
  
            
            return RedirectToAction("LeaderBoard");

        }

        public IActionResult Inventory()
        {
            return RedirectToAction("index");
        }

        public async Task<IActionResult> LeaderBoard()
        {


            var playersResult = await _playerApi.Find(new PlayerFilter
            {
                FilterUserPlayers = false
            });


            if (!playersResult.IsSuccess)
            {
                return RedirectToAction("index", "Home");
            }
            
            var user = new User
            {
                Players = playersResult.Data,
                UserName = "Tester"

            };
            return View(user);
        }
        [HttpGet]
        public async Task<IActionResult> PickPlayer()
        {
            var user = await GetUser();
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> PickPlayer([FromForm] int id)
        {
            await _playerStore.SaveTokenAsync(id);
            return RedirectToAction("index");
            
        }





        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult CreatePlayer()
        {

            return View();
        }

        private async Task<User> GetUser()
        {
            var playersResult = await _playerApi.Find(new PlayerFilter
            {
                FilterUserPlayers = false
            });

            if (_httpContextAccessor.HttpContext is null|| !playersResult.IsSuccess)
            {
                return null;
            }


            var claimsIdentity = User.Identity as ClaimsIdentity;
            var emailClaim = claimsIdentity.FindFirst(ClaimTypes.Email);
            var username = emailClaim.Value;

            return new User
            {
                Players = playersResult.Data,
                UserName = username

            };
        }

        
    }
}