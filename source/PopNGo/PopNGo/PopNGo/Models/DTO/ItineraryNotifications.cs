namespace PopNGo.Models.DTO
{
    public class ItineraryNotificationDTO
    {
        public int Id { get; set; }
        public int ItineraryId { get; set; }
        public string Address { get; set; }
    }
}

namespace PopNGo.ExtensionMethods
{
    public static class ItineraryNotificationExtensions
    {
        public static PopNGo.Models.DTO.ItineraryNotificationDTO ToDTO(this PopNGo.Models.ItineraryNotification notification)
        {
            return new PopNGo.Models.DTO.ItineraryNotificationDTO
            {
                Id = notification.Id,
                ItineraryId = notification.ItineraryId,
                Address = notification.NotificationAddress
            };
        } 
    }
}