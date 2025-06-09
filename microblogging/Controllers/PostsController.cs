using AutoMapper;
using Azure.Core;
using Microblogging.API.DTOs;
using Microblogging.API.JWT;
using Microblogging.API.Models;
using Microblogging.Helper;
using Microblogging.Helper.AzureBlobStorage;
using Microblogging.Service.DTOs;
using Microblogging.Services.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Microblogging.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {

        private readonly IConfiguration _config;

        private readonly IPostService _postService;
        private readonly IMapper _mapper;
        public PostsController(IConfiguration config, IPostService postService, IMapper mapper)
        {
            _config = config;
            _postService = postService;
            _mapper = mapper;

        }

        [HttpPost("AddNewPost")]
        
        [Authorize]
        public async Task<ActionResult<APIResponse<AddNewPost_ResModel>>> AddNewPost([FromForm] AddNewPost_ReqModel _ReqModel)
        {
           
                string ImagePath=null;
                if (ModelState.IsValid)
                {
                    var UserName = User.Identity.Name; // 👈 Extract from token

                    if (_ReqModel.ImageFile != null)
                    {
                      

                        var res = await AzuriteService.UploadImage(_ReqModel.ImageFile.FileName, _ReqModel.ImageFile.OpenReadStream(), UserName);
                        ImagePath = res;

                    }


                    var response = await _postService.AddNewPost(_mapper.Map<AddNewPost_ReqDto>(_ReqModel), UserName, ImagePath);
                   // var result = _mapper.Map<APIResponse<AddNewPost_ResModel>>(response);
                    return StatusCode((int)response.StatusCode, response);
                }
                else
                    return BadRequest(ModelState);
            
           

           
        }
        [HttpGet("GetAllPosts")]
        [Authorize]
        public async Task<ActionResult<APIResponse<List<AllPosts_ResModel>>>> GetAllPosts()
        {
            try
            {

                var response = await _postService.GetAllPosts();
                var result = _mapper.Map<List<AllPosts_ResModel>>(response);
                var _apiresponse = new APIResponse<List<AllPosts_ResModel>>()
                {
                    Data= result,
                    StatusCode=System.Net.HttpStatusCode.OK
                };
                return StatusCode((int)_apiresponse.StatusCode, _apiresponse);
            }
            catch(Exception ex) { return null; }
           

        }
    }
}
