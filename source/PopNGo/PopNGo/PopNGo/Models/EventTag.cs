using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PopNGo.Models;

[Table("EventTag")]
public partial class EventTag
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    public int TagId { get; set; }

    public int EventId { get; set; }

    [ForeignKey("EventId")]
    [InverseProperty("EventTags")]
    public virtual Event Event { get; set; }

    [ForeignKey("TagId")]
    [InverseProperty("EventTags")]
    public virtual Tag Tag { get; set; }
}
