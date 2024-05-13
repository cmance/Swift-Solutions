using PopNGo.Models;

namespace PopNGo.Services
{
    public interface IWeatherForecastService
    {
        Task<Weather> GetForecast(string location);
    }
}