
using PopNGo.Models;

namespace PopNGo.DAL.Abstract
{
    public interface IItineraryRepository : IRepository<Itinerary>
    {
        public void CreateNewItinerary(int userId, string itineraryTitle);
        public List<PopNGo.Models.DTO.Itinerary> GetAllItinerary(int userId);
        public void DeleteItinerary(int itineraryId);

        public List<string> GetNotificationAddresses(int itineraryId);
        public void AddNotification(int itineraryId, string notificationAddress, string optOutCode);
        public void DeleteNotification(int itineraryId, string notificationAddress);

    }
}
