using AutoMapper;
using Azure;
using Hangfire;
using Microblogging.API.DTOs;
using Microblogging.API.JWT;
using Microblogging.API.Models;
using Microblogging.Data.Entities;
using Microblogging.Helper;
using Microblogging.Service.IServices;
using Microblogging.Services.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace Microblogging.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private  readonly ILogger<UsersController> _logger;
        private readonly IConfiguration _config;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public UsersController(ILogger<UsersController> logger,IConfiguration config, IUserService userService, IMapper mapper)
        {
            _config = config;
            _userService = userService;
            _mapper = mapper;
            _logger = logger;

        }




        [HttpPost("login")]
        public async Task<ActionResult<APIResponse<Login_ResModel>>>  Login([FromBody] LoginDto login)
        {

            //try
            //{
                if (ModelState.IsValid)
                {

                    var hasher = new PasswordHasher<string>();
                    string hashedPassword = hasher.HashPassword(null, login.Password);
                    login.Password = hashedPassword;
                    // You can validate the user manually here
                    if (_userService.Login(login.Username, login.Password).Result.Data)
                    {
                        var token = JWTHelper.GenerateJwtToken(
                            login.Username,
                            _config["JwtSettings:SecretKey"],
                            _config["JwtSettings:Issuer"],
                          int.Parse(_config["JwtSettings:ExpirationMinutes"].ToString())
                        );


                        var refreshToken = JWTHelper.GenerateRefreshToken();
                        //save RefreshToken in db
                        _userService.SaveRefreshToken(refreshToken, login.Username);
                        var result = new APIResponse<Login_ResModel>()
                        {
                            StatusCode = System.Net.HttpStatusCode.OK,
                            Data = new Login_ResModel() { refreshToken = refreshToken, Token = token, ExpirationMinutes = int.Parse(_config["JwtSettings:ExpirationMinutes"].ToString()) }
                        };


                        return Ok(result);
                    }
                    return Unauthorized();
                }

                else
                {
                    return BadRequest();
                }
           // }
            //catch (Exception ex)
            //{
            //    _logger.LogError(ex, "Something went wrong in DoWork()");
            //    return StatusCode(500, new { message = "An unexpected error occurred." });
            //}


        }



        [HttpPost("refresh")]
        public async Task<ActionResult<APIResponse<Login_ResModel>>> Refresh(string refreshtokn)
        {


            string UserName=await _userService.ValidateRefreshToken(refreshtokn);
            // For demonstration, let's just generate a new access token
            var newAccessToken = JWTHelper.GenerateJwtToken(
                        UserName,
                        _config["JwtSettings:SecretKey"],
                        _config["JwtSettings:Issuer"],
                      int.Parse(_config["JwtSettings:ExpirationMinutes"].ToString())
                    );
            string newRefreshToken=JWTHelper.GenerateRefreshToken();
           await _userService.SaveRefreshToken(newRefreshToken, UserName);
            var response = new Login_ResModel
            {
                 Token= newAccessToken,
                 refreshToken = newRefreshToken, 
                 ExpirationMinutes= int.Parse(_config["JwtSettings:ExpirationMinutes"].ToString())
            };
            var result = new APIResponse<Login_ResModel>()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Data = response
            };


            return Ok(result);
        }
               

           
        
    }
}
