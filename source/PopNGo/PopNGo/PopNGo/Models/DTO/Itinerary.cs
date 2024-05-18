namespace PopNGo.Models.DTO
{
    public class Itinerary
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string ItineraryTitle { get; set; }
        public List<Event> Events { get; set; } // Ensure this refers to Event DTO

        public string EventName { get; set; }

        public string ApiEventId { get; set; }

        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
    }
}

namespace PopNGo.ExtensionMethods
{
    public static class ItineraryExtensions
    {
        public static PopNGo.Models.DTO.Itinerary ToDTO(this PopNGo.Models.Itinerary dayEvent)
        {
            return new PopNGo.Models.DTO.Itinerary
            {
                Id = dayEvent.Id,
                UserId = dayEvent.UserId,
                ItineraryTitle = dayEvent.ItineraryTitle,
                Events = dayEvent.ItineraryEvents.Select(e => e.Event.ToDTO()).ToList(),
                // EventName = dayEvent.ItineraryEvents.Select(e => e.Event.EventName).FirstOrDefault(),// Convert each ItineraryEvent to EventDTO
                // ApiEventId = dayEvent.ItineraryEvents.Select(e => e.Event.ApiEventId).FirstOrDefault(),
                // Latitude = dayEvent.ItineraryEvents.Select(e => e.Event.Latitude).FirstOrDefault(),
                // Longitude = dayEvent.ItineraryEvents.Select(e => e.Event.Longitude).FirstOrDefault()

            };
        } 
    }
}