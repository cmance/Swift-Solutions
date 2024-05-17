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

namespace PopNGo_Tests;

/**
 * This is the recommended way to test using the in-memory db.  Seed the db and then write your tests based only on the seed
 * data + anything else you do to it.  No other tests will modify the db for that test.  Every test gets a brand new seeded db.
 * 
 */
public class Admin_Tests
{
    private static readonly string _seedFile = @"../../../Sql/SEED.sql";  // relative path from where the executable is: bin/Debug/net7.0

    // Create this helper like this, for whatever context you desire
    private static readonly InMemoryDbHelper<PopNGoDB> _dbHelper = new(_seedFile, DbPersistence.OneDbPerTest);
    private static ScheduledNotificationRepository _scheduledNotificationRepository = null!;
    private static PopNGoDB _context = null!;

    [SetUp]
    public void Setup()
    {
        _context = _dbHelper.GetContext();
        _scheduledNotificationRepository = new ScheduledNotificationRepository(_context);
    }

    [Test]
    public async Task AddScheduledNotification_ShouldAddNewScheduledNotification()
    {
        // Arrange
        var scheduledNotificationUserId = 1;
        var scheduledNotificationTime = DateTime.Now;
        var scheduledNotificationType = "Upcoming Events";

        // Act
        int newEventId = await _scheduledNotificationRepository.AddScheduledNotification(scheduledNotificationUserId, scheduledNotificationTime, scheduledNotificationType);

        // Assert
        var addedScheduledNotification = await _context.ScheduledNotifications.FindAsync(newEventId);
        Assert.That(addedScheduledNotification, Is.Not.Null);

        Assert.That(addedScheduledNotification.Time, Is.EqualTo(scheduledNotificationTime));
        Assert.That(addedScheduledNotification.Type, Is.EqualTo(scheduledNotificationType));
        Assert.That(addedScheduledNotification.UserId, Is.EqualTo(scheduledNotificationUserId));
    }

