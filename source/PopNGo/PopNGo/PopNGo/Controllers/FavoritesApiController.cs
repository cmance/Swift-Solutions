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
public class FavoritesApiController : Controller
{
    private readonly ILogger<FavoritesApiController> _logger;
    private readonly IConfiguration _configuration;
    private readonly IFavoritesRepository _favoritesRepo;
    private readonly IEventRepository _eventRepo;
    private readonly UserManager<PopNGoUser> _userManager;
    private readonly IPgUserRepository _pgUserRepository;

    public FavoritesApiController(ILogger<FavoritesApiController> logger, IConfiguration configuration, IFavoritesRepository favoritesRepo, IEventRepository eventRepo, UserManager<PopNGoUser> userManager, IPgUserRepository pgUserRepository)
    {
        _logger = logger;
        _configuration = configuration;
        _favoritesRepo = favoritesRepo;
        _eventRepo = eventRepo;
        _userManager = userManager;
        _pgUserRepository = pgUserRepository;
    }

    [HttpPost("AddFavorite")]
    public IActionResult AddFavorite([FromBody] PopNGo.Models.DTO.Event eventInfo) //AddFavoriteRequest is a class that contains a Favorite and an Event
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

        if (!_eventRepo.IsEvent(eventInfo.ApiEventID)) //If the event does not exist, add it to the events
        {
            _eventRepo.AddEvent(eventInfo.ApiEventID, eventInfo.EventDate, eventInfo.EventName, eventInfo.EventDescription, eventInfo.EventLocation);
        }

        // Whether the event existed or not, add it to the favorites
        _favoritesRepo.AddFavorite(pgUser.Id, eventInfo.ApiEventID);
        return Ok();
    }
}
