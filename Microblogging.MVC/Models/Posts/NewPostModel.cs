using Microblogging.Helper.CustomAttributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Microblogging.MVC.Models.Posts
{
    public class NewPostModel
    {
      
        [Required]
        [MaxLength(140,ErrorMessage = "maximum post characters is 140")]
        public string Text { get; set; } = null!;

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        [MaxFileSize(2 * 1024 * 1024)] // 2 MB limit
        public IFormFile  ImageFile { get; set; } = default!;


    }
}
