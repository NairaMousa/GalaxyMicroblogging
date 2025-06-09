using Microblogging.Data.Entities;
using Microblogging.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microblogging.Repository.IRepo
{
    public interface IPostsRepo
    {

        Task<int> AddNewPost(Post _post);
        Task<List<PostModel>> GetAllPosts();
    }
}
