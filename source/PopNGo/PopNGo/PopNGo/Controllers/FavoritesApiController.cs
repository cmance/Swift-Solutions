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
    private readonly IBookmarkListRepository _bookmarkListRepository;

    public FavoritesApiController(ILogger<FavoritesApiController> logger, IConfiguration configuration, IFavoritesRepository favoritesRepo, IEventRepository eventRepo, UserManager<PopNGoUser> userManager, IPgUserRepository pgUserRepository, IBookmarkListRepository bookmarkListRepository)
    {
        _logger = logger;
        _configuration = configuration;
        _favoritesRepo = favoritesRepo;
        _eventRepo = eventRepo;
        _userManager = userManager;
        _pgUserRepository = pgUserRepository;
        _bookmarkListRepository = bookmarkListRepository;
    }

    [HttpPost("AddFavorite")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddFavorite([FromBody] BookmarkFavorite bookmarkFavorite) //AddFavoriteRequest is a class that contains a Favorite and an Event
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

            var eventInfo = bookmarkFavorite.EventInfo;

            if (!_eventRepo.IsEvent(eventInfo.ApiEventID)) //If the event does not exist, add it to the events
            {
                _eventRepo.AddEvent(eventInfo.ApiEventID, eventInfo.EventDate, eventInfo.EventName, eventInfo.EventDescription, eventInfo.EventLocation, eventInfo.EventImage);
            }

            if (string.IsNullOrEmpty(bookmarkFavorite.BookmarkListTitle))
            {
                // If the bookmark list title is null or empty, fail
                return BadRequest("Bookmark list title cannot be null or empty.");
            }

            // If the favorite is already in the list, return 204 No Content
            if (_favoritesRepo.IsInBookmarkList(bookmarkFavorite.BookmarkListTitle, eventInfo.ApiEventID))
            {
                return NoContent();
            }

            // Get the bookmark list ID from the title
            int bookmarkListId = _bookmarkListRepository.GetBookmarkListIdFromName(pgUser.Id, bookmarkFavorite.BookmarkListTitle);

            // Whether the event existed or not, add it to the favorites
            _favoritesRepo.AddFavorite(bookmarkListId, eventInfo.ApiEventID);
            return Ok();
        }
        catch (Exception ex)
        {
            // Log the exception, for example:
            _logger.LogError(ex, "An error occurred while adding a favorite event.");

            // Return a 500 Internal Server Error status code and a message to the client
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    [HttpPost("RemoveFavorite")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RemoveFavorite([FromBody] PopNGo.Models.DTO.Event eventInfo, string bookmarkListTitle)
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
            if (!_eventRepo.IsEvent(eventInfo.ApiEventID)) //If the event does not exist, add it to the events
            {
                _eventRepo.AddEvent(eventInfo.ApiEventID, eventInfo.EventDate, eventInfo.EventName, eventInfo.EventDescription, eventInfo.EventLocation, eventInfo.EventImage);
            }

            if (string.IsNullOrEmpty(bookmarkListTitle))
            {
                // If the bookmark list title is null or empty, fail
                return BadRequest("Bookmark list title cannot be null or empty.");
            }

            // Get the bookmark list ID from the title
            int bookmarkListId = _bookmarkListRepository.GetBookmarkListIdFromName(pgUser.Id, bookmarkListTitle);

            _favoritesRepo.RemoveFavorite(bookmarkListId, eventInfo.ApiEventID);
            return Ok();
        }
        catch (Exception ex)
        {
            // Log the exception, for example:
            _logger.LogError(ex, "An error occurred while removing a favorite event.");

            // Return a 500 Internal Server Error status code and a message to the client
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    [HttpGet("Favorites")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IEnumerable<PopNGo.Models.DTO.Event>>> GetUserFavorites(string bookmarkListTitle)
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

            if (string.IsNullOrEmpty(bookmarkListTitle))
            {
                // If the bookmark list title is null or empty, fail
                return BadRequest("Bookmark list title cannot be null or empty.");
            }

            // Get the bookmark list ID from the title
            int bookmarkListId;
            try
            {
                bookmarkListId = _bookmarkListRepository.GetBookmarkListIdFromName(pgUser.Id, bookmarkListTitle);
            } catch (ArgumentException)
            {
                return NotFound($"No bookmark list with the name {bookmarkListTitle} found");
            }

            List<PopNGo.Models.DTO.Event> events = _favoritesRepo.GetUserFavorites(bookmarkListId);
            
            return Ok(events);
        }
        catch (Exception ex)
        {
            // Log the exception, for example:
            _logger.LogError(ex, "An error occurred while getting user favorites.");

            // Return a 500 Internal Server Error status code and a message to the client
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    [HttpGet("IsFavorite")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<bool>> IsFavorite(string eventId)
    {
        try
        {
            PopNGoUser user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return false; // Return false if the user is not authenticated, instead of returning Unauthorized which is misleading
            }

            PgUser pgUser = _pgUserRepository.GetPgUserFromIdentityId(user.Id);
            if (pgUser == null)
            {
                return false; // Return false if the user is not found, instead of returning Unauthorized which is misleading
            }

            return _favoritesRepo.IsFavorite(pgUser.Id, eventId);
        }
        catch (Exception ex)
        {
            // Log the exception, for example:
            _logger.LogError(ex, "An error occurred while checking if the event is a favorite.");

            // Return a 500 Internal Server Error status code and a message to the client
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }
}
