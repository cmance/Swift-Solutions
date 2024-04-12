using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PopNGo.Models;

[Table("Event")]
public partial class Event
{
    public int Id { get; set; }

    public string ApiEventId { get; set; }

    public DateTime EventDate { get; set; }

    public string EventName { get; set; }

    public string EventDescription { get; set; }

    public string EventLocation { get; set; }

    public string EventImage { get; set; }

    public virtual ICollection<EventHistory> EventHistories { get; set; } = new List<EventHistory>();

    public virtual ICollection<FavoriteEvent> FavoriteEvents { get; set; } = new List<FavoriteEvent>();
}
