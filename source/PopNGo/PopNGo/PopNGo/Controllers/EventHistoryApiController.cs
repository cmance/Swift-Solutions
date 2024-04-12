using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PopNGo.Areas.Identity.Data;
using PopNGo.DAL.Abstract;
using PopNGo.Models;
using PopNGo.Models.DTO;


namespace PopNGo.Controllers;
[ApiController]
[Route("api/[controller]")]
public class EventHistoryApiController : Controller
{
    private readonly ILogger<EventHistoryApiController> _logger;
    private readonly IConfiguration _configuration;
    private readonly IEventHistoryRepository _eventHistoryRepository;
    private readonly UserManager<PopNGoUser> _userManager;
    private readonly IPgUserRepository _pgUserRepository;

    public EventHistoryApiController(ILogger<EventHistoryApiController> logger, IConfiguration configuration, IEventHistoryRepository eventHistoryRepository, UserManager<PopNGoUser> userManager, IPgUserRepository pgUserRepository)
    {
        _logger = logger;
        _configuration = configuration;
        _eventHistoryRepository = eventHistoryRepository;
        _userManager = userManager;
        _pgUserRepository = pgUserRepository;
    }

    // GET: /api/EventHistoryApi/EventHistory
    [HttpGet("EventHistory")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Models.DTO.Event>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<IEnumerable<Models.DTO.Event>> GetUserEventHistory()
    {
        PopNGoUser user = _userManager.GetUserAsync(User).Result;
        if (user == null)
        {
            return Unauthorized();
        }

        PgUser pgUser = _pgUserRepository.GetPgUserFromIdentityId(user.Id);
        if (pgUser == null)
        {
            return Unauthorized();
        }

        List<Models.DTO.Event> events = _eventHistoryRepository.GetEventHistory(pgUser.Id);
        if (events == null || events.Count == 0)
        {
            return NotFound();
        }

        return events;
    }

    // POST: /api/EventHistoryApi/EventHistory
    [HttpPost("EventHistory")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> AddEventToHistoryAsync(string apiEventId)
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

        _eventHistoryRepository.AddEventHistory(pgUser.Id, apiEventId);
        return Ok();
    }
}
