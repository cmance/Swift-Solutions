using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PopNGo.Models;

[Table("Event")]
public partial class Event
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Required]
    [Column("ApiEventID")]
    [StringLength(255)]
    public string ApiEventId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EventDate { get; set; }

    [StringLength(255)]
    public string EventName { get; set; }

    [StringLength(1000)]
    public string EventDescription { get; set; }

    [StringLength(255)]
    public string EventLocation { get; set; }

    [StringLength(255)]
    public string EventImage { get; set; }

    [StringLength(255)]
    public string VenuePhoneNumber { get; set; }

    [StringLength(255)]
    public string VenueName { get; set; }

    [Column(TypeName = "decimal(2, 1)")]
    public decimal? VenueRating { get; set; }

    [StringLength(255)]
    public string VenueWebsite { get; set; }

    [InverseProperty("Event")]
    public virtual ICollection<EventHistory> EventHistories { get; set; } = new List<EventHistory>();

    [InverseProperty("Event")]
    public virtual ICollection<FavoriteEvent> FavoriteEvents { get; set; } = new List<FavoriteEvent>();

    [InverseProperty("Event")]
    public virtual ICollection<TicketLink> TicketLinks { get; set; } = new List<TicketLink>();
}
