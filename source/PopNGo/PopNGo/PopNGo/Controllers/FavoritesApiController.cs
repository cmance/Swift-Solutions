using Microsoft.AspNetCore.Mvc;
using PopNGo.DAL.Abstract;
using PopNGo.Models;
using PopNGo.Models.DTO;
// using PopNGo.DAL.Abstract;
// using PopNGo.DAL.Concrete;
// using PopNGo.Models.DTO;
// using PopNGo.ExtensionMethods;

namespace PopNGo.Controllers;
[ApiController]
[Route("api/[controller]")]
public class FavoritesApiController : Controller
{
    private readonly ILogger<FavoritesApiController> _logger;
    private readonly IConfiguration _configuration;
    private readonly IFavoritesRepository _favoritesRepo;
    private readonly IEventRepository _eventRepo;

    public FavoritesApiController(ILogger<FavoritesApiController> logger, IConfiguration configuration, IFavoritesRepository favoritesRepo, IEventRepository eventRepo)
    {
        _logger = logger;
        _configuration = configuration;
        _favoritesRepo = favoritesRepo;
        _eventRepo = eventRepo;
    }

    [HttpPost("AddFavorite")]
    public IActionResult AddFavorite([FromBody] AddFavoriteRequest request) //AddFavoriteRequest is a class that contains a Favorite and an Event
    {
        var favorite = request.Favorite;
        var eventDto = request.Event;

        if (!_eventRepo.IsEvent(favorite.EventID)) //If the event does not exist, add it to the events
        {
            _eventRepo.AddEvent(eventDto.EventID, eventDto.EventDate, eventDto.EventName, eventDto.EventDescription, eventDto.EventLocation);
        }

        // Whether the event existed or not, add it to the favorites
        _favoritesRepo.AddFavorite(favorite.UserId, favorite.EventID);
        return Ok();
    }
}
