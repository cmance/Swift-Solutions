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
public class BookmarkListApiController : Controller
{
    private readonly ILogger<BookmarkListApiController> _logger;
    private readonly IConfiguration _configuration;
    private readonly IFavoritesRepository _favoritesRepo;
    private readonly IEventRepository _eventRepo;
    private readonly UserManager<PopNGoUser> _userManager;
    private readonly IPgUserRepository _pgUserRepository;
    private readonly IBookmarkListRepository _bookmarkListRepository;

    public BookmarkListApiController(ILogger<BookmarkListApiController> logger, IConfiguration configuration, IFavoritesRepository favoritesRepo, IEventRepository eventRepo, UserManager<PopNGoUser> userManager, IPgUserRepository pgUserRepository, IBookmarkListRepository bookmarkListRepository)
    {
        _logger = logger;
        _configuration = configuration;
        _favoritesRepo = favoritesRepo;
        _eventRepo = eventRepo;
        _userManager = userManager;
        _pgUserRepository = pgUserRepository;
        _bookmarkListRepository = bookmarkListRepository;
    }

    [HttpGet("BookmarkLists")]
    public async Task<IActionResult> GetBookmarkLists()
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

            List<PopNGo.Models.DTO.BookmarkList> bookmarkLists = _bookmarkListRepository.GetBookmarkLists(pgUser.Id);
            return Ok(bookmarkLists);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting bookmark lists");
            return StatusCode(500);
        }
    }

    [HttpPost("BookmarkList")]
    public async Task<IActionResult> AddBookmarkList([FromBody] PopNGo.Models.DTO.BookmarkList bookmarkList)
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

            if (string.IsNullOrEmpty(bookmarkList.Title))
            {
                return BadRequest("Bookmark list title cannot be null or empty.");
            }

            _bookmarkListRepository.AddBookmarkList(pgUser.Id, bookmarkList.Title);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding bookmark list");
            return StatusCode(500);
        }
    }
}
