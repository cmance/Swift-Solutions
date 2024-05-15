using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PopNGo.Models;

public partial class PgUser
{
    public int Id { get; set; }

    public string AspnetuserId { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<EmailHistory> EmailHistories { get; set; } = new List<EmailHistory>();

    [InverseProperty("User")]
    public virtual ICollection<EventHistory> EventHistories { get; set; } = new List<EventHistory>();

    [InverseProperty("User")]
    public virtual ICollection<Itinerary> Itineraries { get; set; } = new List<Itinerary>();

    [InverseProperty("User")]
    public virtual ICollection<ScheduledNotification> ScheduledNotifications { get; set; } = new List<ScheduledNotification>();
}
