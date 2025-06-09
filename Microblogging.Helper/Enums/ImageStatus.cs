using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microblogging.Helper.Enums
{
    public enum ImageStatus
    {
        [Description("Pending")]
        Pending = 1,
        [Description("Processing")]
        Processing = 2,
        [Description("Done")]
        Done = 3,
    }
}
