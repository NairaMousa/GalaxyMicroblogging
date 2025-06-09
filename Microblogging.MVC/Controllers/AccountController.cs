using Microblogging.Helper.Models;
using Microblogging.MVC.Models.Users;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;
using System.Diagnostics.Eventing.Reader;
using Microblogging.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microblogging.Helper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Microblogging.MVC.Controllers
{
    public class AccountController : Controller
    {

        private readonly ILogger<AccountController> _logger;
        private readonly HttpClient _httpClient;
        private readonly MVCAppSettings _appSettings;
        protected readonly IConfiguration _configuration;
        public AccountController(IConfiguration configuration ,ILogger<AccountController> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
            _configuration = configuration;

            _appSettings =  _configuration.GetSection("APPSettings").Get<MVCAppSettings>(); 
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel _model)
        {

           
                if (ModelState.IsValid)
                {
                    
                    var json = JsonSerializer.Serialize(_model);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await _httpClient.PostAsync(_appSettings.APIBaseURL + "/Users/Login", content);
                    if(response.StatusCode==System.Net.HttpStatusCode.OK) 
                        {
                        var jsonResult = await response.Content.ReadAsStringAsync();
                        var options = new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        };
                        
                        var user = JsonSerializer.Deserialize<APIResponse<LoginResModel>> (jsonResult, options);

                        // Store token in session or cookie
                        HttpContext.Session.SetString("AuthToken", user.Data.Token);
                        HttpContext.Session.SetString("UserName", _model.UserName);
                        Response.Cookies.Append("refreshToken", user.Data.refreshToken);
                        


                        // Redirect to protected area
                        return RedirectToAction("Index", "Home");
                    }

                    else
                    {
                        ModelState.AddModelError("", "Invalid login attempt.");
                        return View(_model);
                    }
                       
                }

                else
                {
                    return View(_model);
                }
         
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account"); // or wherever you want
        }

    }
}
