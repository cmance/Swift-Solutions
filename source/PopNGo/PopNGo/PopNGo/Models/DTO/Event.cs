using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace PopNGo.Models.DTO
{
    public class Event
    {
        public int Id { get; set; }
        public string ApiEventID { get; set; }
        public DateTime EventDate { get; set; }
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
        public IEnumerable<PopNGo.Models.DTO.TicketLink> TicketLinks { get; set; }
        public int? ItineraryId { get; set; }

        public IEnumerable<PopNGo.Models.DTO.Tag> Tags { get; set; }
    }
}

namespace PopNGo.ExtensionMethods
{
    public static class EventExtensions
    {
        public static PopNGo.Models.DTO.Event ToDTO(this PopNGo.Models.Event Event)
        {
            return new PopNGo.Models.DTO.Event
            {
                Id = Event.Id,
                ApiEventID = Event.ApiEventId,
                EventDate = Event.EventDate ?? default(DateTime),
                EventDescription = Event.EventDescription,
                EventLocation = Event.EventLocation,
                EventName = Event.EventName,
                EventImage = Event.EventImage,
                EventOriginalLink = Event.EventOriginalLink,
                Latitude = Event.Latitude,
                Longitude = Event.Longitude,
                VenueName = Event.VenueName,
                VenueRating = Event.VenueRating,
                VenueWebsite = Event.VenueWebsite,
                VenuePhoneNumber = Event.VenuePhoneNumber,
                TicketLinks = Event.TicketLinks.Select(t => t.ToDTO()).ToList(),
                ItineraryId = Event.ItineraryEvents.FirstOrDefault()?.ItineraryId, // safely handling null and providing default
                Tags = Event.EventTags.Select(t => t.Tag.ToDTO()).ToList()
            };
        }
    }
}
