using AutoMapper;
using Microblogging.Helper.Models;
using Microblogging.MVC.Models.Posts;
using Microblogging.Service.DTOs;
using Microblogging.Services.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text.Json;
using System.Text;
using Azure.Storage.Blobs;
using Microblogging.Helper.AzureBlobStorage;
using System.Net.Http.Headers;
using Microblogging.Data.Entities;

namespace Microblogging.MVC.Controllers
{
    public class PostsController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly HttpClient _httpClient;
        private readonly MVCAppSettings _appSettings;
        protected readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public PostsController(IWebHostEnvironment env,ILogger<AccountController> logger, HttpClient httpClient,IConfiguration configuration)
        {
            _logger = logger;
            _httpClient = httpClient;
            _configuration = configuration;
            _appSettings = _configuration.GetSection("APPSettings").Get<MVCAppSettings>();
            _env = env;


        }
        public IActionResult AddPost()
        {
            NewPostModel _model = new NewPostModel();
            return View(_model);
        }


        [HttpPost]
        public async Task<IActionResult> AddPost(NewPostModel _model)
        {
            try
            {




              
                if (ModelState.IsValid)
                {
                    var token = HttpContext.Session.GetString("AuthToken");
                    _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);




                    using var content = new MultipartFormDataContent();
                  
                        using var imageStream = _model.ImageFile.OpenReadStream();
                        var imageContent = new StreamContent(imageStream);
                        imageContent.Headers.ContentType = new MediaTypeHeaderValue(_model.ImageFile.ContentType);

                        content.Add(imageContent, "ImageFile", _model.ImageFile.FileName);
                    
                    content.Add(new StringContent(_model.Latitude.ToString()), "Latitude");
                    content.Add(new StringContent(_model.Longitude.ToString()), "Longitude");
                    content.Add(new StringContent(_model.Text), "Text");


                    var response = await _httpClient.PostAsync(_appSettings.APIBaseURL+ "/Posts/AddNewPost", content);

                    if (response.IsSuccessStatusCode == true)
                    {
                        return RedirectToAction("Index", "Home");

                    }
                   


                }
            }
            catch (Exception ex)
            {
            }
            return View();
        }



        //[HttpPost]
        //public  async Task<IActionResult> AddPost(NewPostModel _model)
        //{
        //    try
        //    {




        //        if (_model.ImageFile != null)
        //        {
        //            var UserName = HttpContext.Session.GetString("UserName");

        //            var res = await AzuriteService.UploadImage(_model.ImageFile.FileName, _model.ImageFile.OpenReadStream(), UserName);
        //            _model.Image = res;

        //        }
        //        if (ModelState.IsValid)
        //        {
        //            var token = HttpContext.Session.GetString("AuthToken");
        //            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        //            var json = JsonSerializer.Serialize(_model);
        //            var content = new StringContent(json, Encoding.UTF8, "application/json");

        //            var response = await _httpClient.PostAsync(_appSettings.APIBaseURL + "/Posts/AddNewPost", content);

        //            if (response.IsSuccessStatusCode == true)
        //            {
        //                return RedirectToAction("Index", "Home");

        //            }


        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    return View();
        //}
    }
}
