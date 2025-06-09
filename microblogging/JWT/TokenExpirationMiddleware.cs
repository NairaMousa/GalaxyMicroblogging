using Microblogging.Helper.Enums;
using Microblogging.Service.IServices;
using Microsoft.Extensions.Azure;
using System.IdentityModel.Tokens.Jwt;

namespace Microblogging.API.JWT
{
    public class TokenExpirationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<TokenExpirationMiddleware> _logger;
        //private readonly ITokenService _tokenService;
        private readonly IConfiguration _config;
        public TokenExpirationMiddleware(IConfiguration config,RequestDelegate next, ILogger<TokenExpirationMiddleware> logger)
        {
            _next = next;
            _logger = logger;
           // _tokenService = tokenService;
            _config = config;
        }

        public async Task Invoke(HttpContext context)
        {
            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
            {
                var token = authHeader.Substring("Bearer ".Length).Trim();

                var tokenHandler = new JwtSecurityTokenHandler();
                try
                {
                    var jwtToken = tokenHandler.ReadJwtToken(token);
                    var exp = jwtToken.ValidTo;

                    //if (DateTime.UtcNow > exp)
                    //{


                        //  var refreshToken = context.Request.Cookies["refreshToken"];
                        //  if (refreshToken != null)
                        //  {
                        //      var obj = await JWTHelper.RefreshAccessToken(refreshToken, "",
                        //           _config["JwtSettings:SecretKey"],
                        //  _config["JwtSettings:Issuer"],
                        //int.Parse(_config["JwtSettings:ExpirationMinutes"].ToString()));
                        //      if (obj.refreshToken != null)
                        //      {

                        //          context.Response.OnStarting(() =>
                        //          {
                        //              context.Response.Headers["X-New-Access-Token"] = obj.Token;
                        //              context.Response.Headers["X-New-refresh-Token"] = obj.refreshToken;
                        //              return Task.CompletedTask;
                        //          });


                        //          context.Request.Headers["Authorization"] = $"Bearer {obj.Token}";
                        //      }
                        //  }


                        _logger.LogWarning("Access token is expired.");

                        context.Response.StatusCode = 403;
                        await context.Response.WriteAsync(TokenEnum.RefreshToken.ToString());
                        return;
                    //}
                }
                catch (Exception ex)
                {
                    _logger.LogError("Invalid token: " + ex.Message);
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync(TokenEnum.InvalidToken.ToString());
                    return;
                }
            }

            await _next(context);
        }
    }
}
