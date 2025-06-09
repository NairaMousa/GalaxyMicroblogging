using Microblogging.Helper.AzureBlobStorage;
using SixLabors.ImageSharp.Formats.Webp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Microblogging.Helper.ImageConverter
{
    public class ImageConverterService
    {
        private readonly List<(int width, int height)> _resizeDimensions = new()
    {
        (1920, 1080),
        (1280, 720),
        (800, 600),
        (400, 300)
    };

        public async Task ConvertAndResizeToWebPAsync(string inputPath, string UserName)
        {
            //if (!Directory.Exists(outputFolder))
            //    Directory.CreateDirectory(outputFolder);
            try
            {
                using var image = await Image.LoadAsync(inputPath);

                foreach (var (width, height) in _resizeDimensions)
                {
                    using var resized = image.Clone(x => x.Resize(new ResizeOptions
                    {
                        Mode = ResizeMode.Max,
                        Size = new Size(width, height)
                    }));

                    // var outputFile = Path.Combine(outputFolder, $"{Path.GetFileNameWithoutExtension(inputPath)}_{width}x{height}.webp");

                    var webpEncoder = new WebpEncoder
                    {
                        Quality = 75 // You can change quality between 0–100
                    };

                    var memoryStream = new MemoryStream();
                    await resized.SaveAsync(memoryStream, webpEncoder);
                    await AzuriteService.ReplaceImageAsync(UserName, memoryStream, inputPath);

                }


            }
            catch (Exception ex)
            {
            }
        }

    }
}
