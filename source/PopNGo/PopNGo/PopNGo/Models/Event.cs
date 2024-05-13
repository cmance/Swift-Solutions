using System;
using System.Collections.Generic;

namespace PopNGo.Models;

public partial class Event
{
    public int Id { get; set; }

    public string ApiEventId { get; set; }

    public DateTime? EventDate { get; set; }

    public string EventName { get; set; }

    public string EventDescription { get; set; }

    public string EventLocation { get; set; }

    public string EventImage { get; set; }

    public string EventOriginalLink { get; set; }

    public decimal? Latitude { get; set; }

    public decimal? Longitude { get; set; }

    public string VenuePhoneNumber { get; set; }

    public string VenueName { get; set; }

    public decimal? VenueRating { get; set; }

    public string VenueWebsite { get; set; }

    public virtual ICollection<EventHistory> EventHistories { get; set; } = new List<EventHistory>();

    public virtual ICollection<FavoriteEvent> FavoriteEvents { get; set; } = new List<FavoriteEvent>();

    public virtual ICollection<ItineraryEvent> ItineraryEvents { get; set; } = new List<ItineraryEvent>();

    public virtual ICollection<TicketLink> TicketLinks { get; set; } = new List<TicketLink>();
}
