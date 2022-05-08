using System.Threading.Tasks;
using ActionCommandGame.Sdk.Abstractions;

namespace ActionCommandGame.Ui.WebApp.Stores
{
    public class TokenStore: ITokenStore
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _tokenName = "jwt_token";

        public TokenStore(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            
        }

        public Task<string> GetTokenAsync()
        {
            if (_httpContextAccessor.HttpContext is null)
            {
                return Task.FromResult(string.Empty);
            }

            _httpContextAccessor.HttpContext.Request.Cookies.TryGetValue(_tokenName, out string? token);
            return Task.FromResult(token ?? "");
        }

        public Task SaveTokenAsync(string token)
        {
            if (_httpContextAccessor.HttpContext is null)
            {
                return Task.FromResult(string.Empty);
            }

            if (_httpContextAccessor.HttpContext.Request.Cookies.ContainsKey(_tokenName))
            {
                _httpContextAccessor.HttpContext.Response.Cookies.Delete(_tokenName);
            }
            _httpContextAccessor.HttpContext.Response.Cookies.Append(
                _tokenName,
                token,
                new CookieOptions
                {
                    HttpOnly = true
                });

            return Task.CompletedTask;
        }
    }
}
