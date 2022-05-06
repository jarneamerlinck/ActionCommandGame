using ActionCommandGame.Ui.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ActionCommandGame.Sdk.Abstractions;
using ActionCommandGame.Services.Model.Filters;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;


namespace ActionCommandGame.Ui.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IIdentityApi _identityApi;
        private readonly ITokenStore _tokenStore;
        private readonly User _user;
        private readonly IPlayerApi _playerApi;
        private const string AuthSchemes = CookieAuthenticationDefaults.AuthenticationScheme;

        public HomeController(IIdentityApi identityApi, ITokenStore tokenStore, IPlayerApi playerApi)
        {
            _identityApi = identityApi;
            _playerApi = playerApi;
            _tokenStore = tokenStore;

            List<Player> temPlayers = new List<Player>
            {
                new Player
                {
                    Id = 0,
                    Cash = 539,
                    Name = "TestPlayer",
                    ImageLocation = "../images/playerImage_01.png"
                },
                new Player
                {
                    Id = 1,
                    Cash = 349,
                    Name = "SkyLander",
                    ImageLocation = "../images/playerImage_02.png"
                },
                new Player
                {
                    Id = 2,
                    Cash = 3249,
                    Name = "Eragon",
                    ImageLocation = "../images/playerImage_03.png"
                },
                new Player
                {
                    Id = 3,
                    Cash = 12,
                    Name = "Vader",
                    ImageLocation = "../images/playerImage_04.png"
                },
            };

            _user = new User
            {
                Id = 0, 
                Players = temPlayers
            };
        }
        
        public IActionResult Index()
        {
            return View(_user);
        }
        [Authorize]
        [Route("/shop")]
        public IActionResult Shop()
        {
            //_tokenStore.GetTokenAsync();
            //Console.WriteLine("wip here");
            return View(_user.Players[0]);
        }
        [Authorize]
        public IActionResult Buy(ShopItem shopItem)
        {

            return RedirectToAction("index");
        }
        [Authorize]
        public async Task<IActionResult> Mine()
        {
            var token = await _tokenStore.GetTokenAsync();
            if (token == null)
            {
                return RedirectToAction("index");
            }
            return RedirectToAction("LeaderBoard");

        }

        public IActionResult Inventory()
        {
            return RedirectToAction("index");
        }

        public async Task<IActionResult> LeaderBoard()
        {
            var playerResult = await _playerApi.Find(new PlayerFilter
            {
                FilterUserPlayers = false
            });
            if (!playerResult.IsSuccess)
            {
                return View("Index");
            }
            return View(playerResult.Data);
        }

        public IActionResult PickPlayer()
        {
            return View(_user);
        }





        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}