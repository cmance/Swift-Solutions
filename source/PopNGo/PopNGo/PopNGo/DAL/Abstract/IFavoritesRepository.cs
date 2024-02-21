using PopNGo.Models;

namespace PopNGo.DAL.Abstract
{
    public interface IFavoritesRepository : IRepository<FavoriteEvents>
    {
        public void AddFavorite(int userId, string eventId);
        public void RemoveFavorite(int userId, string eventId);
        public bool IsFavorite(int userId, string eventId);
    }
}
