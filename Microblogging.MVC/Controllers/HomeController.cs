using Microblogging.Helper;
using Microblogging.Helper.Models;
using Microblogging.MVC.Models;
using Microblogging.MVC.Models.Posts;
using Microblogging.MVC.Models.Users;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;
using System.Text.Json;

namespace Microblogging.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly HttpClient _httpClient;
        private readonly MVCAppSettings _appSettings;
        protected readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public HomeController(IWebHostEnvironment env, ILogger<AccountController> logger, HttpClient httpClient, IConfiguration configuration)
        {
            _logger = logger;
            _httpClient = httpClient;
            _configuration = configuration;
            _appSettings = _configuration.GetSection("APPSettings").Get<MVCAppSettings>();
            _env = env;


        }
        public async Task<IActionResult> Index()
        {

            var token = HttpContext.Session.GetString("AuthToken");
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync(_appSettings.APIBaseURL + "/Posts/GetAllPosts");

            if (response.IsSuccessStatusCode == true)
            {
                var jsonResult = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var _posts = JsonSerializer.Deserialize<APIResponse<List<AllPostsModel>>>(jsonResult, options);
                if (_posts.StatusCode == HttpStatusCode.OK)
                {
                    return View(_posts.Data);
                }

                else
                    return View();
            }
            else
                return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
