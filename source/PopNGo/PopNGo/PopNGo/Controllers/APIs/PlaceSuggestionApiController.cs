using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PopNGo.Areas.Identity.Data;
using PopNGo.DAL.Abstract;
using PopNGo.Models;
using PopNGo.Models.DTO;  // Ensure the DTO namespace is correctly referenced
using PopNGo.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PopNGo.Controllers
{
    [Route("api/")]
    [ApiController]
    public class PlaceSuggestionApiController : ControllerBase
    {
        private readonly IPlaceSuggestionsService _placeSuggestionsService;
        private readonly ISearchRecordRepository _searchRecordRepository;
        private readonly IPgUserRepository _pgUserRepository;
        private readonly ILogger<PlaceSuggestionApiController> _logger;
        private readonly UserManager<PopNGoUser> _userManager;

        public PlaceSuggestionApiController(
            IPlaceSuggestionsService placeSuggestionsService,
            ISearchRecordRepository searchRecordRepository,
            IPgUserRepository pgUserRepository,
            ILogger<PlaceSuggestionApiController> logger,
            UserManager<PopNGoUser> userManager)
        {
            _placeSuggestionsService = placeSuggestionsService;
            _searchRecordRepository = searchRecordRepository;
            _pgUserRepository = pgUserRepository;
            _logger = logger;
            _userManager = userManager;
        }

        // GET: api/search/places/?q=Q&coordinates=lat,long
        [HttpGet("search/places")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Models.DTO.PlaceSuggestion>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Models.DTO.PlaceSuggestion>>> GetPlaceSuggestions(string q, string coordinates)
        {
            try
            {
                var placeSuggestions = await _placeSuggestionsService.SearchPlaceSuggestion(q, coordinates);

                if (placeSuggestions == null)
                {
                    _logger.LogError("Failed to retrieve place suggestions or none found.");
                    return NotFound("No place suggestions found.");
                }

                return Ok(placeSuggestions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving place suggestions");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the service.");
            }
        }
    }
}
