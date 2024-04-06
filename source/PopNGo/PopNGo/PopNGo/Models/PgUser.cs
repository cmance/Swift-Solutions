using System;
using System.Collections.Generic;

namespace PopNGo.Models;

public partial class PgUser
{
    public int Id { get; set; }

    public string AspnetuserId { get; set; }

    public virtual ICollection<EventHistory> EventHistories { get; set; } = new List<EventHistory>();

    public virtual ICollection<FavoriteEvent> FavoriteEvents { get; set; } = new List<FavoriteEvent>();

    public virtual ICollection<ScheduledNotification> ScheduledNotifications { get; set; } = new List<ScheduledNotification>();
}
