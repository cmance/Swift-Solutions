using PopNGo.Models;

namespace PopNGo.DAL.Abstract
{
    public interface IItineraryEventRepository : IRepository<ItineraryEvent>
    {
        public List<PopNGo.Models.DTO.ItineraryEventDTO> GetEventsFromItinerary(int userId, int itineraryId);

        public void AddOrUpdateItineraryDayEvent(int userId, string apiEventId, int itineraryId, string reminderTime);

        public void DeleteEventFromItinerary(int userId, string apiEventId, int itineraryId);

        public void SaveReminderTime(int userId, string apiEventId, int itineraryId, string reminderTime, DateTime customTime);
    }
}
