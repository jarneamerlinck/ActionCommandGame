using System.Diagnostics;
using System.Threading.Tasks;
using ActionCommandGame.Api.Authentication.Model;
using ActionCommandGame.Sdk.Abstractions;
using ActionCommandGame.Ui.WebApp.Models;
using ActionCommandGame.Sdk;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;



namespace ActionCommandGame.Ui.WebApp.Controllers
{
    public class AccountController : Controller
    {

        private readonly IIdentityApi _identityApi;
        private readonly IPlayerApi _playerApi;
        private readonly ITokenStore _tokenStore;

        public AccountController(IIdentityApi identityApi, IPlayerApi playerApi, ITokenStore tokenStore)
        {
            _identityApi = identityApi;
            _playerApi = playerApi;
            _tokenStore = tokenStore;
        }

        public IActionResult Logout(string returnUrl)
        {

            return Login(returnUrl);
        }
        public IActionResult AccessDenied(string returnUrl)
        {

            return View();
        }

        [Route("/login")]
        [Route("/account/login")]
        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            var signInModel = new SignInModel
            {
                ReturnUrl = returnUrl
            };
            return View(signInModel);
        }

        [HttpPost]
        public async Task<IActionResult> Login(SignInModel signInModel)
        {
            /*
            if (!ModelState.IsValid)
            {
                return View(signInModel.ReturnUrl);
            }*/

            var loginResult = await _identityApi.SignInAsync(new UserSignInRequest 
                { Email = signInModel.Username, Password = signInModel.Password });

            if (loginResult.Success && loginResult.Token is not null)
            {

                await _tokenStore.SaveTokenAsync(loginResult.Token);
            }
            else if (loginResult.Errors is not null)
            {
                
                return RedirectToLocal(signInModel.ReturnUrl);
            }

            return RedirectToLocal("/");
        }
        public IActionResult Register(string returnUrl)
        {
            var registerModel = new RegisterModel
            {
                ReturnUrl = returnUrl
            };
            return View(registerModel);
        }
        public IActionResult ForgotPassword(string returnUrl)
        {
            var registerModel = new RegisterModel
            {
                ReturnUrl = "/Shop"
            };
            return View(registerModel);
        }
        
        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                return Login(returnUrl);
            }

            return LocalRedirect(returnUrl);
        }
    }
}