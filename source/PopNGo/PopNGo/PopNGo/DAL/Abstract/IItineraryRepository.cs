
using PopNGo.Models;

namespace PopNGo.DAL.Abstract
{
    public interface IItineraryRepository : IRepository<Itinerary>
    {
        public void CreateNewItinerary(int userId, string itineraryTitle);
        public List<PopNGo.Models.DTO.Itinerary> GetAllItinerary(int userId);
        public void DeleteItinerary(int itineraryId);

    }
}
