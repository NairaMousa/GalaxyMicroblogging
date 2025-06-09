using Microblogging.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microblogging.Repository.IRepo
{
    public interface IImageRepo
    {
        Task<int> AddImage(Image _image);
        Task<List<Image>> GetImagesWithPendingStatus();
        Task<bool> UpdateImage(int  ImageId,string ImagePath);
    }
}
