using PopNGo.DAL.Abstract;
using PopNGo.Models;
using Microsoft.EntityFrameworkCore;
using PopNGo.ExtensionMethods;

namespace PopNGo.DAL.Concrete
{
    public class BookmarkListRepository : Repository<BookmarkList>, IBookmarkListRepository
    {
        private readonly DbSet<BookmarkList> _bookmarkLists;
        private readonly DbSet<FavoriteEvent> _favoriteEvents;

        public BookmarkListRepository(PopNGoDB context) : base(context)
        {
            _bookmarkLists = context.BookmarkLists;
            _favoriteEvents = context.FavoriteEvents;
        }

        public List<PopNGo.Models.DTO.BookmarkList> GetBookmarkLists(int userId)
        {
            // Get all bookmark lists for the user and set the image 
            return _bookmarkLists
                .Where(bl => bl.UserId == userId)
                .Select(bl => new PopNGo.Models.DTO.BookmarkList
                {
                    Id = bl.Id,
                    Title = bl.Title,
                    UserId = bl.UserId,
                    // Query the favorite events to get the event image
                    // Prioritize the image from the event that is happening the soonest, but has not passed yet.
                    // If there are no events that have not passed, get the image from the event happened the latest.
                    // If there are no events at all, set the image to null.
                    Image = bl.FavoriteEvents
                        .Where(fe => fe.Event.EventDate > System.DateTime.Now)
                        .OrderBy(fe => fe.Event.EventDate)
                        .Select(fe => fe.Event.EventImage)
                        .FirstOrDefault() ?? bl.FavoriteEvents
                        .OrderByDescending(fe => fe.Event.EventDate)
                        .Select(fe => fe.Event.EventImage)
                        .FirstOrDefault(),
                    FavoriteEventQuantity = bl.FavoriteEvents.Count
                })
                .ToList();
            
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

        public void DeleteBookmarkList(int userId, int listId)
        {
            var bookmarkList = _bookmarkLists.FirstOrDefault(bl => bl.UserId == userId && bl.Id == listId);
            if (bookmarkList == null)
            {
                throw new ArgumentException($"No bookmark list found for user {userId} with the id {listId}", nameof(listId));
            }

            try
            {
                // Delete all favorite events associated with the bookmark list
                foreach(var favoriteEvent in bookmarkList.FavoriteEvents.ToList())
                {
                    // Couldn't get cascade delete to work, so have to delete each favorite event manually
                    _favoriteEvents.Remove(favoriteEvent);
                }
                Delete(bookmarkList);
            }
            catch (Exception ex)
            {
                // Log the exception, rethrow it, or handle it in some other way
                throw new Exception("Error deleting bookmark list", ex);
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
