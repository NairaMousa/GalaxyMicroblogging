using System.Net.Http.Headers;
using System.Net;
using Microblogging.Helper.Models;

namespace Microblogging.MVC.TokenService
{
    public class AuthHandler : DelegatingHandler
    {
        private readonly ITokenService _tokenService;
       // private readonly MVCAppSettings _appSettings;
        public AuthHandler(ITokenService tokenService)
        {
            _tokenService = tokenService;
            //_appSettings = appSettings;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {


            
            var accessToken = await _tokenService.GetAccessTokenAsync();

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                var refreshToken = await _tokenService.GetRefreshTokenAsync();

                var newAccessToken = await _tokenService.RefreshAccessTokenAsync(refreshToken);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", newAccessToken);

                response = await base.SendAsync(request, cancellationToken);
            }

            return response;
        }
    }
}
