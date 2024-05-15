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

namespace PopNGo.Controllers.APIs
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItineraryEventApiController : Controller
    {
        private readonly IItineraryEventRepository _itineraryEventRepository;
        private readonly UserManager<PopNGoUser> _userManager;
        private readonly IPgUserRepository _pgUserRepository;

        public ItineraryEventApiController(UserManager<PopNGoUser> userManager, IItineraryEventRepository itineraryEventRepository, IPgUserRepository pgUserRepository)
        {
            _itineraryEventRepository = itineraryEventRepository;
            _userManager = userManager;
            _pgUserRepository = pgUserRepository;
        }

        // GET: api/ItineraryApi
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Models.DTO.Event>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<Models.DTO.Event>>> GetUserEventFromItinerary(int itineraryId)
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
            IEnumerable<Models.DTO.Event> events = _itineraryEventRepository.GetEventsFromItinerary(pgUser.Id, itineraryId);

            if (events == null || !events.Any())
            {
                return NotFound();
            }

            return Ok(events);
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

            _itineraryEventRepository.AddOrUpdateItineraryDayEvent(pgUser.Id, apiEventId, itineraryId);
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
                _itineraryEventRepository.DeleteEventFromItinerary(pgUser.Id, apiEventId, itineraryId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error deleting event: {ex.Message}");
            }
        }
    }
}