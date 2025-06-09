namespace Microblogging.MVC.TokenService
{
    public interface ITokenService
    {
        public Task<string> GetAccessTokenAsync();
        public Task<string> GetRefreshTokenAsync();

        public Task<string> RefreshAccessTokenAsync( string refreshToken);
    }
}
