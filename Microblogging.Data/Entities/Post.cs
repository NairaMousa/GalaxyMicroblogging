using System;
using System.Collections.Generic;

namespace Microblogging.Data.Entities;

public partial class Post
{
    public int Id { get; set; }

    public int FkUserId { get; set; }

    public string Text { get; set; } = null!;

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual User FkUser { get; set; } = null!;

    public virtual ICollection<Image> Images { get; set; } = new List<Image>();
}
