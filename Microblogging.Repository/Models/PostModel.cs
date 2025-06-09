using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microblogging.Repository.Models
{
    public class PostModel
    {
        public string Text { get; set; }

        public string UserName { get; set; }

        public string ImagePath { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
