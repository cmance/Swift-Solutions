using PopNGo.DAL.Abstract;
using PopNGo.Models;
using Microsoft.EntityFrameworkCore;
using PopNGo.ExtensionMethods;

namespace PopNGo.DAL.Concrete
{
    public class EventRepository : Repository<Event>, IEventRepository
    {
        private readonly DbSet<Event> _event;
        public EventRepository(PopNGoDB context) : base(context)
        {
            _event = context.Event;
        }

        public void AddEvent(string EventId, DateTime EventDate, string EventName, string EventDescription, string EventLocation)
        {
            var newEvent = new Event { EventID = EventId, EventDate = EventDate, EventName = EventName, EventDescription = EventDescription, EventLocation = EventLocation };
            AddOrUpdate(newEvent);
        }

        public bool IsEvent(string eventId)
        {
            return _event.Any(e => e.EventID == eventId);
        }
    }
}
