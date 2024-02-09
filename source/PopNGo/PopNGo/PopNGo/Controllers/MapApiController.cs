using Microsoft.AspNetCore.Mvc;
using PopNGo.Models;
// using PopNGo.DAL.Abstract;
// using PopNGo.DAL.Concrete;
// using PopNGo.Models.DTO;
// using PopNGo.ExtensionMethods;

namespace PopNGo.Controllers;
[ApiController]
[Route("api/[controller]")]
public class MapApiController : Controller
{
    private readonly ILogger<MapApiController> _logger;
    // private readonly "REPOSITORIES"

    public MapApiController(ILogger<MapApiController> logger)
    {
        _logger = logger;
        // _repo = new "REPOSITORIES"
    }

    // Was going to use to hide the API key, but it's not necessary???
    // [HttpGet]
    // public IActionResult Get()
    // {
    //     return Ok();
    // }
}
