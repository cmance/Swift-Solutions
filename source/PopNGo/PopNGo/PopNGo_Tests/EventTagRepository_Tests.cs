using Moq;
using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using PopNGo.DAL.Concrete;
using PopNGo.DAL.Abstract;
using PopNGo.Models;
using System.Collections.Generic;

namespace PopNGo_Tests;

public class EventTagRepositoryTests
{
    private static readonly string _seedFile = @"../../../Sql/SEED.sql";  // relative path from where the executable is: bin/Debug/net7.0

    // Create this helper like this, for whatever context you desire
    private static readonly InMemoryDbHelper<PopNGoDB> _dbHelper = new(_seedFile, DbPersistence.OneDbPerTest);
    private static EventTagRepository _eventTagRepository = null!;
    private static PopNGoDB _context = null!;

    [SetUp]
    public void Setup()
    {
        _context = _dbHelper.GetContext();
        _eventTagRepository = new EventTagRepository(_context);
    }

    [Test]
    public void AddEventTag_ShouldAddEventTag()
    {
        // Arrange
        // Tags are seeded in the database
        var tagId = 1;
        var eventId = 1;

        // Act
        _eventTagRepository.AddEventTag(tagId, eventId);

        // Assert
        var eventTags = _eventTagRepository.GetEventTags(eventId);
        var eventTag = eventTags.First();
        Assert.AreEqual(tagId, eventTag.TagId);
        Assert.AreEqual(eventId, eventTag.EventId);
    }    
}
