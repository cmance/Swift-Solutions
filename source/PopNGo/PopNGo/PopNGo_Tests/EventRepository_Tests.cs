using Moq;
using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using PopNGo.DAL.Concrete;
using PopNGo.DAL.Abstract;
using PopNGo.Models.DTO;
using PopNGo.Models;
using System.Collections.Generic;
using System;


namespace PopNGo_Tests;

public class EventRepositoryTests
{
    private static readonly string _seedFile = @"..\..\..\..\PopNGo\Data\Scripts\Testing\SEED.sql";  // relative path from where the executable is: bin/Debug/net7.0

    // Create this helper like this, for whatever context you desire
    private static readonly InMemoryDbHelper<PopNGoDB> _dbHelper = new(_seedFile, DbPersistence.OneDbPerTest);
    private static EventRepository _eventRepository = null!;
    private static PopNGoDB _context = null!;


    [SetUp]
    public void Setup()
    {
        _context = _dbHelper.GetContext();
        _eventRepository = new EventRepository(_context);
    }

    [Test]
    public void GetEventFromApiId_ShouldReturnEvent()
    {
        // Arrange
        var eventId = "event1";

        // Act
        var result = _eventRepository.GetEventFromApiId(eventId);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.ApiEventId, Is.EqualTo(eventId));
    }

    [Test]
    public void GetEventFromApiId_ShouldErrorIfEventIdIsNull()
    {
        // Arrange
        var eventId = "";

        // Act
        var ex = Assert.Throws<ArgumentException>(() => _eventRepository.GetEventFromApiId(eventId));

        // Assert
        Assert.That(ex.Message, Is.EqualTo("ApiEventId cannot be null or empty (Parameter 'apiEventId')"));
    }

    [Test]
    public void AddEvent_ShouldAddNewEvent()
    {
        // Arrange
        var eventId = "event1";
        var eventDate = DateTime.Now;
        var eventName = "Event 1";
        var eventDescription = "Test Description";
        var eventLocation = "Test Location";
        var eventImage = "";
        IEnumerable<PopNGo.Models.DTO.TicketLink> ticketLinks = new List<PopNGo.Models.DTO.TicketLink>();

        var eventDetail = new EventDetail
        {
            EventID = eventId,
            EventStartTime = eventDate,
            EventName = eventName,
            EventDescription = eventDescription,
            Full_Address = eventLocation,
            EventThumbnail = eventImage,
            TicketLinks = ticketLinks
        };


        // Act
        _eventRepository.AddEvent(eventDetail);

        // Assert
        var events = _context.Events.ToList();
        Assert.That(events.Count, Is.EqualTo(7));
        var addedEvent = events.Last();
        Assert.That(addedEvent.ApiEventId, Is.EqualTo(eventDetail.EventID));
        Assert.That(addedEvent.EventDate, Is.EqualTo(eventDetail.EventStartTime.Value));
        Assert.That(addedEvent.EventName, Is.EqualTo(eventDetail.EventName));
        Assert.That(addedEvent.EventDescription, Is.EqualTo(eventDetail.EventDescription));
        Assert.That(addedEvent.EventLocation, Is.EqualTo(eventDetail.Full_Address));
        Assert.That(addedEvent.EventImage, Is.EqualTo(eventDetail.EventThumbnail));
    }

    [Test]
    public void AddEvent_ShouldErrorIfEventIdIsNull()
    {
        // Arrange
        var eventId = "";
        var eventDate = DateTime.Now;
        var eventName = "Test Event";
        var eventDescription = "Test Description";
        var eventLocation = "Test Location";
        var eventImage = "";

        var eventDetail = new EventDetail
        {
            EventID = eventId,
            EventStartTime = eventDate,
            EventName = eventName,
            EventDescription = eventDescription,
            Full_Address = eventLocation,
            EventThumbnail = eventImage
        };

        // Act
        var ex = Assert.Throws<ArgumentException>(() => _eventRepository.AddEvent(eventDetail));

        // Assert
        Assert.That(ex.Message, Is.EqualTo("EventId cannot be null or empty (Parameter 'EventID')"));
    }

    [Test]
    public void AddEvent_ShouldErrorIfEventDateIsDefault()
    {
        // Arrange
        var eventId = "1";
        var eventDate = default(DateTime);
        var eventName = "Test Event";
        var eventDescription = "Test Description";
        var eventLocation = "Test Location";
        var eventImage = "";

        var eventDetails = new EventDetail
        {
            EventID = eventId,
            EventStartTime = eventDate,
            EventName = eventName,
            EventDescription = eventDescription,
            Full_Address = eventLocation,
            EventThumbnail = eventImage
        };

        // Act
        var ex = Assert.Throws<ArgumentException>(() => _eventRepository.AddEvent(eventDetails));

        // Assert
        Assert.That(ex.Message, Is.EqualTo("EventDate cannot be default (Parameter 'EventStartTime')"));
    }

    [Test]
    public void AddEvent_ShouldErrorIfEventNameIsNull()
    {
        // Arrange
        var eventId = "1";
        var eventDate = DateTime.Now;
        var eventName = "";
        var eventDescription = "Test Description";
        var eventLocation = "Test Location";
        var eventImage = "";

        var eventDetails = new EventDetail
        {
            EventID = eventId,
            EventStartTime = eventDate,
            EventName = eventName,
            EventDescription = eventDescription,
            Full_Address = eventLocation,
            EventThumbnail = eventImage
        };

        // Act
        var ex = Assert.Throws<ArgumentException>(() => _eventRepository.AddEvent(eventDetails));

        // Assert
        Assert.That(ex.Message, Is.EqualTo("EventName cannot be null or empty (Parameter 'EventName')"));
    }

    [Test]
    public void IsEvent_ShouldReturnTrueIfEventExists()
    {
        // Arrange
        var eventId = "event1";

        // Act
        var result = _eventRepository.IsEvent(eventId);

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void IsEvent_ShouldReturnFalseIfEventDoesNotExist()
    {
        // Arrange
        var eventId = "fakeEvent";

        // Act
        var result = _eventRepository.IsEvent(eventId);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void IsEvent_ShouldErrorIfEventIdIsNull()
    {
        // Arrange
        var eventId = "";

        // Act
        var ex = Assert.Throws<ArgumentException>(() => _eventRepository.IsEvent(eventId));

        // Assert
        Assert.That(ex.Message, Is.EqualTo("ApiEventId cannot be null or empty (Parameter 'apiEventId')"));
    }
}
