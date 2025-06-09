using System;
using System.Collections.Generic;

namespace Microblogging.Data.Entities;

public partial class LkImageStatus
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Image> Images { get; set; } = new List<Image>();
}
