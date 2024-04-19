using Microsoft.Identity.Client;

namespace PopNGo.Models.DTO
{
    public class ScheduledNotificationDTO
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public DateTime Time { get; set; }
        public string UserId { get; set; }

        public override string ToString()
        {
            return $"Id: {Id}, Type: {Type}, Time: {Time}, UserId: {UserId}";
        }
    }
}

namespace PopNGo.ExtensionMethods
{
    public static class ScheduledNotificationExtensions
    {
        public static Models.DTO.ScheduledNotificationDTO ToDTO(this Models.ScheduledNotification Notification)
        {
            return new Models.DTO.ScheduledNotificationDTO
            {
                Id = Notification.Id,
                Type = Notification.Type,
                Time = Notification.Time.AddSeconds(-1 * Math.Abs(Notification.Time.Second)), // Always round down to the nearest minute
                UserId = Notification.User.AspnetuserId
            };
        }
    }
}