using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PopNGo.Models;

[Table("SearchRecord")]
public partial class SearchRecord
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("UserID")]
    public int UserId { get; set; }

    [Required]
    [StringLength(255)]
    public string SearchQuery { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime Time { get; set; }
}
