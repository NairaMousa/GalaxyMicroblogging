using Microblogging.Helper.CustomAttributes;
using System.ComponentModel.DataAnnotations;

namespace Microblogging.API.Models
{
    public class AddNewPost_ReqModel
    {
        [Required]
        [MaxLength(140, ErrorMessage = "maximum post characters is 140")]
        public string Text { get; set; } = null!;

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        [MaxFileSize(2 * 1024 * 1024)] // 2 MB limit
        public IFormFile? ImageFile { get; set; } = default!;


        //public string Image { get; set; }
    }
}
