using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PopNGo.Models;

[Table("FavoriteEvents")]
public partial class FavoriteEvent
{
    public int Id { get; set; }

    [Column("BookmarkListID")]
    public int BookmarkListId { get; set; }

    public int EventId { get; set; }

    [ForeignKey("BookmarkListId")]
    [InverseProperty("FavoriteEvents")]
    public virtual BookmarkList BookmarkList { get; set; }

    [ForeignKey("EventId")]
    [InverseProperty("FavoriteEvents")]
    public virtual Event Event { get; set; }
}
