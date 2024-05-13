using System;
using System.Collections.Generic;

namespace PopNGo.Models;

public partial class ItineraryEvent
{
    public int Id { get; set; }

    public int ItineraryId { get; set; }

    public int EventId { get; set; }

    public virtual Event Event { get; set; }

    public virtual Itinerary Itinerary { get; set; }
}
