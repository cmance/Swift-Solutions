using PopNGo.Models;

namespace PopNGo.DAL.Abstract
{
    public interface IBookmarkListRepository : IRepository<BookmarkList>
    {
        public List<PopNGo.Models.DTO.BookmarkList> GetBookmarkLists(int userId);
        public void AddBookmarkList(int userId, string listName);
        public void DeleteBookmarkList(int userId, int bookmarkListId);
        public void UpdateBookmarkListName(int userId, int bookmarkListId, string newBookmarkListName);
        public int GetBookmarkListIdFromName(int userId, string listName);
        public bool IsBookmarkListNameUnique(int userId, string listName);
    }
}
