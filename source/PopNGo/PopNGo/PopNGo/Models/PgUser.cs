using System;
using System.Collections.Generic;

namespace PopNGo.Models;

public partial class PgUser
{
    public int Id { get; set; }

    public string AspnetuserId { get; set; }

    public virtual ICollection<EmailHistory> EmailHistories { get; set; } = new List<EmailHistory>();

    public virtual ICollection<EventHistory> EventHistories { get; set; } = new List<EventHistory>();

    public virtual ICollection<Itinerary> Itineraries { get; set; } = new List<Itinerary>();

    public virtual ICollection<ScheduledNotification> ScheduledNotifications { get; set; } = new List<ScheduledNotification>();
}
