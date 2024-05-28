using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PopNGo.Models.DTO;
using PopNGo.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace PopNGo.Controllers.APIs
{
    [Route("api/")]
    [ApiController]
    public class MapDirectionApiController : ControllerBase
    {
        private readonly IMapDirectionsService _mapDirectionsService;
        private readonly ILogger<MapDirectionApiController> _logger;

        public MapDirectionApiController(IMapDirectionsService mapDirectionsService, ILogger<MapDirectionApiController> logger)
        {
            _mapDirectionsService = mapDirectionsService;
            _logger = logger;
        }

        // GET: api/directions?startAddress=start&endAddress=end
        [HttpGet("directions")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Models.DTO.DirectionDetail>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Models.DTO.DirectionDetail>>> GetDirections(string startAddress, string endAddress)
        {
            try
            {
                var directions = await _mapDirectionsService.GetDirections(startAddress, endAddress);

                if (directions == null)
                {
                    _logger.LogError("Failed to retrieve directions or none found.");
                    return NotFound("No directions found.");
                }

                return Ok(directions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving directions");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the service.");
            }
        }
    }
}
