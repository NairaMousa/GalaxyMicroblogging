using Microblogging.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microblogging.Repository.IRepo
{
    public interface IUserRepo
    {
        Task<User> GetUserByName(string UserName);
        Task<bool> Login(string username, string password);
        Task<bool> SaveRefreshToken(string refreshToken,string  UserName);

        Task<string> ValidateRefreshToken(string refreshToken);

    }
}
