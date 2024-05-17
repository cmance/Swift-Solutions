using PopNGo.DAL.Abstract;
using PopNGo.Models;
using Microsoft.EntityFrameworkCore;
using PopNGo.ExtensionMethods;

namespace PopNGo.DAL.Concrete
{
    public class EventTagRepository : Repository<EventTag>, IEventTagRepository
    {
        private readonly DbSet<EventTag> _eventTags;
        public EventTagRepository(PopNGoDB context) : base(context)
        {
            _eventTags = context.EventTags;
        }

        public void AddEventTag(int tagId, int eventId)
        {
            var newEventTag = new EventTag
            {
                EventId = eventId,
                TagId = tagId
            };
            AddOrUpdate(newEventTag);
            
        }

        public IEnumerable<EventTag> GetEventTags(int eventId)
        {
            return _eventTags.Where(et => et.EventId == eventId).ToList();
        }
    }
}
