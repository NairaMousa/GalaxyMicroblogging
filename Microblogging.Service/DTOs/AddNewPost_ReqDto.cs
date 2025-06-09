using Microblogging.Data.Entities;
using Microblogging.Helper.CustomAttributes;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microblogging.Service.DTOs
{
    public class AddNewPost_ReqDto
    {
      
        public string Text { get; set; } = null!;

        public double Latitude { get; set; }

        public double Longitude { get; set; }
        public string Image { get; set; }


    }
}
