using PopNGo.DAL.Abstract;
using PopNGo.Models;
using Microsoft.EntityFrameworkCore;
using PopNGo.ExtensionMethods;

namespace PopNGo.DAL.Concrete
{
    public class BookmarkListRepository : Repository<BookmarkList>, IBookmarkListRepository
    {
        private readonly DbSet<BookmarkList> _bookmarkLists;

        public BookmarkListRepository(PopNGoDB context) : base(context)
        {
            _bookmarkLists = context.BookmarkLists;
        }

        public List<PopNGo.Models.DTO.BookmarkList> GetBookmarkLists(int userId)
        {
            return _bookmarkLists.Where(bl => bl.UserId == userId).Select(bl => bl.ToDTO()).ToList();
        }

        public void AddBookmarkList(int userId, string listName)
        {
            if (string.IsNullOrEmpty(listName))
            {
                throw new ArgumentNullException("List name cannot be null or empty", nameof(listName));
            }

            if (!IsBookmarkListNameUnique(userId, listName))
            {
                throw new ArgumentException($"Bookmark list name {listName} is not unique for user {userId}", nameof(listName));
            }

            var bookmarkList = new BookmarkList { UserId = userId, Title = listName };

            try
            {
                AddOrUpdate(bookmarkList);
            }
            catch (Exception ex)
            {
                // Log the exception, rethrow it, or handle it in some other way
                throw new Exception("Error adding or updating bookmark list", ex);
            }
        }

        public int GetBookmarkListIdFromName(int userId, string listName)
        {
            if (string.IsNullOrEmpty(listName))
            {
                throw new ArgumentNullException("List name cannot be null or empty", nameof(listName));
            }

            var bookmarkList = _bookmarkLists.FirstOrDefault(bl => bl.UserId == userId && bl.Title == listName);
            return bookmarkList == null
                ? throw new ArgumentException($"No bookmark list found for user {userId} with the name {listName}", nameof(listName))
                : bookmarkList.Id;
        }

        public bool IsBookmarkListNameUnique(int userId, string listName)
        {
            return _bookmarkLists.All(bl => bl.UserId != userId || bl.Title != listName);
        }
    }
}
