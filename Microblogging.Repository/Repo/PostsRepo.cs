using Microblogging.Data.Entities;
using Microblogging.Repository.IRepo;
using Microblogging.Repository.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microblogging.Repository.Repo
{
    public class PostsRepo: IPostsRepo
    {

        microbloggingContext _context;
        public PostsRepo(microbloggingContext context)
        {
            _context = context;
        }

        public async Task<int> AddNewPost(Post _post)
        {
            _context.Posts.Add(_post);
            await _context.SaveChangesAsync();
            return _post.Id;
        }

        public async Task<List<PostModel>> GetAllPosts()
        {

            return await _context.Posts.Include(o => o.Images).Include(x => x.FkUser)
                .Select(x=> new PostModel{ CreatedAt = x.CreatedAt, Text = x.Text, UserName = x.FkUser.Username, ImagePath =(x.Images!=null && x.Images.Count()>0 ?x.Images.FirstOrDefault().ImagePath:null) }).OrderByDescending(x=>x.CreatedAt).ToListAsync();


           
        }
    }
}
