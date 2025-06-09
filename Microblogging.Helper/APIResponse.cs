using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Microblogging.Helper
{
    public class APIResponse<T>
    {
        public T Data { get; set; }
        public List<string> ResponseMessage { get; set; }
        public HttpStatusCode StatusCode { get; set; }

        public bool IsOK() { return StatusCode == HttpStatusCode.OK; }
    }
}
