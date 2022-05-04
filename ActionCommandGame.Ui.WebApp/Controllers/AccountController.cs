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
        //private readonly IPlayerApi _playerApi;
        private readonly ITokenStore _tokenStore;

        public AccountController(IIdentityApi identityApi, /*IPlayerApi playerApi,*/ ITokenStore tokenStore)
        {

            _identityApi = identityApi;
           // _playerApi = playerApi;
            _tokenStore = tokenStore;
        }

        public async Task<IActionResult> Logout(string returnUrl)
        {
            await _tokenStore.SaveTokenAsync("");

            return RedirectToLocal("/");
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
            /*Console.WriteLine("wip login");
            var signInModel = new SignInModel
            {
                ReturnUrl = returnUrl
            };*/
            if (returnUrl is null)
            {
                returnUrl = "/";
            }
            return View(new UserSignInRequest()
            {
                ReturnUrl = returnUrl
            });
        }
        [Route("/login")]
        [Route("/account/login")]
        [HttpPost]
        public async Task<IActionResult> Login(UserSignInRequest request)
        {
            var signInResult = await _identityApi.SignInAsync(request);
            if (!signInResult.Success)
            {
                if (signInResult.Errors is not null)
                {
                    foreach (var error in signInResult.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                return Login("/Login");
            }

            var token = signInResult.Token;

            //Save token for later use in the API
            await _tokenStore.SaveTokenAsync(token);

            return RedirectToLocal(request.ReturnUrl);
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