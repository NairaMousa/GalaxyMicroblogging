using System;
using System.Collections.Generic;

namespace Microblogging.Data.Entities;

public partial class ImageVariant
{
    public int Id { get; set; }

    public int FkImageId { get; set; }

    public int Width { get; set; }

    public int Height { get; set; }

    public string WebpUrl { get; set; } = null!;

    public virtual Image FkImage { get; set; } = null!;
}
