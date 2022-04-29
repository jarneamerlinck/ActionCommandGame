using System.Threading.Tasks;
using ActionCommandGame.Sdk.Abstractions;

namespace ActionCommandGame.Ui.WebApp
{
    public class TokenStore: ITokenStore
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TokenStore(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public Task<string> GetTokenAsync()
        {
            if (_httpContextAccessor.HttpContext is null || !_httpContextAccessor.HttpContext.Request.Cookies.ContainsKey("token"))
            {
                return Task.FromResult("");
            }

            var token = _httpContextAccessor.HttpContext.Request.Cookies["token"];
            if (token is null)
            {
                return Task.FromResult("");
            }
            return Task.FromResult(token);

        }

        public Task SaveTokenAsync(string token)
        {
            _httpContextAccessor.HttpContext.Response.Cookies.Append(
                "token",
                token,
                new CookieOptions
                {
                    HttpOnly = true
                });

            return Task.CompletedTask;
        }
    }
}
