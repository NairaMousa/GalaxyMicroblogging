using System;
using System.Collections.Generic;

namespace Microblogging.Data.Entities;

public partial class Image
{
    public int Id { get; set; }

    public int FkPostId { get; set; }

    public string ImagePath { get; set; } = null!;

    public int FkStatus { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Post FkPost { get; set; } = null!;

    public virtual LkImageStatus FkStatusNavigation { get; set; } = null!;

    public virtual ICollection<ImageVariant> ImageVariants { get; set; } = new List<ImageVariant>();
}
