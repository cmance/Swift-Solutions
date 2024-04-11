using PopNGo.Models;

namespace PopNGo.DAL.Abstract
{
    public interface IBookmarkListRepository : IRepository<BookmarkList>
    {
        public List<PopNGo.Models.DTO.BookmarkList> GetBookmarkLists(int userId);
        public void AddBookmarkList(int userId, string listName);
        public int GetBookmarkListIdFromName(int userId, string listName);
        public bool IsBookmarkListNameUnique(int userId, string listName);
    }
}
