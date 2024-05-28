using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PopNGo.Models;

[Table("RecommendedEvent")]
public partial class RecommendedEvent
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("EventID")]
    public int EventId { get; set; }

    [Column("UserID")]
    public int UserId { get; set; }

    [ForeignKey("EventId")]
    [InverseProperty("RecommendedEvents")]
    public virtual Event Event { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("RecommendedEvents")]
    public virtual PgUser User { get; set; }
}
