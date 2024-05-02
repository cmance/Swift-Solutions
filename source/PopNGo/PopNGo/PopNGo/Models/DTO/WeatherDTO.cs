using Microsoft.Identity.Client;

namespace PopNGo.Models.DTO
{
    public class WeatherDTO
    {
        public List<WeatherForecastDTO> WeatherForecasts { get; set; }
        public string TemperatureUnit { get; set; }
        public string MeasurementUnit { get; set; }
    }
}

namespace PopNGo.ExtensionMethods
{
    public static class WeatherExtensions
    {
        public static Models.DTO.WeatherDTO ToDTO(this Models.Weather Weather, string temperatureUnit, string measurementUnit)
        {
            return new Models.DTO.WeatherDTO
            {
                WeatherForecasts = Weather.WeatherForecasts.Select(wf => wf.ToDTO(temperatureUnit, measurementUnit)).ToList(),
                TemperatureUnit = temperatureUnit,
                MeasurementUnit = measurementUnit
            };
        }
    }
}