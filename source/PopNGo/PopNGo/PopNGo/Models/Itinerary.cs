using System;
using System.Collections.Generic;

namespace PopNGo.Models;

public partial class Itinerary
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string ItineraryTitle { get; set; }

    public virtual ICollection<ItineraryEvent> ItineraryEvents { get; set; } = new List<ItineraryEvent>();

    public virtual PgUser User { get; set; }
}
