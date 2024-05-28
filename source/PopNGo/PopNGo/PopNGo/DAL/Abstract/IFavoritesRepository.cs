using PopNGo.Models;

namespace PopNGo.DAL.Abstract
{
    public interface IFavoritesRepository : IRepository<FavoriteEvent>
    {
        public void AddFavorite(int bookmarkListId, string eventId);
        public void RemoveFavorite(int bookmarkListId, string eventId);
        public List<PopNGo.Models.DTO.Event> GetUserFavorites(int bookmarkListId);
        public List<PopNGo.Models.DTO.Event> GetAllFavorites(int userId);
        public bool IsFavorite(int userId, string eventId);
        public bool IsInBookmarkList(string bookmarkListName, string eventId);
    }
}
