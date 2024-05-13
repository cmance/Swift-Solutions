using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using PopNGo.Data;
using PopNGo.Models;
using System.Diagnostics;
using System.Net;
using PopNGo.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PopNGo.DAL.Concrete;
using Humanizer;

namespace PopNGo_Tests;

/**
 * This is the recommended way to test using the in-memory db.  Seed the db and then write your tests based only on the seed
 * data + anything else you do to it.  No other tests will modify the db for that test.  Every test gets a brand new seeded db.
 * 
 */
public class AdminMetrics_Tests
{
    private static readonly string _seedFile = @"..\..\..\..\PopNGo\Data\Scripts\Testing\SEED.sql";  // relative path from where the executable is: bin/Debug/net7.0

    // Create this helper like this, for whatever context you desire
    private static readonly InMemoryDbHelper<PopNGoDB> _dbHelper = new(_seedFile, DbPersistence.OneDbPerTest);
    private static SearchRecordRepository _searchRecordRepository = null!;
    private static EmailHistoryRepository _emailHistoryRepository = null!;
    private static PopNGoDB _context = null!;

    [SetUp]
    public void Setup()
    {
        _context = _dbHelper.GetContext();
        _searchRecordRepository = new SearchRecordRepository(_context);
        _emailHistoryRepository = new EmailHistoryRepository(_context);
    }

    [Test]
    public void AddSearchRecord_ShouldAddNewSearchRecord()
    {
        // Arrange
        var searchRecordUserId = 1;
        var searchRecordTime = DateTime.Now;
        var searchRecordQuery = "Test Query";

        SearchRecord searchRecord = new()
        {
            UserId = searchRecordUserId,
            Time = searchRecordTime,
            SearchQuery = searchRecordQuery
        };

        // Act
        int newSearchRecordId = _searchRecordRepository.AddOrUpdate(searchRecord).Id;

        // Assert
        var addedSearchRecord = _searchRecordRepository.FindById(newSearchRecordId);
        Assert.That(addedSearchRecord, Is.Not.Null);

        Assert.That(addedSearchRecord.Time, Is.EqualTo(searchRecordTime));
        Assert.That(addedSearchRecord.SearchQuery, Is.EqualTo(searchRecordQuery));
        Assert.That(addedSearchRecord.UserId, Is.EqualTo(searchRecordUserId));
    }
    
    [Test]
    public async Task GetSearchRecordsWithinTime_ShouldReturnSearchRecordsWithinTime()
    {
        // Arrange
        var searchRecordUserId = 1;
        var searchRecordTime = DateTime.Now;
        var searchRecordQuery = "Test Query";

        SearchRecord searchRecord = new()
        {
            UserId = searchRecordUserId,
            Time = searchRecordTime,
            SearchQuery = searchRecordQuery
        };

        // Act
        int newSearchRecordId = _searchRecordRepository.AddOrUpdate(searchRecord).Id;
        var searchRecords = await _searchRecordRepository.GetSearchRecordsWithinTime(1);

        // Assert
        Assert.That(searchRecords.Count(), Is.EqualTo(1));
        Assert.That(searchRecords[0].Id, Is.EqualTo(newSearchRecordId));
    }

    [Test]
    public void AddEmailHistory_ShouldAddNewEmailHistory()
    {
        // Arrange
        var emailHistoryUserId = 1;
        var emailHistoryTime = DateTime.Now;
        var emailHistoryType = "Test Type";

        EmailHistory emailHistory = new()
        {
            UserId = emailHistoryUserId,
            TimeSent = emailHistoryTime,
            Type = emailHistoryType
        };

        // Act
        int newEmailHistoryId = _emailHistoryRepository.AddOrUpdate(emailHistory).Id;

        // Assert
        var addedEmailHistory = _emailHistoryRepository.FindById(newEmailHistoryId);
        Assert.That(addedEmailHistory, Is.Not.Null);

        Assert.That(addedEmailHistory.TimeSent, Is.EqualTo(emailHistoryTime));
        Assert.That(addedEmailHistory.Type, Is.EqualTo(emailHistoryType));
        Assert.That(addedEmailHistory.UserId, Is.EqualTo(emailHistoryUserId));
    }

    [Test]
    public async Task GetEmailHistoriesWithinTime_ShouldReturnEmailHistoriesWithinTime()
    {
        // Arrange
        var emailHistoryUserId = 1;
        var emailHistoryTime = DateTime.Now;
        var emailHistoryType = "Test Type";

        EmailHistory emailHistory = new()
        {
            UserId = emailHistoryUserId,
            TimeSent = emailHistoryTime,
            Type = emailHistoryType
        };

        // Act
        int newEmailHistoryId = _emailHistoryRepository.AddOrUpdate(emailHistory).Id;
        var emailHistories = await _emailHistoryRepository.GetEmailHistoriesWithinTime(1);

        // Assert
        Assert.That(emailHistories.Count(), Is.EqualTo(1));
        Assert.That(emailHistories[0].Id, Is.EqualTo(newEmailHistoryId));
    }

    [Test]
    public void PopNGoDBContext_HasBeenSeeded()
    {
        Console.WriteLine("Users: " + _context.PgUsers.Count());
        Assert.That(_context.PgUsers.Count(), Is.EqualTo(4));
    }
}