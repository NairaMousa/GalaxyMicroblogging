using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Microblogging.Helper.JWT
{
    public class JWTHelper
    {
        public static string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
   
        public static string GenerateJwtToken(string username, string key, string issuer, int Expires)
        {
            var claims = new[]
            {
            new Claim(ClaimTypes.Name, username)
        };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(issuer,
                issuer,
                claims,
                expires: DateTime.UtcNow.AddHours(Expires),
                signingCredentials: credentials);




            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
