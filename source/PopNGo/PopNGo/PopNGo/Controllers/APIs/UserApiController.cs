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
public class UserApiController : Controller
{
    private readonly ILogger<UserApiController> _logger;
    private readonly UserManager<PopNGoUser> _userManager;

    public UserApiController(ILogger<UserApiController> logger, UserManager<PopNGoUser> userManager)
    {
        _logger = logger;
        _userManager = userManager;
    }

    [HttpGet("LoggedIn")]
    public async Task<ActionResult<bool>> LoggedIn()
    {
      PopNGoUser user = await _userManager.GetUserAsync(User);
      if (user == null)
      {
        return Ok(false);
      }

      return Ok(true);
    }
}
