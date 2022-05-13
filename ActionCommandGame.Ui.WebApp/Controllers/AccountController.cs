using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;
using ActionCommandGame.Api.Authentication.Model;
using ActionCommandGame.Sdk.Abstractions;
using ActionCommandGame.Ui.WebApp.Models;
using ActionCommandGame.Sdk;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;



namespace ActionCommandGame.Ui.WebApp.Controllers
{
    public class AccountController : Controller
    {

        private readonly IIdentityApi _identityApi;
        private readonly ITokenStore _tokenStore;


        public AccountController(IIdentityApi identityApi, ITokenStore tokenStore)
        {

            _identityApi = identityApi;
            _tokenStore = tokenStore;
            
        }

        public async Task<IActionResult> Logout(string returnUrl)
        {
            await _tokenStore.SaveTokenAsync(string.Empty);
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserSignInRequest request)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Username/password is not valid");
                return View(request.ReturnUrl);
            }
            if (request.Email is null || request.Password is null)
            {
                return Login(request.ReturnUrl);
            }
            var signInResult = await _identityApi.SignInAsync(request);
            if (!signInResult.Success|| signInResult.Token is null)
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


            await _tokenStore.SaveTokenAsync(token);
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.Email, request.Email));


            await HttpContext.SignInAsync(new ClaimsPrincipal(identity));
            

            return RedirectToAction("PickPlayer", "Game");
        }

        [Route("/register")]
        [Route("/account/register")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([FromForm] UserRegistrationRequest request)
        {

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Username/password is not valid");
                return View(request.ReturnUrl);
            }
            if (request.Email is null || request.Password is null)
            {
                return Login(request.ReturnUrl);
            }
            var registerResult = await _identityApi.RegisterAsync(request);
            if (!registerResult.Success || registerResult.Token is null)
            {
                if (registerResult.Errors is not null)
                {
                    foreach (var error in registerResult.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                return Login("/Register");
            }

            var token = registerResult.Token;


            await _tokenStore.SaveTokenAsync(token);
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.Email, request.Email));


            await HttpContext.SignInAsync(new ClaimsPrincipal(identity));


            return RedirectToAction("CreatePlayer", "Game");
        }
        [Route("/register")]
        [Route("/account/register")]
        [HttpGet]
        public IActionResult Register(string returnUrl)
        {
            
            var registerModel = new UserRegistrationRequest
            {
                ReturnUrl = returnUrl
            };
            return View(registerModel);
        }
        public IActionResult ForgotPassword(string returnUrl)
        {
            var registerModel = new UserRegistrationRequest
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