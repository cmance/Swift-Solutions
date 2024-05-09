using Microsoft.AspNetCore.Mvc;
using PopNGo.DAL.Abstract;

using Microsoft.AspNetCore.Identity;
using PopNGo.Areas.Identity.Data;
using PopNGo.ExtensionMethods;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Text.Encodings.Web;
using System.ComponentModel;

using PopNGo.Services;
using PopNGo.Models;
using Humanizer;

namespace PopNGo.Controllers;
[ApiController]
[Route("api/distances/")]
public class DistanceCalculatorApiController : Controller
{
    private readonly IDistanceCalculatorService _distanceCalculatorService;
    private readonly ILogger<EventApiController> _logger;
    private readonly UserManager<PopNGoUser> _userManager;

    public DistanceCalculatorApiController(
        IDistanceCalculatorService distanceCalculatorService,
        UserManager<PopNGoUser> userManager,
        ILogger<EventApiController> logger
    )
    {
        _logger = logger;
        _userManager = userManager;
        _distanceCalculatorService = distanceCalculatorService;
    }

    [HttpGet("calculate/startingLat={startingLat}&startingLong={startingLong}&events={events}&unit={unit}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DistanceReturn))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<DistanceReturn>> GetDistancesToEvents(double? startingLat, double? startingLong, [FromRoute]List<string> events, [FromRoute] string unit)
    {
        PopNGoUser user = await _userManager.GetUserAsync(User);
        if(startingLat == null || startingLong == null)
        {
            return StatusCode(StatusCodes.Status400BadRequest);
        }

        try
        {
            string location = $"({startingLat}%2C{startingLong})";
            Console.WriteLine($"Events:{events.Count}");

            List<string> eventsList = new List<string>();
            foreach (string e in events)
            {
                string[] temp = e.Split("),");
                foreach (string t in temp)
                {
                    string t2 = t;
                    if(!t2.EndsWith(')'))
                    {
                        t2 += ")";
                    }
                    eventsList.Add(t2);
                }
            }
            return await _distanceCalculatorService.CalculateDistance(location, eventsList, unit ?? user?.DistanceUnit ?? "miles");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to calculate distances for events");
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet("unit")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<string>> GetDistanceUnit()
    {
        PopNGoUser user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return StatusCode(StatusCodes.Status401Unauthorized);
        }

        return user.DistanceUnit?.ToLower() ?? "miles";
    }
}