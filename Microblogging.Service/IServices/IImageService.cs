using Microblogging.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microblogging.Service.IServices
{
    public  interface IImageService
    {
        Task<bool> UpdateImage(int ImageId, string ImagePath);
        Task<List<Image>> GetImagesWithPendingStatus();
    }
}
