using PopNGo.Models;

namespace PopNGo.DAL.Abstract
{
    public interface IItineraryEventRepository : IRepository<ItineraryEvent>
    {
        public List<PopNGo.Models.DTO.Event> GetEventsFromItinerary(int userId, int itineraryId);

        public void AddOrUpdateItineraryDayEvent(int userId, string apiEventId, int itineraryId);

        public void DeleteEventFromItinerary(int userId, string apiEventId, int itineraryId);

    }
}
