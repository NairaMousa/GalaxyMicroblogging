using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microblogging.Service.DTOs
{
    public class Posts_ResDto
    {
        public string  UserName { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }

        public string ImagePath { get; set; }

       
    }
}
