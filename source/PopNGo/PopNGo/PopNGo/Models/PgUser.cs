using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PopNGo.Models;

[Table("PG_User")]
public partial class PgUser
{
    public int Id { get; set; }

    public string AspnetuserId { get; set; }

    public virtual ICollection<EventHistory> EventHistories { get; set; } = new List<EventHistory>();

    [InverseProperty("User")]
    public virtual ICollection<ScheduledNotification> ScheduledNotifications { get; set; } = new List<ScheduledNotification>();
}
