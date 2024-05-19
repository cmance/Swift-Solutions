using PopNGo.DAL.Abstract;
using PopNGo.Models;
using Microsoft.EntityFrameworkCore;
using PopNGo.Models.DTO;
using SendGrid.Helpers.Mail;
using PopNGo.ExtensionMethods;
using System.Security.Claims;
using Microsoft.Identity.Client;

namespace PopNGo.DAL.Concrete
{
    public class ItineraryEventRepository : Repository<Models.ItineraryEvent>, IItineraryEventRepository
    {
        private readonly DbSet<Models.Itinerary> _itinerary;
        private readonly DbSet<Models.Event> _events;
        private readonly DbSet<Models.ItineraryEvent> _itineraryEvents;

        public ItineraryEventRepository(PopNGoDB context) : base(context)
        {
            _itinerary = context.Itineraries;
            //_dayEvent = context.ItineraryDayEvents;
            _events = context.Events;
            _itineraryEvents = context.ItineraryEvents;
        }


        public List<PopNGo.Models.DTO.ItineraryEventDTO> GetEventsFromItinerary(int userId, int itineraryId)
        {

            return _itineraryEvents
                   .Where(eh => eh.Itinerary.UserId == userId && eh.ItineraryId == itineraryId)
                   .OrderBy(e => e.Event.EventDate)
                   .Select(eh => eh.ToDTO()) // Move Select after OrderBy
                   .ToList();

        }
        public void AddOrUpdateItineraryDayEvent(int userId, string apiEventId, int itineraryId, string reminderTime)
        {
            if (string.IsNullOrEmpty(apiEventId))
            {
                throw new ArgumentException("EventId cannot be null or empty", nameof(apiEventId));
            }


            var eventEntity = _events.FirstOrDefault(e => e.ApiEventId == apiEventId);


            if (eventEntity == null)
            {
                throw new ArgumentException($"No event found with the id {apiEventId}", nameof(apiEventId));
            }

            var itinerary = _itinerary.FirstOrDefault(i => i.Id == itineraryId && i.UserId == userId);
            if (itinerary == null)
            {
                throw new InvalidOperationException("No valid itinerary found for the given user and itinerary ID.");
            }

            var itineraryEvent = _itineraryEvents
                .Include(ie => ie.Event)
                .Include(ie => ie.Itinerary)
                .FirstOrDefault(ie => ie.Event.ApiEventId == apiEventId && ie.ItineraryId == itineraryId);

            if (itineraryEvent != null)
            {
                itineraryEvent.ItineraryId = itinerary.Id; // Ensure it is linked to the right itinerary
                itineraryEvent.EventId = eventEntity.Id; // Update with the correct event ID if needed
                itineraryEvent.Event = eventEntity;
                itineraryEvent.ReminderTime = reminderTime;
                itineraryEvent.ReminderCustomTime = null;
                
                AddOrUpdate(itineraryEvent);
            }
            else
            {
                // Create a new ItineraryEvent if not found
                var newItineraryEvent = new Models.ItineraryEvent
                {
                    ItineraryId = itinerary.Id, // Directly use the provided itineraryId
                    EventId = eventEntity.Id,
                    ReminderTime = reminderTime,
                    ReminderCustomTime = null
                };
                AddOrUpdate(newItineraryEvent);
            }
        }


        public void DeleteEventFromItinerary(int userId, string apiEventId, int itineraryId)
        {
            if (string.IsNullOrEmpty(apiEventId))
            {
                throw new ArgumentException("EventId cannot be null or empty", nameof(apiEventId));
            }

            var itineraryEvent = _itineraryEvents
                .Include(ie => ie.Event)
                .Include(ie => ie.Itinerary)
                .FirstOrDefault(ie => ie.Event.ApiEventId == apiEventId && ie.Itinerary.UserId == userId && ie.ItineraryId == itineraryId);

            if (itineraryEvent == null)
            {
                throw new InvalidOperationException("Event not found or does not belong to the user's itinerary.");
            }

            try
            {
                Delete(itineraryEvent);
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting event from itinerary", ex);
            }
        }

        public void SaveReminderTime(int userId, string apiEventId, int itineraryId, string reminderTime, DateTime customTime)
        {
            if (string.IsNullOrEmpty(apiEventId))
            {
                throw new ArgumentException("EventId cannot be null or empty", nameof(apiEventId));
            }

            var itineraryEvent = _itineraryEvents
                .Include(ie => ie.Event)
                .Include(ie => ie.Itinerary)
                .FirstOrDefault(ie => ie.Event.ApiEventId == apiEventId && ie.Itinerary.UserId == userId && ie.ItineraryId == itineraryId);

            if (itineraryEvent == null)
            {
                throw new InvalidOperationException("Event not found or does not belong to the user's itinerary.");
            }

            itineraryEvent.ReminderTime = reminderTime;
            itineraryEvent.ReminderCustomTime = customTime;
            AddOrUpdate(itineraryEvent);
        }
    }
}