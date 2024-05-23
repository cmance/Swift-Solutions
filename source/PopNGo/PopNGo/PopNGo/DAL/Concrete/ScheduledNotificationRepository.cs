using PopNGo.DAL.Abstract;
using PopNGo.Models;
using Microsoft.EntityFrameworkCore;

namespace PopNGo.DAL.Concrete
{
    public class ScheduledNotificationRepository : Repository<ScheduledNotification>, IScheduledNotificationRepository
    {
        private readonly DbSet<ScheduledNotification> _scheduledNotifications;
        private readonly PopNGoDB _context;
        public ScheduledNotificationRepository(PopNGoDB context) : base(context)
        {
            _scheduledNotifications = context.ScheduledNotifications;
            _context = context;
        }

        public async Task<int> AddScheduledNotification(int userId, DateTime scheduledNotificationTime, string scheduledNotificationType)
        {
            ScheduledNotification existingNotification = await _scheduledNotifications.FirstOrDefaultAsync(u => u.UserId == userId && u.Type == scheduledNotificationType);
            if (existingNotification != null)
            {
                return -1;
            }
            
            ScheduledNotification notification = new ScheduledNotification
            {
                UserId = userId,
                Time = scheduledNotificationTime,
                Type = scheduledNotificationType
            };

            _scheduledNotifications.Add(notification);
            await _context.SaveChangesAsync();
            return notification.Id;
        }

        public Task CleanUpScheduledNotifications(DateTime? checkTime)
        {
            if (checkTime == null)
            {
                checkTime = DateTime.Now;
            }
            
            var notifications = _scheduledNotifications.Where(u => u.Time < checkTime);
            foreach (var notification in notifications)
            {
                _scheduledNotifications.Remove(notification);
            }
            return _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteScheduledNotification(int scheduledNotificationId)
        {
            var notification = await _scheduledNotifications.FindAsync(scheduledNotificationId);
            if (notification == null)
            {
                return false;
            }

            _scheduledNotifications.Remove(notification);
            await _context.SaveChangesAsync();

            await AddScheduledNotification(notification.UserId, notification.Time.AddDays(1), notification.Type);
            return true;
        }

        public async Task<bool> DeleteScheduledNotificationsByUserId(int userId)
        {
            var notifications = await _scheduledNotifications.Where(u => u.UserId == userId).ToListAsync();
            if (notifications.Count == 0)
            {
                return false;
            }

            _scheduledNotifications.RemoveRange(notifications);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> FindByUserIdAsync(int userId, string scheduledNotificationType)
        {
            var notification = await _scheduledNotifications.FirstOrDefaultAsync(u => u.UserId == userId && u.Type == scheduledNotificationType);
            if (notification == null)
            {
                return -1;
            }

            return notification.Id;
        }

        public async Task<List<ScheduledNotification>> GetScheduledNotificationsByUserId(int userId)
        {
            return await _scheduledNotifications.Where(u => u.UserId == userId).ToListAsync();
        }

        public async Task<bool> UpdateScheduledNotification(int scheduledNotificationId, DateTime newTime, string scheduledNotificationType)
        {
            var notification = await _scheduledNotifications.FindAsync(scheduledNotificationId);
            if (notification == null)
            {
                Console.WriteLine("Notification not found");
                return false;
            }

            if (newTime < DateTime.Now.AddSeconds(-30))
            {
                Console.WriteLine("Time is in the past");
                return false;
            }

            notification.Time = newTime;
            notification.Type = scheduledNotificationType;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<ScheduledNotification> GetScheduledNotificationForItinerary(int userId, string type)
        {
            return await _scheduledNotifications.FirstOrDefaultAsync(u => u.UserId == userId && u.Type == type);
        }
    }
}
