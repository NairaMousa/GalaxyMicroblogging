using System;
using System.Collections.Generic;

namespace Microblogging.Data.Entities;

public partial class RefreshToken
{
    public int Id { get; set; }

    public int FkUserId { get; set; }

    public string RefreshToken1 { get; set; } = null!;

    public virtual User FkUser { get; set; } = null!;
}
