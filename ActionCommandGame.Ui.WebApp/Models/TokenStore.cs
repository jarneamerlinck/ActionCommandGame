using System.Threading.Tasks;
using ActionCommandGame.Sdk.Abstractions;

namespace ActionCommandGame.Ui.WebApp
{
    public class TokenStore: ITokenStore
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string _cookieName;

        public TokenStore(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _cookieName = "jwt_token";
        }
        public Task ClearTokenAsync()
        {
            _httpContextAccessor.HttpContext?.Response.Cookies.Delete(_cookieName);
            return Task.CompletedTask;
        }
        public Task<string> GetTokenAsync()
        {
            if (_httpContextAccessor.HttpContext is null || !_httpContextAccessor.HttpContext.Request.Cookies.ContainsKey(_cookieName))
            {
                return Task.FromResult("");
            }

            var token = _httpContextAccessor.HttpContext.Request.Cookies[_cookieName];
            if (token is null)
            {
                return Task.FromResult("");
            }
            return Task.FromResult(token);

        }

        public Task SaveTokenAsync(string token)
        {
            _httpContextAccessor.HttpContext?.Response.Cookies.Append(
                _cookieName,
                token,
                new CookieOptions
                {
                    HttpOnly = true
                });

            return Task.CompletedTask;
        }
    }
}
