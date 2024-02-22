using PopNGo.DAL.Abstract;
using PopNGo.Models;
using Microsoft.EntityFrameworkCore;

namespace PopNGo.DAL.Concrete
{
    public class FavoritesRepository : Repository<FavoriteEvent>, IFavoritesRepository
    {
        private readonly DbSet<FavoriteEvent> _favoriteEvents;
        private readonly DbSet<Event> _events;

        public FavoritesRepository(PopNGoDB context) : base(context)
        {
            _favoriteEvents = context.FavoriteEvents;
            _events = context.Events;
        }

        public void AddFavorite(int userId, string eventId)
        {
            var eventPrimaryId = _events.FirstOrDefault(e => e.ApiEventId == eventId).Id;
            var favoriteEvent = new FavoriteEvent { UserId = userId, EventId = eventPrimaryId };
            AddOrUpdate(favoriteEvent);
        }

        public void RemoveFavorite(int userId, string apiEventId)
        {
            var favoriteEvent = _favoriteEvents.FirstOrDefault(fe => fe.UserId == userId && fe.Event.ApiEventId == apiEventId);
            if (favoriteEvent != null)
            {
                Delete(favoriteEvent);
            }
        }

        public List<PopNGo.Models.DTO.Event> GetUserFavorites(int userId)
        {
            var userFavorites = _favoriteEvents
                .Where(fe => fe.UserId == userId)
                .Select(fe => fe.Event)
                .Select(e => new PopNGo.Models.DTO.Event
                {
                    // Assuming PopNGo.Models.DTO.Event and PopNGo.Models.Event have similar properties
                    ApiEventID = e.ApiEventId,
                    EventDate = e.EventDate,
                    EventName = e.EventName,
                    EventDescription = e.EventDescription,
                    EventLocation = e.EventLocation
                })
                .ToList();

            return userFavorites;
        }

        public bool IsFavorite(int userId, string apiEventId)
        {
            return _favoriteEvents.Any(fe => fe.UserId == userId && fe.Event.ApiEventId == apiEventId);
        }
    }
}
