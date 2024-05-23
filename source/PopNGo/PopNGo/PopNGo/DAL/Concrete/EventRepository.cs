using PopNGo.DAL.Abstract;
using PopNGo.Models;
using Microsoft.EntityFrameworkCore;
using PopNGo.ExtensionMethods;

namespace PopNGo.DAL.Concrete
{
    public class EventRepository : Repository<Event>, IEventRepository
    {
        private readonly DbSet<Event> _event;
        private readonly DbSet<TicketLink> _ticketLink;
        public EventRepository(PopNGoDB context) : base(context)
        {
            _event = context.Events;
            _ticketLink = context.TicketLinks;
        }

        public Event GetEventFromApiId(string apiEventId)
        {
            if (string.IsNullOrEmpty(apiEventId))
            {
                throw new ArgumentException("ApiEventId cannot be null or empty", nameof(apiEventId));
            }
            return _event.FirstOrDefault(e => e.ApiEventId == apiEventId);
        }

        public Event AddEvent(EventDetail eventDetail)
        {
            ValidateEventParameters(eventDetail);
            var newEvent = new Event
            {
                ApiEventId = eventDetail.EventID,
                EventDate = eventDetail.EventStartTime,
                EventName = eventDetail.EventName,
                EventDescription = eventDetail.EventDescription,
                EventLocation = eventDetail.Full_Address,
                EventImage = eventDetail.EventThumbnail,
                EventOriginalLink = eventDetail.EventLink,
                Latitude = eventDetail.Latitude,
                Longitude = eventDetail.Longitude,
                VenuePhoneNumber = eventDetail.Phone_Number,
                VenueName = eventDetail.VenueName,
                VenueRating = eventDetail.VenueRating,
                VenueWebsite = eventDetail.VenueWebsite
            };
            var addedEvent = AddOrUpdate(newEvent);

            foreach (var ticketLink in eventDetail.TicketLinks)
            {
                var newTicketLink = new TicketLink
                {
                    EventId = addedEvent.Id,
                    Source = ticketLink.Source,
                    Link = ticketLink.Link,
                };
                _ticketLink.Add(newTicketLink);
            }

            return addedEvent;
        }

        public List<PopNGo.Models.DTO.Event> GetEventsFromEventApiIds(List<string> eventApiIds)
        {
            if (eventApiIds == null)
            {
                throw new ArgumentNullException(nameof(eventApiIds));
            }

            var events = _event.Where(e => eventApiIds.Contains(e.ApiEventId)).Select(e => e.ToDTO()).ToList();
            return events;
        }

        public bool IsEvent(string apiEventId)
        {
            if (string.IsNullOrEmpty(apiEventId))
            {
                throw new ArgumentException("ApiEventId cannot be null or empty", nameof(apiEventId));
            }
            return _event.Any(e => e.ApiEventId == apiEventId);
        }

        private void ValidateEventParameters(EventDetail eventDetail)
        {
            if (string.IsNullOrEmpty(eventDetail.EventID))
            {
                throw new ArgumentException("EventId cannot be null or empty", nameof(eventDetail.EventID));
            }

            if (eventDetail.EventStartTime == default(DateTime))
            {
                throw new ArgumentException("EventDate cannot be default", nameof(eventDetail.EventStartTime));
            }

            if (string.IsNullOrEmpty(eventDetail.EventName))
            {
                throw new ArgumentException("EventName cannot be null or empty", nameof(eventDetail.EventName));
            }
        }
    }
}
