using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PopNGo.Models;
using PopNGo.DAL.Abstract;
using PopNGo.DAL.Concrete;
using Microsoft.AspNetCore.Identity;
using PopNGo.Areas.Identity.Data;
using PopNGo.Models.DTO;
using PopNGo.Services;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace PopNGo.Controllers.APIs
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItineraryApiController : Controller
    {
        private readonly IItineraryRepository _itineraryRepository;
        private readonly UserManager<PopNGoUser> _userManager;
        private readonly IPgUserRepository _pgUserRepository;
        private readonly EmailBuilder _emailBuilder;
        private readonly EmailSender _emailSender;
        public ItineraryApiController(
            UserManager<PopNGoUser> userManager,
            IItineraryRepository itineraryRepository,
            IPgUserRepository pgUserRepository,
            EmailBuilder emailBuilder,
            EmailSender emailSender)
        {
            _itineraryRepository = itineraryRepository;
            _userManager = userManager;
            _pgUserRepository = pgUserRepository;
            _emailBuilder = emailBuilder;
            _emailSender = emailSender;
        }

        [HttpPost("Itinerary")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateNewItinerary(string itineraryTitle)
        {
            PopNGoUser user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }


            PgUser pgUser = _pgUserRepository.GetPgUserFromIdentityId(user.Id);
            if (pgUser == null)
            {
                return Unauthorized();
            }

            _itineraryRepository.CreateNewItinerary(pgUser.Id, itineraryTitle);
            return Ok();
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Models.DTO.Itinerary>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<Models.DTO.Itinerary>>> GetUserEventFromItinerary()
        {
            PopNGoUser user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            PgUser pgUser = _pgUserRepository.GetPgUserFromIdentityId(user.Id);
            if (pgUser == null)
            {
                return Unauthorized();
            }

            // Replace this with your logic to get events from itinerary based on pgUser.Id
            IEnumerable<Models.DTO.Itinerary> events = _itineraryRepository.GetAllItinerary(pgUser.Id);

            // if (events == null || !events.Any())
            if (events == null)
            {
                return NotFound();
            }

            return Ok(events);
        }

        [HttpDelete("{itineraryId}")]  // Specify the route to include the itinerary ID
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteItinerary(int itineraryId)
        {
            try
            {
                PopNGoUser user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return Unauthorized();
                }

                PgUser pgUser = _pgUserRepository.GetPgUserFromIdentityId(user.Id);
                if (pgUser == null)
                {
                    return Unauthorized();
                }

                // Perform the deletion
                _itineraryRepository.DeleteItinerary(itineraryId);

                // If no exception was thrown, deletion was successful
                return Ok($"Itinerary with ID {itineraryId} deleted successfully.");
            }
            catch (ArgumentException ex)
            {
                // If the itinerary was not found, return a 404 error
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                // For any other exceptions, return a 500 internal server error
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("ConfirmNotifications/{itineraryId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> ConfirmNotifications(int itineraryId, [FromBody] List<string> notificationAddresses)
        {
            try
            {
                PopNGoUser user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return Unauthorized();
                }

                PgUser pgUser = _pgUserRepository.GetPgUserFromIdentityId(user.Id);
                if (pgUser == null)
                {
                    return Unauthorized();
                }

                Models.Itinerary itinerary = _itineraryRepository.FindById(itineraryId);

                // Perform the confirmation
                notificationAddresses = notificationAddresses.Except(new List<string> { "" }).ToList();
                List<string> formerAddresses = _itineraryRepository.GetNotificationAddresses(itineraryId);
                List<string> deletedAddresses = formerAddresses.Except(notificationAddresses).ToList();
                List<string> newAddresses = notificationAddresses.Except(formerAddresses).ToList();

                // Send Deletion Emails
                try {
                    foreach (string address in deletedAddresses)
                    {
                        _itineraryRepository.DeleteNotification(itineraryId, address);

                        string emailSubject = "Itinerary Update - Removal from Notification List";
                        string emailType = $"ItineraryNotificationRemoval-{address}-{itineraryId}";
                        Dictionary<string, string> emailData = new Dictionary<string, string>() {
                            { "template", "itineraryNotificationRemoval" },
                            { "messageContent", "" },
                            { "itineraryTitle", itinerary.ItineraryTitle }
                        };

                        await _emailSender.SendEmailAsync(
                            address,
                            emailSubject,
                            emailData
                        );
                    }
                } catch (ArgumentException ex) {
                    Console.WriteLine("Deleting Addresses: " + deletedAddresses.ToString());
                    Console.WriteLine($"Error deleting notification: {ex.Message}");
                    return NotFound(ex.Message);
                }

                // Send Addition Emails
                try {
                    foreach (string address in newAddresses)
                    {
                        var code = await _userManager.GenerateUserTokenAsync(user, "Default", "AddNotification");
                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                        var callbackUrl = Url.Page(
                            "/OptOut",
                            pageHandler: null,
                            values: new { area = "Notifications", itineraryId = itineraryId, address = address, code = code },
                            protocol: Request.Scheme.Replace("ItineraryApi/ConfirmNotifications", "")
                        );
                        _itineraryRepository.AddNotification(itineraryId, address, code);

                        string emailSubject = "Itinerary Update - Addition to Notification List";
                        string emailType = $"ItineraryNotificationAddition-{address}-{itineraryId}";
                        Dictionary<string, string> emailData = new Dictionary<string, string>() {
                            { "template", "itineraryNotificationAddition" },
                            { "messageContent", "" },
                            { "itineraryTitle", itinerary.ItineraryTitle },
                            { "optOutURL", callbackUrl }
                        };

                        await _emailSender.SendEmailAsync(
                            address,
                            emailSubject,
                            emailData
                        );
                    }
                } catch (ArgumentException ex) {
                    Console.WriteLine("Adding Addresses: " + newAddresses.ToString());
                    Console.WriteLine($"Error adding notification: {ex.Message}");
                    return NotFound(ex.Message);
                }

                // If no exception was thrown, confirmation was successful
                return Ok($"Notifications for itinerary with ID {itineraryId} confirmed successfully.");
            }
            catch (ArgumentException ex)
            {
                // If the itinerary was not found, return a 404 error
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                // For any other exceptions, return a 500 internal server error
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
