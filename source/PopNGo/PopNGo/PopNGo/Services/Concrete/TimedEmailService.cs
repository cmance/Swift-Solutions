using System.Data;
using System.Reflection.PortableExecutable;
using Humanizer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using PopNGo.Areas.Identity.Data;
using PopNGo.DAL.Abstract;
using PopNGo.Models;

namespace PopNGo.Services;

public  class ScheduledTask
{
    public ScheduledNotification Task { get; set; }
    public Timer Trigger { get; set; }
}

public static class ScheduleTasking
{
    private static bool _isCleaning = false;
    private static List<ScheduledTask> _tasks = [];
    private static IServiceScopeFactory _serviceScopeFactory;

    public static void SetServiceScopeFactory(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public static IServiceScopeFactory GetServiceScopeFactory()
    {
        return _serviceScopeFactory;
    }

    public static bool GetCleaningStatus()
    {
        return _isCleaning;
    }

    public static void AddTask(ScheduledNotification task, Timer trigger)
    {
        _tasks.Add(new ScheduledTask { Task = task, Trigger = trigger });
    }

    public static void RemoveTask(ScheduledNotification task)
    {
        foreach (ScheduledTask t in _tasks)
        {
            if (t.Task.Id == task.Id)
            {
                t.Trigger.Dispose();
                _tasks.Remove(t);
                break;
            }
        }
    }

    public static void ClearTasks()
    {
        foreach (ScheduledTask task in _tasks)
        {
            task.Trigger.Dispose();
        }
        _tasks.Clear();
    }

    public static async void CleanupTasks(object state)
    {
        _isCleaning = true;
        DateTime current = DateTime.Now;
        List<ScheduledTask> toRemove = new List<ScheduledTask>();

        // Prevent the list from being modified while we're iterating
        foreach (ScheduledTask task in _tasks)
        {
            if (task.Task.Time < current)
            {
                task.Trigger.Dispose();
                toRemove.Add(task);
            }
        }

        using ( IServiceScope scope = _serviceScopeFactory.CreateScope())
        {
            IScheduledNotificationRepository _scheduledNotificationRepo = scope.ServiceProvider.GetRequiredService<IScheduledNotificationRepository>();
            // Remove the tasks that have already run
            foreach (ScheduledTask task in toRemove)
            {
                await _scheduledNotificationRepo.DeleteScheduledNotification(task.Task.Id);
                _tasks.Remove(task);
            }
        }

        _isCleaning = false;
    }

}
public class TimedEmailService : IHostedService, IDisposable
{
    private readonly ILogger<TimedEmailService> _logger;
    // private readonly string _logoBase64 = null;

    public TimedEmailService(ILogger<TimedEmailService> logger)
    {
        _logger = logger;

        // Convert the logo to base64
        // string logoPath = "wwwroot/media/images/logo.svg";
        // byte[] imageArray = File.ReadAllBytes(logoPath);
        // _logoBase64 = Convert.ToBase64String(imageArray);

        _logger.LogInformation("Timed Hosted Service running.");
    }

    public async Task StartAsync(CancellationToken stoppingToken)
    {
        DateTime current = DateTime.Now;
        DateTime next = DateTime.Now;
        DateTime soonest = next;

        using (IServiceScope scope = ScheduleTasking.GetServiceScopeFactory().CreateScope())
        {
            IScheduledNotificationRepository _scheduledNotificationRepo = scope.ServiceProvider.GetRequiredService<IScheduledNotificationRepository>();
            List<ScheduledNotification> scheduledNotifications = _scheduledNotificationRepo.GetAll().ToList();
            // For each task, start a timer to run at the next time

            foreach (ScheduledNotification task in scheduledNotifications)
            {
                Console.WriteLine($"Task: {task.UserId}, {task.Time}. Current time {current}");
                next = task.Time;
                Console.WriteLine($"Next is: {next}");

                // If the time has already past, reset to the next midnight
                if (next < current)
                {
                    Console.WriteLine("Time has already past, resetting to next midnight");
                    next = current.AtMidnight().AddDays(1);
                    await _scheduledNotificationRepo.UpdateScheduledNotification(task.Id, next, task.Type);
                    Console.WriteLine($"Next adjusted to: {next}");
                }

                // If the time is sooner than the soonest time, set it as the soonest time
                if (next < soonest)
                {
                    soonest = next;
                }

                // Start a task to run at the next time
                Timer _job = new Timer(DoWork, task, TimeSpan.FromSeconds((next - current).TotalSeconds), TimeSpan.FromDays(1));

                ScheduleTasking.AddTask(task, _job);
            }
        }

        // Start a task to clean up old tasks
        Timer _cleanup = new Timer(ScheduleTasking.CleanupTasks, null, TimeSpan.FromSeconds((soonest - current).TotalSeconds), TimeSpan.FromMinutes(1));
        return;
    }

