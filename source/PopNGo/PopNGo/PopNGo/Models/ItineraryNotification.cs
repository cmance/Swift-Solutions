using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PopNGo.Models;

public partial class ItineraryNotification
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("ItineraryID")]
    public int ItineraryId { get; set; }

    [Required]
    [StringLength(255)]
    public string NotificationAddress { get; set; }

    public bool OptOut { get; set; }
    [Required]
    public string OptOutCode { get; set; }

    [ForeignKey("ItineraryId")]
    [InverseProperty("ItineraryNotifications")]
    public virtual Itinerary Itinerary { get; set; }
}
