using PopNGo.Models;

namespace PopNGo.DAL.Abstract
{
    public interface IRecommendedEventRepository : IRepository<RecommendedEvent>
    {
        public List<PopNGo.Models.DTO.Event> GetRecommendedEvents(int userId);
        public void SetRecommendedEvents(int userId, List<string> apiEventIds);
    }
}
