using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PopNGo.Models;

[Table("Itinerary")]
public partial class Itinerary
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("UserID")]
    public int UserId { get; set; }

    [Required]
    [StringLength(255)]
    public string ItineraryTitle { get; set; }

    [InverseProperty("Itinerary")]
    public virtual ICollection<ItineraryEvent> ItineraryEvents { get; set; } = new List<ItineraryEvent>();

    [InverseProperty("Itinerary")]
    public virtual ICollection<ItineraryNotification> ItineraryNotifications { get; set; } = new List<ItineraryNotification>();

    [ForeignKey("UserId")]
    [InverseProperty("Itineraries")]
    public virtual PgUser User { get; set; }
}
