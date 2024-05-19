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
    public class ItineraryRepository : Repository<Models.Itinerary>, IItineraryRepository
    {
        private readonly DbSet<Models.Itinerary> _itinerary;
        private readonly DbSet<Models.Event> _events;
        private readonly DbSet<Models.ItineraryEvent> _itineraryEvents;
        private readonly DbSet<Models.ItineraryNotification> _itineraryNotifications;
        private readonly PopNGoDB _context;

        public ItineraryRepository(PopNGoDB context) : base(context)
        {
            _itinerary = context.Itineraries;
            //_dayEvent = context.ItineraryDayEvents;
            _events = context.Events;
            _itineraryEvents = context.ItineraryEvents;
            _itineraryNotifications = context.ItineraryNotifications;
            _context = context;
        }

        public void CreateNewItinerary(int userId, string itineraryTitle)
        {
            // Check if the itinerary title is null or empty
            if (string.IsNullOrEmpty(itineraryTitle))
            {
                throw new ArgumentException("Itinerary title cannot be null or empty", nameof(itineraryTitle));
            }

            // Attempt to find an existing itinerary that matches the provided userId and title
            var existingItinerary = _itinerary.FirstOrDefault(it => it.UserId == userId && it.ItineraryTitle == itineraryTitle);

            if (existingItinerary != null)
            {
                // Update existing itinerary
                existingItinerary.ItineraryTitle = itineraryTitle;
                try
                {
                    AddOrUpdate(existingItinerary);
                }
                catch (Exception ex)
                {
                    // Properly handle the exception
                    throw new Exception("Error updating existing itinerary", ex);
                }
            }
            else
            {
                // Create a new Itinerary since one does not exist
                var newItinerary = new Models.Itinerary
                {
                    UserId = userId,
                    ItineraryTitle = itineraryTitle
                };

                // Add the new Itinerary
                AddOrUpdate(newItinerary);
            }
        }
        public List<PopNGo.Models.DTO.Itinerary> GetAllItinerary(int userId)
        {
            return _itinerary
                .Where(i => i.UserId == userId)
                .Select(i => i.ToDTO())
                .ToList();
                // .Select(i => new PopNGo.Models.DTO.Itinerary
                // {
                //     Id = i.Id,
                //     UserId = i.UserId,
                //     ItineraryTitle = i.ItineraryTitle,
                //     Events = i.ItineraryEvents.Select(ie => ie.Event.ToDTO()).ToList()
                //     // Events = i.ItineraryEvents.Select(ie => new PopNGo.Models.DTO.Event
                //     // {
                //     //     // Directly access ItineraryId from the ItineraryEvent join entity
                //     //     ItineraryId = ie.ItineraryId,
                //     //     EventName = ie.Event.EventName, // Map the 'EventName' from the Event entity
                //     //     EventDate = ie.Event.EventDate ?? DateTime.MinValue, // Handle nullable DateTime if necessary
                //     //     EventDescription = ie.Event.EventDescription,
                //     //     EventLocation = ie.Event.EventLocation,
                //     //     EventImage = ie.Event.EventImage,
                //     //     ApiEventID = ie.Event.ApiEventId,
                //     //     Longitude = ie.Event.Longitude,
                //     //     Latitude = ie.Event.Latitude
                //     //     // Additional fields that you require in your DTO
                //     // }).ToList()
                // }).ToList();
        }

        public void DeleteItinerary(int itineraryId)
        {
            var itinerary = _itinerary.Include(i => i.ItineraryEvents).FirstOrDefault(i => i.Id == itineraryId);

            if (itinerary == null)
            {
                throw new ArgumentException("Itinerary not found", nameof(itineraryId));
            }

            // Delete all related ItineraryEvents
            var itineraryEvents = itinerary.ItineraryEvents.ToList();
            foreach (var itineraryEvent in itineraryEvents)
            {
                _itineraryEvents.Remove(itineraryEvent);  // Using context directly for clarity
            }
            // Delete all related ItineraryEvents
            var itineraryNotifications = itinerary.ItineraryNotifications.ToList();
            foreach (var itineraryNotification in itineraryNotifications)
            {
                _itineraryNotifications.Remove(itineraryNotification);  // Using context directly for clarity
            }
            // Now, remove the itinerary itself using the Delete method of the Repository
            Delete(itinerary);
        }

        public List<string> GetNotificationAddresses(int itineraryId)
        {
            return _itinerary
                .Include(i => i.ItineraryNotifications)
                .FirstOrDefault(i => i.Id == itineraryId)
                ?.ItineraryNotifications
                .Select(n => n.NotificationAddress)
                .ToList();
        }

        public void AddNotification(int itineraryId, string notificationAddress, string optOutCode)
        {
            var itinerary = _itinerary.Include(i => i.ItineraryNotifications).FirstOrDefault(i => i.Id == itineraryId);

            if (itinerary == null)
            {
                throw new ArgumentException("Itinerary not found", nameof(itineraryId));
            }

            // Check if the notification address is null or empty
            if (string.IsNullOrEmpty(notificationAddress))
            {
                throw new ArgumentException("Notification address cannot be null or empty", nameof(notificationAddress));
            }

            // Check if the notification address already exists
            if (itinerary.ItineraryNotifications.Any(n => n.NotificationAddress == notificationAddress))
            {
                throw new ArgumentException("Notification address already exists", nameof(notificationAddress));
            }

            // Add the notification address
            itinerary.ItineraryNotifications.Add(new Models.ItineraryNotification
            {
                NotificationAddress = notificationAddress,
                OptOut = false,
                OptOutCode = optOutCode
            });

            // Save changes to the database
            AddOrUpdate(itinerary);
        }

        public void DeleteNotification(int itineraryId, string notificationAddress)
        {
            var itinerary = _itinerary.Include(i => i.ItineraryNotifications).FirstOrDefault(i => i.Id == itineraryId);

            if (itinerary == null)
            {
                throw new ArgumentException("Itinerary not found", nameof(itineraryId));
            }

            // Check if the notification address is null or empty
            if (string.IsNullOrEmpty(notificationAddress))
            {
                throw new ArgumentException("Notification address cannot be null or empty", nameof(notificationAddress));
            }

            // Find the notification to delete
            var notification = itinerary.ItineraryNotifications.FirstOrDefault(n => n.NotificationAddress == notificationAddress);

            if (notification == null)
            {
                throw new ArgumentException("Notification not found", nameof(notificationAddress));
            }

            // Remove the notification
            _itineraryNotifications.Remove(notification);
            _context.SaveChanges();
            // _itineraryNotifications.Remove(notification);
            // itinerary.ItineraryNotifications.Remove(notification);

            // Save changes to the database
            AddOrUpdate(itinerary);
        }
    }
}