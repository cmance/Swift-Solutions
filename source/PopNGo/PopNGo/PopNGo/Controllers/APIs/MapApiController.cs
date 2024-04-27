using Microsoft.AspNetCore.Mvc;
using PopNGo.Models;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace PopNGo.Controllers;
[ApiController]
[Route("api/[controller]")]
public class MapApiController : Controller
{
    private readonly ILogger<MapApiController> _logger;
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;
    private string _googleApiKey;
    private string _googleGeolocationApiKey;
    private string _googlePlacesApiKey;
    // private readonly "REPOSITORIES"

    public MapApiController(ILogger<MapApiController> logger, IConfiguration configuration, HttpClient httpClient)
    {
        _logger = logger;
        _configuration = configuration;
        _googleApiKey = _configuration["GoogleMapsApiKey"];
        _googleGeolocationApiKey = _configuration["GoogleGeolocationApiKey"];
        _googlePlacesApiKey = _configuration["GooglePlacesApiKey"];
        _httpClient = httpClient;
    }

    [HttpGet("GetApiKey")]
    public IActionResult GetApiKey()
    {
        return Ok(_googleApiKey);
    }

    [HttpGet("GetGeolocationApiKey")]
    public IActionResult GetGeolocationApiKey()
    {
        return Ok(_googleGeolocationApiKey);
    }

    [HttpGet("GooglePlacesApiKey")]
    public IActionResult GooglePlacesApiKey()
    {
        return Ok(_googlePlacesApiKey);
    }
}
