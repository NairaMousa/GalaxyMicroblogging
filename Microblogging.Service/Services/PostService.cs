using AutoMapper;
using Azure;
using Azure.Core;
using Microblogging.Data.Entities;
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
    public class PostService:BaseService, IPostService
    {
        private readonly AppSettings _appSettings;

        public PostService(IContainerRepo ContainerRepo, IMapper mapper, IConfiguration configuration) :base(ContainerRepo, mapper, configuration)
        {
            _appSettings = _configuration.GetSection("APPSettings").Get<AppSettings>();

        }

        public async Task<APIResponse<AddNewPost_ResDto>> AddNewPost(AddNewPost_ReqDto _ReqDto, string UserName,string Imagepath )
        {
            try
            {
                User user = new User();
                if (string.IsNullOrEmpty(_ReqDto.Text))
                    {
                    return new APIResponse<AddNewPost_ResDto>
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        ResponseMessage = new List<string> { APIMessagesCodes.EmptyText.GetDescription() }
                    };

                }
                if ( !string.IsNullOrEmpty(UserName))
                {
                    user= await _ContainerRepo.UserRepo.GetUserByName(UserName);
                    if (user==null)
                    {
                        return new APIResponse<AddNewPost_ResDto>
                        {
                            StatusCode = System.Net.HttpStatusCode.BadRequest,
                            ResponseMessage = new List<string> { APIMessagesCodes.UserNotExist.GetDescription() }
                        };

                    }
                }
                else
                {
                    return new APIResponse<AddNewPost_ResDto>
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        ResponseMessage = new List<string> { APIMessagesCodes.UserNotExist.GetDescription() }
                    };
                }

                using var transaction = await _ContainerRepo.BeginTransactionAsync();

                try
                {
                    var PostId = await _ContainerRepo.PostsRepo.AddNewPost(new Post()
                    {
                        CreatedAt = DateTime.Now,
                        FkUserId = user.Id,
                        Text = _ReqDto.Text,
                        Latitude = _ReqDto.Latitude,
                        Longitude = _ReqDto.Longitude
                    });

                    if (Imagepath != null)
                    {
                        //Adding Image To Azure Blob



                        //Adding Image Object To Database

                        var result = await _ContainerRepo.ImageRepo.AddImage(new Image()
                        {
                            CreatedAt = DateTime.Now,
                            FkPostId = PostId,
                            ImagePath = Imagepath,
                            FkStatus = ImageStatus.Pending.ToInt()
                        });

                    }
                    transaction.Commit();
                    return new APIResponse<AddNewPost_ResDto>
                    {
                        StatusCode = System.Net.HttpStatusCode.OK
                    };
                }
                catch
                {
                    await transaction.RollbackAsync();
                    return new APIResponse<AddNewPost_ResDto>
                    {
                        StatusCode = System.Net.HttpStatusCode.InternalServerError
                    };
                    throw;
                }

            }
            catch (Exception ex)
            {
                return new APIResponse<AddNewPost_ResDto>
                {
                    StatusCode = System.Net.HttpStatusCode.InternalServerError
                };

            }
        }

        public async Task<List<Posts_ResDto>> GetAllPosts()
        {
            try
            {
                var response = await _ContainerRepo.PostsRepo.GetAllPosts();
                var Posts = _mapper.Map<List<Posts_ResDto>>(response);
                return Posts;
            }
            catch (Exception ex)
            {
               return null;

            }
        }
    }
}
