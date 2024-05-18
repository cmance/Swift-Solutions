using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PopNGo.Models;

public partial class ItineraryEvent
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("ItineraryID")]
    public int ItineraryId { get; set; }

    [Column("EventID")]
    public int EventId { get; set; }

    [Column("ReminderTime")]
    public string ReminderTime { get; set; }

    [Column("ReminderCustomTime", TypeName = "datetime")]
    public DateTime? ReminderCustomTime { get; set; }

    [ForeignKey("EventId")]
    [InverseProperty("ItineraryEvents")]
    public virtual Event Event { get; set; }

    [ForeignKey("ItineraryId")]
    [InverseProperty("ItineraryEvents")]
    public virtual Itinerary Itinerary { get; set; }
}
