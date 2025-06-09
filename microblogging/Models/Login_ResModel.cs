using System.Security.Claims;

namespace Microblogging.API.Models
{
    public class Login_ResModel
    {
        public string Token { get; set; }
        public double ExpirationMinutes { get; set; }

        public string refreshToken { get; set; }


    }
}
