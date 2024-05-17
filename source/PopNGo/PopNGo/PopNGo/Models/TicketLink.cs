using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PopNGo.Models;

[Table("TicketLink")]
public partial class TicketLink
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("EventID")]
    public int EventId { get; set; }

    [StringLength(255)]
    public string Source { get; set; }

    [StringLength(255)]
    public string Link { get; set; }

    [ForeignKey("EventId")]
    [InverseProperty("TicketLinks")]
    public virtual Event Event { get; set; }
}
