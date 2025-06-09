using Microblogging.Helper.Models;
using Microblogging.MVC.Models.Users;
using System.Net.Http;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text;
using Newtonsoft.Json;
using Microblogging.Helper;
using System.Text.Json;

namespace Microblogging.MVC.TokenService
{
    public class TokenService:ITokenService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _contextAccessor;
        


        public TokenService(IHttpContextAccessor contextAccessor, IHttpClientFactory httpClientFactory)
        {
            _contextAccessor = contextAccessor;
            _httpClient = httpClientFactory.CreateClient("TokenClient");

        }

        public async Task<string> GetAccessTokenAsync()
        {
            return _contextAccessor.HttpContext.Session.GetString("AuthToken");
        }

        public async Task<string> GetRefreshTokenAsync()
        {
            return _contextAccessor.HttpContext.Request.Cookies["refreshToken"];
        }

        public async Task<string> RefreshAccessTokenAsync( string refreshToken)
        {
            var Refreshcontent = new StringContent(JsonConvert.SerializeObject(refreshToken), Encoding.UTF8, "application/json");
            // Call the API to refresh the access token
            var response = await _httpClient.PostAsync("api/Users/refresh", Refreshcontent);

            if (response.IsSuccessStatusCode)
            {

                var jsonResult = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var user = System.Text.Json.JsonSerializer.Deserialize<APIResponse<LoginResModel>>(jsonResult, options);
             
                _contextAccessor.HttpContext.Session.SetString("AuthToken", user.Data.Token);
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    Expires = DateTime.UtcNow.AddDays(7),
                    SameSite = SameSiteMode.Strict
                };
                _contextAccessor.HttpContext.Response.Cookies.Append("refreshToken", user.Data.refreshToken, cookieOptions);

                return user.Data.Token;
            }

            throw new UnauthorizedAccessException("Unable to refresh access token.");
        }
    }
}
