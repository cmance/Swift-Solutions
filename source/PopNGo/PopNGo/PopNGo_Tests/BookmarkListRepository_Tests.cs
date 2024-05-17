using Moq;
using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using PopNGo.DAL.Concrete;
using PopNGo.DAL.Abstract;
using PopNGo.Models;
using System.Collections.Generic;

namespace PopNGo_Tests;

public class BookMarkRepositoryTests
{
    private static readonly string _seedFile = @"../../../Sql/SEED.sql";  // relative path from where the executable is: bin/Debug/net7.0

    // Create this helper like this, for whatever context you desire
    private static readonly InMemoryDbHelper<PopNGoDB> _dbHelper = new(_seedFile, DbPersistence.OneDbPerTest);
    private static BookmarkListRepository _bookmarkListRepository = null!;
    private static FavoritesRepository _favoritesRepository = null!;
    private static PopNGoDB _context = null!;

    [SetUp]
    public void Setup()
    {
        _context = _dbHelper.GetContext();
        _bookmarkListRepository = new BookmarkListRepository(_context);
        _favoritesRepository = new FavoritesRepository(_context);
    }

    [Test]
    public void GetBookmarkLists_ShouldReturnBookmarkLists()
    {
        // Arrange
        var userId = 1;

        // Act
        var result = _bookmarkListRepository.GetBookmarkLists(userId);

        // Assert that the result is not null and is of type List<BookmarkList>
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.TypeOf<List<PopNGo.Models.DTO.BookmarkList>>());
    }

    [Test]
    public void GetBookmarkLists_WithNonExistentUserId_ShouldReturnEmptyList()
    {
        // Arrange
        var userId = 0;

        // Act
        var result = _bookmarkListRepository.GetBookmarkLists(userId);

        // Assert
        Assert.That(result, Is.Empty);
    }

    [Test]
    public void GetBookmarkLists_WithEventsThatHaveNoImage_ShouldReturnBookmarkListsWithNullImage()
    {
        // Arrange
        var userId = 1;

        // Act
        _bookmarkListRepository.AddBookmarkList(userId, "Bookmark list with no image");
        var bookmarkListId = _bookmarkListRepository.GetBookmarkListIdFromName(userId, "Bookmark list with no image");
        _favoritesRepository.AddFavorite(bookmarkListId, "event3");
        var result = _bookmarkListRepository.GetBookmarkLists(userId);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.TypeOf<List<PopNGo.Models.DTO.BookmarkList>>());
        Assert.That(result.Last().Image, Is.Null);
    }

    [Test]
    public void GetBookmarkLists_WithInternalEventWithImage_ShouldReturnBookmarkListsWithImage()
    {
        // Arrange
        var userId = 1;
        // Create a bookmark list with a saved event that has an image
        _bookmarkListRepository.AddBookmarkList(userId, "Test List");
        var bookmarkListId = _bookmarkListRepository.GetBookmarkListIdFromName(userId, "Test List");
        _favoritesRepository.AddFavorite(bookmarkListId, "event4");

        // Act
        var result = _bookmarkListRepository.GetBookmarkLists(userId);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.TypeOf<List<PopNGo.Models.DTO.BookmarkList>>());
        Assert.That(result.Last().Image, Is.Not.Null);
    }

    [Test]
    public void GetBookmarkLists_WithMultipleEventsWithImages_ShouldReturnBookmarkListsWithLatestEventImage()
    {
        // Arrange
        var userId = 1;
        // Create a bookmark list with multiple saved events that have images
        _bookmarkListRepository.AddBookmarkList(userId, "Test List");
        var bookmarkListId = _bookmarkListRepository.GetBookmarkListIdFromName(userId, "Test List");
        _favoritesRepository.AddFavorite(bookmarkListId, "event3"); // event3 has no image
        _favoritesRepository.AddFavorite(bookmarkListId, "event4"); // event4 has passed and has an image
        _favoritesRepository.AddFavorite(bookmarkListId, "event5"); // event5 has not passed and has an image and is the soonest event
        _favoritesRepository.AddFavorite(bookmarkListId, "event6"); // event6 has not passed and has an image and is the latest event

        // Act
        var result = _bookmarkListRepository.GetBookmarkLists(userId);

        // Assert
        // Check that the image is the latest event image from the events stored in the bookmark list
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.TypeOf<List<PopNGo.Models.DTO.BookmarkList>>());
        Assert.That(result.Last().Image, Is.EqualTo("event5.jpg"));
    }


    [Test]
    public void AddBookmarkList_ShouldAddNewBookmarkList()
    {
        // Arrange
        var userId = 1;
        var listName = "Test List";

        // Act
        _bookmarkListRepository.AddBookmarkList(userId, listName);

        // Assert
        // Assumes there is already a bookmark list in the database
        Assert.That(_bookmarkListRepository.GetBookmarkLists(userId).Count, Is.EqualTo(2));
        Assert.That(_bookmarkListRepository.GetBookmarkLists(userId).Last().Title, Is.EqualTo(listName));
    }

    [Test]
    public void AddBookmarkList_ShouldThrowExceptionWhenListNameIsNull()
    {
        // Arrange
        var userId = 1;
        var listName = "";

        // Assert
        Assert.Throws<ArgumentNullException>(() => _bookmarkListRepository.AddBookmarkList(userId, listName));
    }

    [Test]
    public void AddBookmarkList_ShouldThrowExceptionWhenListNameIsDuplicate()
    {
        // Arrange
        var userId = 1;
        var listName = "Test List";

        // Act
        _bookmarkListRepository.AddBookmarkList(userId, listName);

        // Assert
        Assert.Throws<ArgumentException>(() => _bookmarkListRepository.AddBookmarkList(userId, listName));
    }

    [Test]
    public void DeleteBookmarkList_ShouldDeleteBookmarkList()
    {
        // Arrange
        var userId = 3;
        var listName = "Test List";

        // Act
        _bookmarkListRepository.AddBookmarkList(userId, listName);
        var bookmarkListId = _bookmarkListRepository.GetBookmarkListIdFromName(userId, listName);
        _bookmarkListRepository.DeleteBookmarkList(userId, bookmarkListId);

        // Assert
        Assert.That(_bookmarkListRepository.GetBookmarkLists(userId).Count, Is.EqualTo(0));
    }

    [Test]
    public void DeleteBookmarkList_ShouldDeleteFavorites()
    {
        // Arrange
        var userId = 1;
        var listName = "Test List";

        // Act
        _bookmarkListRepository.AddBookmarkList(userId, listName);
        var bookmarkListId = _bookmarkListRepository.GetBookmarkListIdFromName(userId, listName);
        _favoritesRepository.AddFavorite(bookmarkListId, "event3");
        _favoritesRepository.AddFavorite(bookmarkListId, "event4");
        _favoritesRepository.AddFavorite(bookmarkListId, "event5");
        _favoritesRepository.AddFavorite(bookmarkListId, "event6");
        _bookmarkListRepository.DeleteBookmarkList(userId, bookmarkListId);

        // Assert
        Assert.That(_favoritesRepository.GetUserFavorites(bookmarkListId).Count, Is.EqualTo(0));
    }

    [Test]
    public void DeleteBookmarkList_ShouldThrowExceptionWhenListIdIsNotFound()
    {
        // Arrange
        var userId = 1;
        var listId = 2;

        // // Assert
        Assert.Throws<ArgumentException>(() => _bookmarkListRepository.DeleteBookmarkList(userId, listId));
    }

    [Test]
    public void UpdateBookmarkListName_ShouldEditBookmarkListName()
    {
        // Arrange
        var userId = 1;
        var listName = "Test List";
        var newTitle = "New Title";

        // Act
        _bookmarkListRepository.AddBookmarkList(userId, listName);
        var bookmarkListCount = _bookmarkListRepository.GetBookmarkLists(userId).Count;
        var bookmarkListId = _bookmarkListRepository.GetBookmarkListIdFromName(userId, listName);
        _bookmarkListRepository.UpdateBookmarkListName(userId, bookmarkListId, newTitle);

        // Assert
        Assert.That(_bookmarkListRepository.GetBookmarkLists(userId).Last().Title, Is.EqualTo(newTitle));
        // Check that it hasnt added a new bookmark list
        Assert.That(_bookmarkListRepository.GetBookmarkLists(userId).Count, Is.EqualTo(bookmarkListCount));
    }

    [Test]
    public void UpdateBookmarkListName_ShouldThrowExceptionWhenListIdIsNotFound()
    {
        // Arrange
        var userId = 1;
        var listId = 999;
        var newTitle = "New Title";

        // Assert
        Assert.Throws<ArgumentException>(() => _bookmarkListRepository.UpdateBookmarkListName(userId, listId, newTitle));
    }

    [Test]
    public void UpdateBookmarkListName_ShouldThrowExceptionWhenListNameIsNull()
    {
        // Arrange
        var userId = 1;
        var listName = "Test List";
        var newTitle = "";

        // Assert
        Assert.Throws<ArgumentNullException>(() => _bookmarkListRepository.UpdateBookmarkListName(userId, 1, newTitle));
    }

    [Test]
    public void UpdateBookmarkListName_ShouldThrowExceptionWhenListNameIsDuplicate()
    {
        // Arrange
        var userId = 1;
        var listName = "Test List";
        var newTitle = "New Title";

        // Act
        _bookmarkListRepository.AddBookmarkList(userId, listName);
        _bookmarkListRepository.AddBookmarkList(userId, newTitle);
        var bookmarkListId = _bookmarkListRepository.GetBookmarkListIdFromName(userId, listName);

        // Assert
        Assert.Throws<ArgumentException>(() => _bookmarkListRepository.UpdateBookmarkListName(userId, bookmarkListId, newTitle));
    }

    [Test]
    public void GetBookmarkListIdFromName_ShouldReturnListId()
    {
        // Arrange
        var userId = 1;
        var listName = "Wishlist events :)";

        // Act, assuming there is a bookmarkList seeded with the name 'Wishlist events :)'
        var result = _bookmarkListRepository.GetBookmarkListIdFromName(userId, listName);

        // Assert
        Assert.That(result, Is.EqualTo(1));
    }

    [Test]
    public void GetBookmarkListIdFromName_ShouldThrowExceptionWhenListNameIsNull()
    {
        // Arrange
        var userId = 1;
        var listName = "";

        // Assert
        Assert.Throws<ArgumentNullException>(() => _bookmarkListRepository.GetBookmarkListIdFromName(userId, listName));
    }

    [Test]
    public void GetBookmarkListIdFromName_ShouldThrowExceptionWhenListNameIsNotFound()
    {
        // Arrange
        var userId = 1;
        var listName = "Test List";

        // Act
        // Assert
        Assert.Throws<ArgumentException>(() => _bookmarkListRepository.GetBookmarkListIdFromName(userId, listName));
    }

    [Test]
    public void IsBookmarkListNameUnique_ShouldReturnTrue()
    {
        // Arrange
        var userId = 1;
        var listName = "Test List";

        // Act
        var result = _bookmarkListRepository.IsBookmarkListNameUnique(userId, listName);

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void IsBookmarkListNameUnique_ShouldReturnFalse()
    {
        // Arrange
        var userId = 1;
        var listName = "Wishlist events :)"; // assuming this list name already exists in SEED

        // Act
        var result = _bookmarkListRepository.IsBookmarkListNameUnique(userId, listName);

        // Assert
        Assert.That(result, Is.False);
    }
}
