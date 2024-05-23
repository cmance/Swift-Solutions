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
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using PopNGo.Areas.Identity.Data;
using PopNGo.DAL.Abstract;
using PopNGo.DAL.Concrete;
using PopNGo.Models;

namespace PopNGo.Areas.Notifications.Pages
{
    public class OptOutModel : PageModel
    {
        private readonly UserManager<PopNGoUser> _userManager;
        private readonly IItineraryRepository _itineraryRepository;
        private readonly IRepository<ItineraryNotification> _itineraryNotificationRepository;

        public OptOutModel(
            UserManager<PopNGoUser> userManager,
            IItineraryRepository itineraryRepository,
            IRepository<ItineraryNotification> itineraryNotificationRepository
        )
        {
            _userManager = userManager;
            _itineraryRepository = itineraryRepository;
            _itineraryNotificationRepository = itineraryNotificationRepository;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }
        public IActionResult OnGetAsync(int itineraryId, string address, string code)
        {
            if (itineraryId == 0 || address == null || code == null)
            {
                return RedirectToPage("/Index");
            }

            var itinerary = _itineraryRepository.FindById(itineraryId);
            if (itinerary == null)
            {
                return NotFound($"Unable to load itinerary with ID '{itineraryId}'.");
            }

            var notification = itinerary.ItineraryNotifications
                                .FirstOrDefault(
                                    n => n.NotificationAddress == address &&
                                    n.ItineraryId == itineraryId
                                );
            if (notification == null)
            {
                return NotFound($"Unable to load notification with address '{address}' and code '{code}'.");
            }

            bool result = code == notification.OptOutCode;
            if (result)
            {
                notification.OptOut = true;
                _itineraryNotificationRepository.AddOrUpdate(notification);
                StatusMessage = "You have successfully opted out of itinerary notifications.";
            }
            else
            {
                StatusMessage = "Error opting out of itinerary notifications.";
            }

            return Page();
        }
    }
}
