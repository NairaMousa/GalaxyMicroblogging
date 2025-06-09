using AutoMapper;
using Microblogging.Helper;
using Microblogging.Helper.Enums;
using Microblogging.Helper.Extensions;
using Microblogging.Helper.Models;
using Microblogging.Repository.IRepo;
using Microblogging.Service.DTOs;
using Microblogging.Service.IServices;
using Microblogging.Services.IService;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microblogging.Service.Services
{
    public class UserService : BaseService, IUserService
    {
        private readonly AppSettings _appSettings;

        public UserService(IContainerRepo ContainerRepo, IMapper mapper, IConfiguration configuration) : base(ContainerRepo, mapper, configuration)
        {
            _appSettings = _configuration.GetSection("APPSettings").Get<AppSettings>();

        }

        public  async Task<APIResponse<bool>> Login(string username, string password)
        {
            if(string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return new APIResponse<bool>
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    ResponseMessage = new List<string> { APIMessagesCodes.WrongUserNameOrPassword.GetDescription() },
                    
                };
            }
            else
            {
                var Result=_ContainerRepo.UserRepo.Login(username, password);
               
                    return new APIResponse<bool>
                    {
                        StatusCode = System.Net.HttpStatusCode.OK,
                       Data= Result.Result==true?true :false

                    };
                

            }
        }

        public async Task<bool> SaveRefreshToken(string refreshToken, string UserName)
        {
            var Result = await _ContainerRepo.UserRepo.SaveRefreshToken(refreshToken, UserName);
            return Result;
        }

       

        public async Task<string> ValidateRefreshToken(string refreshToken)
        {
            var Result = await _ContainerRepo.UserRepo.ValidateRefreshToken(refreshToken);
            return Result;
        }
    }
}
