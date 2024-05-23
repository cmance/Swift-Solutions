namespace PopNGo.Models.DTO
{
    public class ItineraryEventDTO
    {
        public int Id { get; set; }
        public int ItineraryId { get; set; }
        public int EventId { get; set; }
        public string ReminderTime { get; set; }
        public DateTime ReminderCustomTime { get; set; }
        public DTO.Event EventDetails { get; set; }
    }
}

namespace PopNGo.ExtensionMethods
{
    public static class ItineraryEventExtensions
    {
        public static PopNGo.Models.DTO.ItineraryEventDTO ToDTO(this PopNGo.Models.ItineraryEvent dayEvent)
        {
            return new PopNGo.Models.DTO.ItineraryEventDTO
            {
                Id = dayEvent.Id,
                ItineraryId = dayEvent.ItineraryId,
                EventId = dayEvent.EventId,
                ReminderTime = dayEvent.ReminderTime,
                ReminderCustomTime = dayEvent.ReminderCustomTime ?? dayEvent.Event.EventDate.Value,
                EventDetails = dayEvent.Event.ToDTO()
            };
        } 
    }
}