using PopNGo.Models;

namespace PopNGo.DAL.Abstract
{
    public interface IWeatherRepository : IRepository<Weather>
    {
        public Task<Weather> GetForecastForLocation(double latitude, double longitude);

        public Task<bool> IsCachedDataExpired(int id);
    }
}
