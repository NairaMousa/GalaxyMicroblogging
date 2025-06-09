using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microblogging.Helper.Enums
{
    public enum APIMessagesCodes
    {
        [Description("Succeeded")]
        Success = 200,
        [Description("Text is Empty")]
        EmptyText = 1000,
        [Description("User Not Exist")]
        UserNotExist = 1001,

        [Description("Wrong UserName Or Password")]
        WrongUserNameOrPassword = 1002

    }
}
