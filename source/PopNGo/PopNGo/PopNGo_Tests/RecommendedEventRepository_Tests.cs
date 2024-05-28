using Moq;
using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using PopNGo.DAL.Concrete;
using PopNGo.DAL.Abstract;
using PopNGo.Models;
using System.Collections.Generic;

namespace PopNGo_Tests;

public class RecommendedEventRepositoryTests
{
    private static readonly string _seedFile = @"../../../Sql/SEED.sql";  // relative path from where the executable is: bin/Debug/net7.0
    private static readonly InMemoryDbHelper<PopNGoDB> _dbHelper = new(_seedFile, DbPersistence.OneDbPerTest);
    private static RecommendedEventRepository _recommendedEventRepository = null!;
    private static PopNGoDB _context = null!;

    [SetUp]
    public void Setup()
    {
        _context = _dbHelper.GetContext();
        _recommendedEventRepository = new RecommendedEventRepository(_context);
    }

    [Test]
    public void GetRecommendedEvents_ShouldReturnEvents()
    {
        // Arrange
        var userId1 = 1;
        var event1 = new RecommendedEvent { Id = 1, UserId = userId1, EventId = 1 };
        var event2 = new RecommendedEvent { Id = 2, UserId = userId1, EventId = 2 };
        var event3 = new RecommendedEvent { Id = 3, UserId = userId1, EventId = 3 };

        var userId2 = 2;
        var event4 = new RecommendedEvent { Id = 4, UserId = userId2, EventId = 4 };
        var event5 = new RecommendedEvent { Id = 5, UserId = userId2, EventId = 5 };
        var event6 = new RecommendedEvent { Id = 6, UserId = userId2, EventId = 6 };

        _context.RecommendedEvents.AddRange(event1, event2, event3, event4, event5, event6);
        _context.SaveChanges();

        // Act
        var result_user_1 = _recommendedEventRepository.GetRecommendedEvents(userId1).OrderBy(e => e.Id).ToList();

        var result_user_2 = _recommendedEventRepository.GetRecommendedEvents(userId2).OrderBy(e => e.Id).ToList();

        // Assert
        Assert.That(result_user_1.Count, Is.EqualTo(3));
        Assert.That(result_user_1[0].Id, Is.EqualTo(1));
        Assert.That(result_user_1[1].Id, Is.EqualTo(2));
        Assert.That(result_user_1[2].Id, Is.EqualTo(3));

        Assert.That(result_user_2.Count, Is.EqualTo(3));
        Assert.That(result_user_2[0].Id, Is.EqualTo(4));
        Assert.That(result_user_2[1].Id, Is.EqualTo(5));
        Assert.That(result_user_2[2].Id, Is.EqualTo(6));
    }

    [Test]
    public void SetRecommendedEvents_ShouldSetEvents()
    {
        // Arrange
        var userId1 = 1;
        var eventIds = new List<string> { "event1", "event2", "event3" };

        var userId2 = 2;
        var eventIds2 = new List<string> { "event1", "event2", "event3" };

        // Act
        _recommendedEventRepository.SetRecommendedEvents(userId1, eventIds);
        _recommendedEventRepository.SetRecommendedEvents(userId2, eventIds2);

        // Assert
        var result_user_1 = _recommendedEventRepository.GetRecommendedEvents(userId1).OrderBy(e => e.Id).ToList();
        Assert.That(result_user_1.Count, Is.EqualTo(3));
        Assert.That(result_user_1[0].ApiEventID, Is.EqualTo("event1"));
        Assert.That(result_user_1[1].ApiEventID, Is.EqualTo("event2"));
        Assert.That(result_user_1[2].ApiEventID, Is.EqualTo("event3"));

        var result_user_2 = _recommendedEventRepository.GetRecommendedEvents(userId2).OrderBy(e => e.Id).ToList();
        Assert.That(result_user_2.Count, Is.EqualTo(3));
        Assert.That(result_user_2[0].ApiEventID, Is.EqualTo("event1"));
        Assert.That(result_user_2[1].ApiEventID, Is.EqualTo("event2"));
        Assert.That(result_user_2[2].ApiEventID, Is.EqualTo("event3"));
    }
}
