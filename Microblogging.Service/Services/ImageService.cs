using AutoMapper;
using Microblogging.Data.Entities;
using Microblogging.Helper.Models;
using Microblogging.Repository.IRepo;
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
    public class ImageService : BaseService, IImageService
    {
        private readonly AppSettings _appSettings;

        public ImageService(IContainerRepo ContainerRepo, IMapper mapper, IConfiguration configuration) : base(ContainerRepo, mapper, configuration)
        {
            _appSettings = _configuration.GetSection("APPSettings").Get<AppSettings>();

        }

        public async Task<List<Image>> GetImagesWithPendingStatus()
        {
            List<Image> _list=await _ContainerRepo.ImageRepo.GetImagesWithPendingStatus();
            return _list;
        }

        public async Task<bool> UpdateImage(int ImageId, string ImagePath)
        {
            var result = await _ContainerRepo.ImageRepo.UpdateImage(ImageId,ImagePath);
            return result;
        }
    }
}
