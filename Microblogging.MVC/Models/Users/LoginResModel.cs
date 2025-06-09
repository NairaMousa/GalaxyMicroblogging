namespace Microblogging.MVC.Models.Users
{
    public class LoginResModel
    {
        public string Token { get; set; }
        public double ExpirationMinutes { get; set; }

        public string refreshToken { get; set; }

    }
}
