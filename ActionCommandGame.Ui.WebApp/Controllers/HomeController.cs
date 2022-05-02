using ActionCommandGame.Ui.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ActionCommandGame.Sdk.Abstractions;
using Microsoft.AspNetCore.Authorization;


namespace ActionCommandGame.Ui.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IIdentityApi _identityApi;
        private readonly ITokenStore _tokenStore;
        private readonly User _user;

        public HomeController(IIdentityApi identityApi, ITokenStore tokenStore)
        {
            _identityApi = identityApi;
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

        public IActionResult Buy(ShopItem shopItem)
        {

            return RedirectToAction("index");
        }
        [Authorize]
        public IActionResult Mine()
        {
            return RedirectToAction("index");
        }

        public IActionResult Inventory()
        {
            return RedirectToAction("index");
        }

        public IActionResult LeaderBoard()
        {
            return View(_user);
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