    public static async void DoWork(object state)
    {

        int userId = (state as ScheduledNotification).UserId;

        while (ScheduleTasking.GetCleaningStatus())
        {
            await Task.Delay(100);
        }

        using (IServiceScope scope = ScheduleTasking.GetServiceScopeFactory().CreateScope())
        {
            IPgUserRepository _userRepo = scope.ServiceProvider.GetRequiredService<IPgUserRepository>();
            IFavoritesRepository _favoritesRepo = scope.ServiceProvider.GetRequiredService<IFavoritesRepository>();
            IEmailHistoryRepository _emailHistoryRepo = scope.ServiceProvider.GetRequiredService<IEmailHistoryRepository>();
            UserManager<PopNGoUser> _userManager = scope.ServiceProvider.GetRequiredService<UserManager<PopNGoUser>>();
            EmailBuilder _emailBuilder = scope.ServiceProvider.GetRequiredService<EmailBuilder>();
            IEmailSender _emailSender = scope.ServiceProvider.GetRequiredService<IEmailSender>();

            PgUser user = _userRepo.FindById(userId);
            PopNGoUser popNGoUser = await _userManager.FindByIdAsync(user.AspnetuserId);

            // If the user has no linked account, don't send the email
            if (popNGoUser != null)
            {
                string emailBody = await _emailBuilder.BuildEmailAsync(userId);
                if(emailBody != "")
                {
                    await _emailSender.SendEmailAsync(popNGoUser.NotificationEmail, "Your Event Reminders", emailBody);
                    _emailHistoryRepo.AddOrUpdate(new EmailHistory { UserId = userId, TimeSent = DateTime.Now, Type = "Upcoming Events" });
                }
            }

            // Now that we're done, queue up the task again at the next frequency
            RescheduleTask(state as ScheduledNotification);
        }
    }

    public Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Timed Hosted Service is stopping.");

        ScheduleTasking.ClearTasks();

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        ScheduleTasking.ClearTasks();
    }

    private static async void RescheduleTask(ScheduledNotification task)
    {
        DateTime current = DateTime.Now;

        using (IServiceScope scope = ScheduleTasking.GetServiceScopeFactory().CreateScope())
        {
            IPgUserRepository _userRepo = scope.ServiceProvider.GetRequiredService<IPgUserRepository>();
            IScheduledNotificationRepository _scheduledNotificationRepo = scope.ServiceProvider.GetRequiredService<IScheduledNotificationRepository>();

            // Get the next time to run the task
            DateTime next = current.AtMidnight().AddDays(1);

            // Delete the old task and add the new one
            try{
                _scheduledNotificationRepo.Delete(task);
            } catch (DBConcurrencyException e) {
                Console.WriteLine($"Error deleting task: {e.Message}");
            } catch (Exception e) {
                Console.WriteLine($"Error deleting task: {e.Message}");
            }
            int newId = await _scheduledNotificationRepo.AddScheduledNotification(task.UserId, next, task.Type);
            ScheduledNotification newNotification = _scheduledNotificationRepo.FindById(newId);

            // Start a new task and add it to the list
            Timer _job = new Timer(DoWork, newNotification, TimeSpan.FromSeconds((next - current).TotalSeconds), TimeSpan.FromDays(1));

            ScheduleTasking.AddTask(newNotification, _job);
        }
    }
}