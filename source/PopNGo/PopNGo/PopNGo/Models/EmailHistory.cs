using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PopNGo.Models;

[Table("EmailHistory")]
public partial class EmailHistory
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("UserID")]
    public int UserId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime TimeSent { get; set; }

    [Required]
    [StringLength(255)]
    public string Type { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("EmailHistories")]
    public virtual PgUser User { get; set; }
}
