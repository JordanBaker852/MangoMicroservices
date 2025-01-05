using Mango.Web.Service.IService;
using Mango.Web.Utility;

namespace Mango.Web.Service
{
    public class TokenProvider : ITokenProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TokenProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void ClearToken()
        {
            _httpContextAccessor.HttpContext?.Response.Cookies.Delete(StaticDetails.TOKEN_COOKIE_NAME);
        }

        public string? GetToken()
        {
            var token = _httpContextAccessor.HttpContext?.Request.Cookies.FirstOrDefault(cookie => cookie.Key == StaticDetails.TOKEN_COOKIE_NAME).Value;

            return token == null ? string.Empty : token;
        }

        public void SetToken(string token)
        {
            _httpContextAccessor.HttpContext?.Response.Cookies.Append(StaticDetails.TOKEN_COOKIE_NAME, token);
        }

        public void Dispose()
        {
            GC.Collect();
        }
    }
}
