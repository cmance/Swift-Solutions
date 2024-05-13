using System;
using System.Collections.Generic;

namespace PopNGo.Models;

public partial class BookmarkList
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Title { get; set; }

    public string Image { get; set; }

    public virtual ICollection<FavoriteEvent> FavoriteEvents { get; set; } = new List<FavoriteEvent>();
}
