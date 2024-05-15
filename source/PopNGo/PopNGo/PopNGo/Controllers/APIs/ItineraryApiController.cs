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
    public class ItineraryApiController : Controller
    {
        private readonly IItineraryRepository _itineraryRepository;
        private readonly UserManager<PopNGoUser> _userManager;
        private readonly IPgUserRepository _pgUserRepository;
        public ItineraryApiController(UserManager<PopNGoUser> userManager, IItineraryRepository itineraryRepository, IPgUserRepository pgUserRepository)
        {
            _itineraryRepository = itineraryRepository;
            _userManager = userManager;
            _pgUserRepository = pgUserRepository;
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

            if (events == null || !events.Any())
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

    }
}
