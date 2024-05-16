using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PopNGo.Models;

public partial class FavoriteEvent
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("BookmarkListID")]
    public int BookmarkListId { get; set; }

    [Column("EventID")]
    public int EventId { get; set; }

    [ForeignKey("BookmarkListId")]
    [InverseProperty("FavoriteEvents")]
    public virtual BookmarkList BookmarkList { get; set; }

    [ForeignKey("EventId")]
    [InverseProperty("FavoriteEvents")]
    public virtual Event Event { get; set; }
}
