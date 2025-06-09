using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microblogging.Helper.Enums
{
    public enum TokenEnum
    {
        [Description("Refresh Token")]
        RefreshToken = 101,
        [Description("Invalid Token")]
        InvalidToken = 102,
    }
}
