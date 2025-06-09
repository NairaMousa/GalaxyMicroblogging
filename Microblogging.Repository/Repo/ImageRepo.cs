using Microblogging.Data.Entities;
using Microblogging.Helper.Enums;
using Microblogging.Helper.Extensions;
using Microblogging.Repository.IRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microblogging.Repository.Repo
{
    public class ImageRepo:IImageRepo
    {
        microbloggingContext _context;
        public ImageRepo(microbloggingContext context)
        {
            _context= context;
        }

        public async Task<int> AddImage(Image _image)
        {
            _context.Images.Add(_image); 
            return await _context.SaveChangesAsync();
        }

        public async Task<List<Image>> GetImagesWithPendingStatus()
        {
           return  _context.Images.Where(x=>x.FkStatus== ImageStatus.Pending.ToInt()).ToList();
        }

        public async Task<bool> UpdateImage(int ImageId,string ImagePath)
        {
             var Image=_context.Images.Where(x => x.Id == ImageId).FirstOrDefault();
             Image.FkStatus = ImageStatus.Done.ToInt();
            Image.ImagePath = ImagePath;
             _context.Images.Update(Image);
             _context.SaveChanges();
              return true;
        }
    }
}
