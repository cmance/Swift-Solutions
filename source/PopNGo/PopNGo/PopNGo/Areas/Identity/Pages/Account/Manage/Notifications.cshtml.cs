// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PopNGo.Areas.Identity.Data;

namespace PopNGo.Areas.Identity.Pages.Account.Manage
{
    public class NotificationsModel : PageModel
    {
        private readonly UserManager<PopNGoUser> _userManager;
        private readonly SignInManager<PopNGoUser> _signInManager;

        public NotificationsModel(
            UserManager<PopNGoUser> userManager,
            SignInManager<PopNGoUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [Display(Name = "Current email")]
        public string NotificationEmail { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///    This property is used to specify a user's notification email display purposes.
            ///    This property is required.
            /// </summary>
            [Required]
            [Display(Name = "New email")]
            public string NotificationEmail { get; set; }

            /// <summary>
            ///   This property is used to specify how long before an event a user wants to be notified.
            ///   This property is required.
            ///   
            ///   Options are:
            ///   - 15 minutes
            ///   - 30 minutes
            ///   - 1 hour
            ///   - 2 hours
            ///   - 3 hours
            ///   - 6 hours
            ///   - 1 day
            ///   
            /// </summary>
            [Required]
            [Display(Name = "Default Itinerary time")]
            public string ItineraryReminderTime { get; set; }

            /// <summary>
            ///   This property is used to specify a user's preference for notifications week-before notifications
            /// </summary>
            [Display(Name = "A week before")]
            public bool WeekBefore { get; set; }

            /// <summary>
            ///  This property is used to specify a user's preference for notifications day-before notifications
            /// </summary>
            [Display(Name = "Day before")]
            public bool DayBefore { get; set; }

            /// <summary>
            /// This property is used to specify a user's preference for notifications day-of notifications
            /// </summary>
            [Display(Name = "Day of")]
            public bool DayOf { get; set; }
        }

        private void Load(PopNGoUser user)
        {
            var notificationEmail = user.NotificationEmail;
            NotificationEmail = notificationEmail;

            Input = new InputModel
            {
                NotificationEmail = notificationEmail,
                ItineraryReminderTime = user.ItineraryReminderTime,
                WeekBefore = user.NotifyWeekBefore,
                DayBefore = user.NotifyDayBefore,
                DayOf = user.NotifyDayOf
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            Load(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                Load(user);
                return Page();
            }

            var notificationEmail = user.NotificationEmail;
            var itineraryTime = user.ItineraryReminderTime;
            var weekBefore = user.NotifyWeekBefore;
            var dayBefore = user.NotifyDayBefore;
            var dayOf = user.NotifyDayOf;

            bool changes = false;
            if (Input.NotificationEmail != notificationEmail)
            {
                user.NotificationEmail = Input.NotificationEmail;
                changes = true;
            }
            if (Input.ItineraryReminderTime != itineraryTime)
            {
                user.ItineraryReminderTime = Input.ItineraryReminderTime;
                changes = true;
            }
            if (Input.WeekBefore != weekBefore)
            {
                user.NotifyWeekBefore = Input.WeekBefore;
                changes = true;
            }
            if (Input.DayBefore != dayBefore)
            {
                user.NotifyDayBefore = Input.DayBefore;
                changes = true;
            }
            if (Input.DayOf != dayOf)
            {
                user.NotifyDayOf = Input.DayOf;
                changes = true;
            }

            // Only update the user if there are changes
            if (changes)
            {
                await _userManager.UpdateAsync(user);
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
