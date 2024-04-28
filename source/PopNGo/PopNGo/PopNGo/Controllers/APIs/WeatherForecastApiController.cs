using Microsoft.AspNetCore.Mvc;
using PopNGo.DAL.Abstract;

using Microsoft.AspNetCore.Identity;
using PopNGo.Areas.Identity.Data;
using PopNGo.ExtensionMethods;
using PopNGo.Services;
using PopNGo.Models;
using PopNGo.Models.DTO;

namespace PopNGo.Controllers;
[ApiController]
[Route("api/weather/")]
public class WeatherForecastAPIController : Controller
{
    private readonly IWeatherForecastService _weatherForecastService;
    private readonly ILogger<EventApiController> _logger;
    private readonly UserManager<PopNGoUser> _userManager;
    private readonly IWeatherRepository _weatherRepository;

    public WeatherForecastAPIController(
        IWeatherForecastService weatherForecastService,
        UserManager<PopNGoUser> userManager,
        ILogger<EventApiController> logger,
        IWeatherRepository weatherRepository
    )
    {
        _logger = logger;
        _userManager = userManager;
        _weatherForecastService = weatherForecastService;
        _weatherRepository = weatherRepository;
    }

    [HttpGet("forecast/lat={latitude}&long={longitude}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<WeatherDTO>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<WeatherDTO>> GetForecastForLocation(double latitude, double longitude)
    {
        PopNGoUser user = await _userManager.GetUserAsync(User);
        string temperatureUnit = user?.TemperatureUnit ?? "f";
        string measurementUnit = user?.MeasurementUnit ?? "inches";

        Weather cachedForecast = await _weatherRepository.GetForecastForLocation(latitude, longitude);
        // If the forecast hasn't been cached before or it's older than seven days, fetch the forecast and store it.
        if(cachedForecast == null || await _weatherRepository.IsCachedDataExpired(cachedForecast.Id)) {
            // (DateTime.Now.AtMidnight() - cachedForecast.DateCached).TotalDays > 7
            try
            {
                string location = $"{latitude}%2C{longitude}";

                Weather weather = await _weatherForecastService.GetForecast(location);
                _weatherRepository.AddOrUpdate(weather);
                
                _logger.LogInformation("Retrieved weather forecast for location");
                return weather.ToDTO(temperatureUnit, measurementUnit);//WeatherForecasts.Select(wf => wf.ToDTO()).ToList();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to retrieve weather forecast for location");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        else {
            _logger.LogInformation("Returning cached weather forecast for location");
            return cachedForecast.ToDTO(temperatureUnit, measurementUnit);
        }
    }

    [HttpGet("unit={unit}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<string>> GetUnit(string unit)
    {
        PopNGoUser user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return StatusCode(StatusCodes.Status401Unauthorized);
        }

        if(unit == "temperature")
        {
            return user.TemperatureUnit.ToLower();
        }
        else if(unit == "measurement")
        {
            return user.MeasurementUnit.ToLower();
        }
        else
        {
            return StatusCode(StatusCodes.Status404NotFound);
        }
    }
}