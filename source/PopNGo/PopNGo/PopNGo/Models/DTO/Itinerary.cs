namespace PopNGo.Models.DTO
{
    public class Itinerary
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string ItineraryTitle { get; set; }
        public List<ItineraryEventDTO> Events { get; set; } // Ensure this refers to Event DTO
        public List<ItineraryNotificationDTO> Notifications { get; set; }
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
                Events = dayEvent.ItineraryEvents.Select(e => e.ToDTO()).ToList(),
                Notifications = dayEvent.ItineraryNotifications.Select(n => n.ToDTO()).ToList()
            };
        } 
    }
}