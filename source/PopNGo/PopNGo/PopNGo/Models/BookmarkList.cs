using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PopNGo.Models;

[Table("BookmarkList")]
public partial class BookmarkList
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("UserID")]
    public int UserId { get; set; }

    [Required]
    [StringLength(128)]
    public string Title { get; set; }

    public string Image { get; set; }

    [InverseProperty("BookmarkList")]
    public virtual ICollection<FavoriteEvent> FavoriteEvents { get; set; } = new List<FavoriteEvent>();
}
