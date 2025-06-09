using AutoMapper;
using Hangfire;
using Hangfire.Common;
using Microblogging.Data.Entities;
using Microblogging.Helper.AzureBlobStorage;
using Microblogging.Helper.ImageConverter;
using Microblogging.Service.IServices;

namespace Microblogging.API.HangFire
{
    public class StartupJobInitializer
    {
        private readonly IRecurringJobManager _jobManager;
     
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;
        public StartupJobInitializer(IRecurringJobManager jobManager, IImageService imageService, IMapper mapper)
        {
            _jobManager = jobManager;
            _imageService= imageService;
            _mapper = mapper;
        }

        public void Schedule()
        {
            _jobManager.AddOrUpdate(
                "UpdateImageJob"
                ,()=>ConvertImage(),
                Cron.Hourly);
        }

        public async Task ConvertImage()
        {
           List<Image> _images= await _imageService.GetImagesWithPendingStatus();
//ImageConverterService _converter = new ImageConverterService();
            foreach (var item in _images)
            {
               item.ImagePath= await AzuriteService.ConvertToWebPAsync(item.ImagePath);
               // _converter.ConvertAndResizeToWebPAsync(item.ImagePath, "");
                await _imageService.UpdateImage(item.Id,item.ImagePath);
            }
        }
    }
}
