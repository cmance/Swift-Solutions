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

namespace PopNGo.Controllers.APIs
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItineraryEventApiController : Controller
    {
        private readonly IEventRepository _eventRepository;
        private readonly IItineraryEventRepository _itineraryEventRepository;
        private readonly IScheduledNotificationRepository _scheduledNotificationRepository;
        private readonly UserManager<PopNGoUser> _userManager;
        private readonly IPgUserRepository _pgUserRepository;

        public ItineraryEventApiController(
            UserManager<PopNGoUser> userManager,
            IEventRepository eventRepository,
            IItineraryEventRepository itineraryEventRepository,
            IScheduledNotificationRepository scheduledNotificationRepository,
            IPgUserRepository pgUserRepository)
        {
            _eventRepository = eventRepository;
            _itineraryEventRepository = itineraryEventRepository;
            _scheduledNotificationRepository = scheduledNotificationRepository;
            _userManager = userManager;
            _pgUserRepository = pgUserRepository;
        }

        // GET: api/ItineraryApi
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Models.DTO.ItineraryEventDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<Models.DTO.ItineraryEventDTO>>> GetUserEventFromItinerary(int itineraryId)
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
            IEnumerable<Models.DTO.ItineraryEventDTO> events = _itineraryEventRepository.GetEventsFromItinerary(pgUser.Id, itineraryId);

            if (events == null || !events.Any())
            {
                return NotFound();
            }

            return Ok(events);
        }


        [HttpPut("SaveReminder/eventId={apiEventId}&itineraryId={itineraryId}&reminderTime={time}&customTime={customTime}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> SaveReminder(string apiEventId, int itineraryId, string time, DateTime customTime)
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

            try
            {
                ScheduledNotification scheduledNotification = await _scheduledNotificationRepository.GetScheduledNotificationForItinerary(pgUser.Id, $"ItineraryEvent-{apiEventId}-{itineraryId}");
                ScheduleTasking.RemoveTask(scheduledNotification);

                DateTime? sendTime = _eventRepository.GetEventFromApiId(apiEventId).EventDate;
                if (sendTime == null)
                {
                    return BadRequest("Event not found");
                }

                if(time == "quarter-hour")
                {
                    sendTime = sendTime.Value.AddMinutes(-15);
                }
                else if(time == "half-hour")
                {
                    sendTime = sendTime.Value.AddMinutes(-30);
                }
                else if(time == "hour")
                {
                    sendTime = sendTime.Value.AddHours(-1);
                }
                else if(time == "two-hours")
                {
                    sendTime = sendTime.Value.AddHours(-2);
                }
                else if(time == "three-hours")
                {
                    sendTime = sendTime.Value.AddHours(-3);
                }
                else if(time == "six-hours")
                {
                    sendTime = sendTime.Value.AddHours(-6);
                }
                else if(time == "one-day")
                {
                    sendTime = sendTime.Value.AddDays(-1);
                }
                else
                {
                    sendTime = customTime;
                }

                await _scheduledNotificationRepository.UpdateScheduledNotification(scheduledNotification.Id, sendTime.Value, scheduledNotification.Type);

                _itineraryEventRepository.SaveReminderTime(pgUser.Id, apiEventId, itineraryId, time, customTime);
                Timer timer = new Timer(TimedEmailService.DoWork, scheduledNotification, TimeSpan.FromSeconds((sendTime.Value - DateTime.Now).TotalSeconds), TimeSpan.FromDays(1));
                ScheduleTasking.AddTask(scheduledNotification, timer);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error saving reminder: {ex.Message}");
            }
        }

        [HttpPost("ItineraryEvent/{apiEventId}/{itineraryId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> AddingToEventFromItineraries(string apiEventId, int itineraryId)
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

            Models.Event eventFound = _eventRepository.GetEventFromApiId(apiEventId);
            if (eventFound == null)
            {
                return BadRequest("Event not found");
            }
            DateTime? sendTime = eventFound.EventDate;

            if(sendTime != null && sendTime.Value < DateTime.Now)
            {
                return BadRequest("Event has already passed");
            }

            if (sendTime == null)
            {
                return BadRequest("Event not found");
            }

            if(user.ItineraryReminderTime == "quarter-hour")
            {
                sendTime = sendTime.Value.AddMinutes(-15);
            }
            else if(user.ItineraryReminderTime == "half-hour")
            {
                sendTime = sendTime.Value.AddMinutes(-30);
            }
            else if(user.ItineraryReminderTime == "hour")
            {
                sendTime = sendTime.Value.AddHours(-1);
            }
            else if(user.ItineraryReminderTime == "two-hours")
            {
                sendTime = sendTime.Value.AddHours(-2);
            }
            else if(user.ItineraryReminderTime == "three-hours")
            {
                sendTime = sendTime.Value.AddHours(-3);
            }
            else if(user.ItineraryReminderTime == "six-hours")
            {
                sendTime = sendTime.Value.AddHours(-6);
            }
            else if(user.ItineraryReminderTime == "one-day")
            {
                sendTime = sendTime.Value.AddDays(-1);
            }
            else
            {
                sendTime = sendTime.Value.AddHours(-1);
            }

            _itineraryEventRepository.AddOrUpdateItineraryDayEvent(pgUser.Id, apiEventId, itineraryId, user.ItineraryReminderTime ?? "hour");
            int newId = await _scheduledNotificationRepository.AddScheduledNotification(pgUser.Id, sendTime.Value, $"ItineraryEvent-{apiEventId}-{itineraryId}");
            ScheduledNotification scheduledNotification = _scheduledNotificationRepository.FindById(newId);

            Timer timer = new Timer(TimedEmailService.DoWork, scheduledNotification, TimeSpan.FromSeconds((sendTime.Value - DateTime.Now).TotalSeconds), TimeSpan.FromDays(1));
            ScheduleTasking.AddTask(scheduledNotification, timer);

            return Ok();
        }

        [HttpDelete("DeleteEventFromItinerary/{apiEventId}/{itineraryId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteEventFromItinerary(string apiEventId, int itineraryId)
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

            try
            {
                ScheduledNotification scheduledNotification = await _scheduledNotificationRepository.GetScheduledNotificationForItinerary(pgUser.Id, $"ItineraryEvent-{apiEventId}-{itineraryId}");
                ScheduleTasking.RemoveTask(scheduledNotification);

                _itineraryEventRepository.DeleteEventFromItinerary(pgUser.Id, apiEventId, itineraryId);
                await _scheduledNotificationRepository.DeleteScheduledNotification(scheduledNotification.Id);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error deleting event: {ex.Message}");
            }
        }
    }
}