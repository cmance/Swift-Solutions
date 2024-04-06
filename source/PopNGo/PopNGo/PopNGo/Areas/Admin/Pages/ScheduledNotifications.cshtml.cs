// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using PopNGo.Areas.Identity.Data;
using PopNGo.DAL.Abstract;
using PopNGo.Models;

namespace PopNGo.Areas.Admin.Pages
{
    [Authorize(Roles = "Admin")]
    public class ScheduledNotificationsModel : PageModel
    {
        private readonly UserManager<PopNGoUser> _userManager;
        private readonly IPgUserRepository _pgUserRepo;
        private readonly IScheduledNotificationRepository _notificationRepo;

        public ScheduledNotificationsModel(
            UserManager<PopNGoUser> userManager,
            IPgUserRepository pgUserRepository,
            IScheduledNotificationRepository scheduledNotificationRepository)
        {
            _userManager = userManager;
            _pgUserRepo = pgUserRepository;
            _notificationRepo = scheduledNotificationRepository;
        }

        [BindProperty]
        public List<ScheduleModel> Schedules { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public class ScheduleModel
        {
            public string UserName { get; set; }
            public string NotificationEmail { get; set; }
            public string Type { get; set; }
            public string Time { get; set; }
            public string UserId { get; set; }
            public int Id { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            StatusMessage = "You are not authorized to view this page.";
            
            if(User.Identity.IsAuthenticated)
            {
                StatusMessage = "";

                Schedules = new List<ScheduleModel>();
                foreach (var user in _userManager.Users)
                {
                    PgUser pgUser = _pgUserRepo.GetPgUserFromIdentityId(user.Id);
                    if(pgUser == null)
                    {
                        Console.WriteLine("PgUser not found for user: " + user.UserName);
                        continue;
                    }
                    List<ScheduledNotification> notifications = await _notificationRepo.GetScheduledNotificationsByUserId(pgUser.Id);
                    if(notifications != null && notifications.Count > 0)
                    {
                        foreach (var notification in notifications)
                        {
                            Schedules.Add(new ScheduleModel
                            {
                                UserName = user.UserName,
                                NotificationEmail = user.NotificationEmail,
                                Type = notification.Type,
                                Time = notification.Time.ToString("M/d/yyyy, hh:mm tt"),
                                UserId = user.Id,
                                Id = notification.Id
                            });
                        }
                    }
                }            

            }

            return Page();
        }
    }
}