using Microblogging.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microblogging.Service.IServices
{
    public interface IUserService
    {
      

        Task<APIResponse<bool>> Login(string username, string password);
        Task<bool> SaveRefreshToken(string refreshToken, string Username);

        Task<string> ValidateRefreshToken(string refreshToken);
    }
}
