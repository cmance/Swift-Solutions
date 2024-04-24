using PopNGo.DAL.Abstract;
using PopNGo.Models;
using Microsoft.EntityFrameworkCore;
using Humanizer;

namespace PopNGo.DAL.Concrete
{
    public class WeatherRepository : Repository<Weather>, IWeatherRepository
    {
        private readonly DbSet<Weather> _weathers;
        private readonly DbSet<WeatherForecast> _weatherForecasts;
        private readonly PopNGoDB _context;
        public WeatherRepository(PopNGoDB context) : base(context)
        {
            _weathers = context.Weathers;
            _weatherForecasts = context.WeatherForecasts;
            _context = context;
        }

        public async Task<Weather> GetForecastForLocation(double latitude, double longitude)
        {
            decimal lat = (decimal)latitude;
            decimal lon = (decimal)longitude;

            return await _weathers.Where(u => u.Latitude == lat && u.Longitude == lon).FirstOrDefaultAsync();
        }

        public async Task<bool> IsCachedDataExpired(int id)
        {
            DateTime cached = await _weathers.Where(u => u.Id == id)
                            .Select(u => u.DateCached)
                            .FirstOrDefaultAsync();

            // If the cached data is older than seven days, it's considered expired
            bool expired = (DateTime.Now.AtMidnight() - cached).TotalDays > 7;

            // If the data is expired, remove it from the database
            if(expired)
            {
                Weather weather = await _weathers.Where(u => u.Id == id).FirstOrDefaultAsync();
                // Remove all weather forecasts associated with the weather
                foreach (var forecast in weather.WeatherForecasts)
                {
                    _weatherForecasts.Remove(forecast);
                }

                // Remove the weather
                _weathers.Remove(weather);
                await _context.SaveChangesAsync();
            }

            return expired;
        }
    }
}
