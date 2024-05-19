using Microsoft.AspNetCore.Mvc;
using PopNGo.DAL.Abstract;

using Microsoft.AspNetCore.Identity;
using PopNGo.Areas.Identity.Data;
using PopNGo.ExtensionMethods;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Text.Encodings.Web;
using System.ComponentModel;

using PopNGo.Services;
using PopNGo.Models;
using Humanizer;

namespace PopNGo.Controllers;
[ApiController]
[Route("api/admin/scheduledNotifications/")]
public class AdminNotificationsApiController : Controller
{
    private readonly ILogger<EventApiController> _logger;
    private readonly IPgUserRepository _pgUserRepository;
    private readonly IScheduledNotificationRepository _scheduledNotificationRepository;
    private readonly IEmailHistoryRepository _emailHistoryRepo;
    private readonly UserManager<PopNGoUser> _userManager;
    private readonly EmailSender _emailSender;
    private readonly EmailBuilder _emailBuilder;

    public AdminNotificationsApiController(
        IPgUserRepository pgUserRepository,
        IScheduledNotificationRepository scheduledNotificationRepository,
        IEmailHistoryRepository emailHistoryRepo,
        UserManager<PopNGoUser> userManager,
        ILogger<EventApiController> logger,
        EmailSender emailSender,
        EmailBuilder emailBuilder
    )
    {
        _logger = logger;
        _pgUserRepository = pgUserRepository;
        _scheduledNotificationRepository = scheduledNotificationRepository;
        _emailHistoryRepo = emailHistoryRepo;
        _userManager = userManager;
        _emailSender = emailSender;
        _emailBuilder = emailBuilder;
    }

    [HttpGet("sendEmail/notificationId={notificationId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<bool>> SendScheduledNotificationEmail(int notificationId)
    {
        var notification = _scheduledNotificationRepository.FindById(notificationId);
        if (notification == null)
        {
            return NotFound();
        }
        try
        {
            var pgUser = _pgUserRepository.FindById(notification.UserId);
            if (pgUser == null)
            {
                return NotFound();
            }
            var user = await _userManager.FindByIdAsync(pgUser.AspnetuserId);
            if (user == null)
            {
                return NotFound();
            }

            string emailBody = await _emailBuilder.BuildUpcomingEventsEmailAsync(pgUser.Id);
            if (emailBody != "")
            {
                await _emailSender.SendEmailAsync(
                    user.NotificationEmail,
                    "Your Event Reminders",
                    new Dictionary<string, string>
                    {
                        { "template", "upcomingEvents" },
                        { "messageContent", emailBody },
                        { "name", user.FirstName }
                    });
                    // emailBody);
                _emailHistoryRepo.AddOrUpdate(new EmailHistory { UserId = notification.UserId, TimeSent = DateTime.Now, Type = "Upcoming Events" });
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to send scheduled notification email for notification {notificationId}", notificationId);
            return false;
        }
        return true;
    }

    [HttpDelete("deleteNotification/notificationId={notificationId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
    public async Task<ActionResult<int>> DeleteScheduledNotification(int notificationId)
    {
        var notification = _scheduledNotificationRepository.FindById(notificationId);
        if (notification == null)
        {
            return NotFound();
        }
        try
        {
            bool result = await _scheduledNotificationRepository.DeleteScheduledNotification(notificationId);
            if (result) {
                int userId = notification.UserId;
                string type = notification.Type;
                DateTime current = DateTime.Now;
                DateTime next = current.AtMidnight().AddDays(1);

                ScheduleTasking.RemoveTask(notification);

                int nextNotificationId = await _scheduledNotificationRepository.FindByUserIdAsync(userId, type);
                ScheduledNotification nextNotification = _scheduledNotificationRepository.FindById(nextNotificationId);
    
                Timer timer = new Timer(TimedEmailService.DoWork, nextNotification, TimeSpan.FromSeconds((next - current).TotalSeconds), TimeSpan.FromDays(1));
                ScheduleTasking.AddTask(nextNotification, timer);

                return nextNotificationId;
            }
            return 0;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to delete scheduled notification for notification {notificationId}", notificationId);
            return 0;
        }
    }

    [HttpGet("getNotificationInfo/notificationId={notificationId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Models.DTO.ScheduledNotificationDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Models.DTO.ScheduledNotificationDTO> GetScheduledNotification(int notificationId)
    {
        var notification = _scheduledNotificationRepository.FindById(notificationId);
        if (notification == null)
        {
            return NotFound();
        }
        return notification.ToDTO();
    }

    [HttpPost("updateNotification/notificationId={notificationId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<bool>> UpdateScheduledNotification(int notificationId, Models.DTO.ScheduledNotificationDTO notificationData)
    {
        Console.WriteLine($"Updating notification {notificationId} to {notificationData.Time} and {notificationData.Type}.");
        var notification = _scheduledNotificationRepository.FindById(notificationId);
        if (notification == null)
        {
            return NotFound();
        }
        try
        {
            bool result = await _scheduledNotificationRepository.UpdateScheduledNotification(notificationId, notificationData.Time, notificationData.Type);
            if (result) {
                DateTime current = DateTime.Now;
                DateTime next = notificationData.Time;
                
                ScheduleTasking.RemoveTask(notification);

                ScheduledNotification nextNotification = _scheduledNotificationRepository.FindById(notificationId);
    
                Timer timer = new Timer(TimedEmailService.DoWork, nextNotification, TimeSpan.FromSeconds((next - current).TotalSeconds), TimeSpan.FromDays(1));
                ScheduleTasking.AddTask(nextNotification, timer);
            }
            return result;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to update scheduled notification for notification {notificationId}", notificationId);
            return false;
        }
    }
}