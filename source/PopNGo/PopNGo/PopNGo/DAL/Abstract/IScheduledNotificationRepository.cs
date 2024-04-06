using PopNGo.Models;

namespace PopNGo.DAL.Abstract
{
    public interface IScheduledNotificationRepository : IRepository<ScheduledNotification>
    {
        public Task<List<ScheduledNotification>> GetScheduledNotificationsByUserId(int userId);

        public Task<int> FindByUserIdAsync(int userId, string scheduledNotificationType);

        public Task<int> AddScheduledNotification(int userId, DateTime scheduledNotificationTime, string scheduledNotificationType);

        public Task<bool> UpdateScheduledNotification(int scheduledNotificationId, DateTime newTime, string scheduledNotificationType);

        public Task<bool> DeleteScheduledNotification(int scheduledNotificationId);

        public Task<bool> DeleteScheduledNotificationsByUserId(int userId);

        public Task CleanUpScheduledNotifications(DateTime? checkTime);
    }
}
