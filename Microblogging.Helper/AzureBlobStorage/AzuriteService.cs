using Azure.Storage.Blobs;
using SixLabors.ImageSharp.Formats.Webp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microblogging.Helper.AzureBlobStorage
{
    public static class AzuriteService
    {



        public async static Task<string> ConvertToWebPAsync(string ImagePath)
        {
            var SplittedString = ImagePath.Split('/');
            string connectionString = "UseDevelopmentStorage=true";

            var blobServiceClient = new BlobServiceClient(connectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient(SplittedString[SplittedString.Length-2]);


           
            await containerClient.CreateIfNotExistsAsync();
           
            var originalBlob = containerClient.GetBlobClient(SplittedString.Last());

            // Download the original image
            using var originalStream = new MemoryStream();
            await originalBlob.DownloadToAsync(originalStream);
            originalStream.Position = 0;

            // Load and convert to WebP
            using var image = await Image.LoadAsync(originalStream);
            image.Mutate(x => x.Resize(new ResizeOptions
            {
                Mode = ResizeMode.Max,
                Size = new Size(800, 600) // optional resize
            }));

            var webpStream = new MemoryStream();
            var webpEncoder = new WebpEncoder { Quality = 75 };
            await image.SaveAsync(webpStream, webpEncoder);
            webpStream.Position = 0;
            var SplitImagePath = SplittedString.Last().Split('.');
            // Upload WebP image
            var outputBlob = containerClient.GetBlobClient(SplitImagePath.First()+".webp");
            await outputBlob.UploadAsync(webpStream, overwrite: true);
            return outputBlob.Uri.ToString();
        }

        public static async Task ReplaceImageAsync(string UserName, Stream imageStream ,string blobPath)
        {
            string connectionString = "UseDevelopmentStorage=true";
            string containerName = "images " + UserName;

            var blobServiceClient = new BlobServiceClient(connectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            await containerClient.CreateIfNotExistsAsync();

            var blobClient = containerClient.GetBlobClient(blobPath);

            // This will overwrite the existing blob if it already exists
            await blobClient.UploadAsync(imageStream, overwrite: true);
        }
        public static async Task<string> UploadImage(string ImageName,Stream Image,string UserName)
        {
            try
            {
                string connectionString = "UseDevelopmentStorage=true";
                string containerName = "images"+UserName.ToLower();

                string blobName = Path.GetFileName(ImageName);
                // Create Blob service and container clients
                var blobServiceClient = new BlobServiceClient(connectionString);
                var containerClient = blobServiceClient.GetBlobContainerClient(containerName);

                await containerClient.CreateIfNotExistsAsync();
                containerClient.SetAccessPolicy(Azure.Storage.Blobs.Models.PublicAccessType.Blob);


                var blobClient = containerClient.GetBlobClient(blobName);

                // Upload the image
                //using FileStream fileStream = File.OpenRead(localImagePath);
                await blobClient.UploadAsync(Image, overwrite: true);
                return blobClient.Uri.ToString();
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static async Task<Stream> DownloadImage(string ImageName)
        {

            try
            {
                string connectionString = "UseDevelopmentStorage=true";
                string containerName = "images";

                var blobServiceClient = new BlobServiceClient(connectionString);
                var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
                var blobClient = containerClient.GetBlobClient(ImageName);

                if (!await blobClient.ExistsAsync())
                {
                    return null;
                }

                var stream = await blobClient.OpenReadAsync();

                return stream; // or detect MIME type
            }
            catch (Exception ex)
            {
                return null;

            }

        }
        }
    }
