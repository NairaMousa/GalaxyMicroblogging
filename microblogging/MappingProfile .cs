using AutoMapper;
using Microblogging.API.Models;
using Microblogging.Repository.Models;
using Microblogging.Service.DTOs;

namespace Microblogging.API
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Example mapping
            CreateMap<AddNewPost_ReqModel, AddNewPost_ReqDto>().ReverseMap();
            CreateMap<AddNewPost_ResModel, AddNewPost_ResDto>().ReverseMap();
            CreateMap<AllPosts_ResModel, Posts_ResDto>().ReverseMap();
            CreateMap<Posts_ResDto, PostsModel>().ReverseMap();
            CreateMap<Login_ResModel, RefreshTokenDto>().ReverseMap();







        }
    }
}