    [Test]
    public async Task UpdateScheduledNotification_ChangeTimeToPast_ShouldFail()
    {
        // Arrange
        var scheduledNotificationUserId = 1;
        var scheduledNotificationTime = DateTime.Now;
        var scheduledNotificationType = "Upcoming Events";

        // Act
        int newEventId = await _scheduledNotificationRepository.AddScheduledNotification(scheduledNotificationUserId, scheduledNotificationTime, scheduledNotificationType);

        // Assert
        var addedScheduledNotification = await _context.ScheduledNotifications.FindAsync(newEventId);
        Assert.That(addedScheduledNotification, Is.Not.Null);

        // Act
        var newTime = DateTime.Now.AddDays(-1);
        var result = await _scheduledNotificationRepository.UpdateScheduledNotification(newEventId, newTime, scheduledNotificationType);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public async Task UpdateScheduledNotification_ChangeTimeToFuture_ShouldSucceed()
    {
        // Arrange
        var scheduledNotificationUserId = 1;
        var scheduledNotificationTime = DateTime.Now;
        var scheduledNotificationType = "Upcoming Events";

        // Act
        int newEventId = await _scheduledNotificationRepository.AddScheduledNotification(scheduledNotificationUserId, scheduledNotificationTime, scheduledNotificationType);

        // Assert
        var addedScheduledNotification = await _context.ScheduledNotifications.FindAsync(newEventId);
        Assert.That(addedScheduledNotification, Is.Not.Null);

        // Act
        var newTime = DateTime.Now.AddDays(1);
        var result = await _scheduledNotificationRepository.UpdateScheduledNotification(newEventId, newTime, scheduledNotificationType);

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public async Task DeleteScheduledNotification_ShouldDeleteScheduledNotification()
    {
        // Arrange
        var scheduledNotificationUserId = 1;
        var scheduledNotificationTime = DateTime.Now;
        var scheduledNotificationType = "Upcoming Events";

        // Act
        int newEventId = await _scheduledNotificationRepository.AddScheduledNotification(scheduledNotificationUserId, scheduledNotificationTime, scheduledNotificationType);

        // Assert
        var addedScheduledNotification = await _context.ScheduledNotifications.FindAsync(newEventId);
        Assert.That(addedScheduledNotification, Is.Not.Null);

        // Act
        var result = await _scheduledNotificationRepository.DeleteScheduledNotification(newEventId);

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public async Task DeleteScheduledNotification_Succeeded_ShouldRescheduleNewNotification()
    {
        // Arrange
        var scheduledNotificationUserId = 1;
        var scheduledNotificationTime = DateTime.Now;
        var scheduledNotificationType = "Upcoming Events";
        int newEventId = await _scheduledNotificationRepository.AddScheduledNotification(scheduledNotificationUserId, scheduledNotificationTime, scheduledNotificationType);

        // Act
        var addedScheduledNotification = await _context.ScheduledNotifications.FindAsync(newEventId);
        Assert.That(addedScheduledNotification, Is.Not.Null);

        var result = await _scheduledNotificationRepository.DeleteScheduledNotification(newEventId);
        Assert.That(result, Is.True);

        // Assert
        var newEventId2 = await _scheduledNotificationRepository.FindByUserIdAsync(scheduledNotificationUserId, scheduledNotificationType);
        var addedScheduledNotification2 = await _context.ScheduledNotifications.FindAsync(newEventId2);

        Assert.That(addedScheduledNotification2, Is.Not.Null);
        Assert.That(newEventId2, Is.Not.EqualTo(newEventId));
    }

    [Test]
    public async Task FindByUserIdAsync_ShouldReturnScheduledNotification()
    {
        // Arrange
        var scheduledNotificationUserId = 4;
        var scheduledNotificationTime = DateTime.Now;
        var scheduledNotificationType = "Upcoming Events";
        int newEventId = await _scheduledNotificationRepository.AddScheduledNotification(scheduledNotificationUserId, scheduledNotificationTime, scheduledNotificationType);

        // Act
        var addedScheduledNotification = await _context.ScheduledNotifications.FindAsync(newEventId);
        Assert.That(addedScheduledNotification, Is.Not.Null);

        await _scheduledNotificationRepository.CleanUpScheduledNotifications(scheduledNotificationTime);

        var result = await _scheduledNotificationRepository.FindByUserIdAsync(scheduledNotificationUserId, scheduledNotificationType);

        // Assert
        Assert.That(result, Is.EqualTo(newEventId));
    }

    [Test]
    public async Task FindByUserIdAsync_ShouldReturnNullIfNoScheduledNotification()
    {
        // Arrange
        var scheduledNotificationUserId = -1;

        // Act
        var result = await _scheduledNotificationRepository.FindByUserIdAsync(scheduledNotificationUserId, "Upcoming Events");

        // Assert
        Assert.That(result, Is.LessThan(0));
    }

    [Test]
    public async Task ScheduledNotifications_AddScheduledNotification_ShouldFailIfExistsAlready()
    {
        // Arrange
        var scheduledNotificationUserId = 1;
        var scheduledNotificationTime = DateTime.Now;
        var scheduledNotificationType = "Upcoming Events";
        var scheduledNotificationTime2 = DateTime.Now.AddDays(1);

        // Act
        var newEventId = await _scheduledNotificationRepository.AddScheduledNotification(scheduledNotificationUserId, scheduledNotificationTime, scheduledNotificationType);
        var newEventId2 = await _scheduledNotificationRepository.AddScheduledNotification(scheduledNotificationUserId, scheduledNotificationTime2, scheduledNotificationType);

        // Assert
        Assert.That(newEventId, Is.Not.EqualTo(newEventId2));
        Assert.That(newEventId2, Is.LessThan(0));
    }

    [Test]
    public void PopNGoDBContext_HasBeenSeeded()
    {
        Console.WriteLine("Users: " + _context.PgUsers.Count());
        Assert.That(_context.PgUsers.Count(), Is.EqualTo(4));
    }
}