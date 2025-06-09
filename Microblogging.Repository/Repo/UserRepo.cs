using Microblogging.Data.Entities;
using Microblogging.Repository.IRepo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microblogging.Repository.Repo
{
    public class UserRepo: IUserRepo
    {
        microbloggingContext _context;
        public UserRepo(microbloggingContext context) 
        {
            _context = context;
        }

        public async Task<User> GetUserByName(string  UserName)
        {
            return await _context.Users.Where(x => x.Username == UserName).FirstOrDefaultAsync()
;


           
        }

        public async Task<bool> Login(string username, string password)
        {
            if(_context.Users.Where(x=>x.Username==username && x.PasswordHash==password).FirstOrDefaultAsync()!=null)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> SaveRefreshToken(string refreshToken, string Username)
        {
            var refreshtokenobj = _context.RefreshTokens.Where(x => x.FkUser.Username == Username).FirstOrDefaultAsync();
            if (refreshtokenobj.Result != null)
            {
                refreshtokenobj.Result.RefreshToken1 = refreshToken;
                _context.RefreshTokens.Update(refreshtokenobj.Result);
               await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                int UserId= _context.Users.Where(x => x.Username == Username).FirstOrDefault().Id;
                _context.RefreshTokens.Add(new RefreshToken() {RefreshToken1=refreshToken,FkUserId=UserId });
               await _context.SaveChangesAsync();
                return true;
            }
                
        }

        public async Task<string> ValidateRefreshToken(string refreshToken)
        {
            var refreshtokenobj = await _context.RefreshTokens.Where(x => x.RefreshToken1 == refreshToken).Include(x=>x.FkUser).FirstOrDefaultAsync();
            if (refreshtokenobj != null)
            {
                return refreshtokenobj.FkUser.Username;
            }
            return null;
        }
    }
}
