using PopNGo.Models;

namespace PopNGo.DAL.Abstract
{
    public interface IEventRepository : IRepository<Event>
    {
        // Add methods specific to Event here
        Event AddEvent(EventDetail eventDetail);

        List<PopNGo.Models.DTO.Event> GetEventsFromEventApiIds(List<string> eventApiIds);
        
        bool IsEvent(string apiEventId);
        
        Event GetEventFromApiId(string apiEventId);
    }
}
