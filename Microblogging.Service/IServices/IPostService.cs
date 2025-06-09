using Microblogging.Helper;
using Microblogging.Service.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microblogging.Services.IService
{
    public interface IPostService
    {
        Task<APIResponse<AddNewPost_ResDto>> AddNewPost(AddNewPost_ReqDto _ReqDto,string UserName,string Imagepath);
        Task<List<Posts_ResDto>> GetAllPosts();

    }
}
