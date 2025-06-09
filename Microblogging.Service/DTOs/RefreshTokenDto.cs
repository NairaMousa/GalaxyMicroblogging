using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microblogging.Service.DTOs
{
    public class RefreshTokenDto
    {
        public string Token { get; set; }
        public double ExpirationMinutes { get; set; }

        public string refreshToken { get; set; }
    }
}
