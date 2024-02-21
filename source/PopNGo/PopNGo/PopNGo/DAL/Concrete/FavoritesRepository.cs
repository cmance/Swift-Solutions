using PopNGo.DAL.Abstract;
using PopNGo.Models;
using Microsoft.EntityFrameworkCore;

namespace PopNGo.DAL.Concrete
{
    public class FavoritesRepository : Repository<FavoriteEvents>, IFavoritesRepository
    {
        private readonly DbSet<FavoriteEvents> _favoriteEvents;
        public FavoritesRepository(PopNGoDB context) : base(context)
        {
            _favoriteEvents = context.FavoriteEvents;
        }

        public void AddFavorite(int userId, string eventId)
        {
            var favoriteEvent = new FavoriteEvents { UserID = userId, EventID = eventId };
            AddOrUpdate(favoriteEvent);
        }

        public void RemoveFavorite(int userId, string eventId)
        {
            var favoriteEvent = _favoriteEvents.FirstOrDefault(fe => fe.UserID == userId && fe.EventID == eventId);
            if (favoriteEvent != null)
            {
                Delete(favoriteEvent);
            }
        }

        public bool IsFavorite(int userId, string eventId)
        {
            return _favoriteEvents.Any(fe => fe.UserID == userId && fe.EventID == eventId);
        }
    }
}
