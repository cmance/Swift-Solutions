USE [PopNGoDB];

-- Insert into PG_User
INSERT INTO [PG_User]
    ([ASPNETUserID])
VALUES
    ('6b6c5d33-e6d7-4d18-a565-a657eaf7a8a5'),
    ('bb60f066-da0e-46b5-83d0-4d73d306dc64'),
    ('c4081410-2b09-4992-803a-ee550c92c76a'),
    ('2ce786bf-706b-4ba4-87b3-b3f0cbc6a1ad');

-- Get the IDs of the users we just inserted
DECLARE @user1ID int = (SELECT [ID]
FROM [PG_User]
WHERE [ASPNETUserID] = '6b6c5d33-e6d7-4d18-a565-a657eaf7a8a5');
DECLARE @user2ID int = (SELECT [ID]
FROM [PG_User]
WHERE [ASPNETUserID] = 'bb60f066-da0e-46b5-83d0-4d73d306dc64');

-- Insert into Event
INSERT INTO [Event]
    ([ApiEventID], [EventDate], [EventName], [EventDescription], [EventLocation], [EventImage], [Latitude], [Longitude], [VenuePhoneNumber], [VenueName], [VenueRating], [VenueWebsite])
VALUES
    ('event1', '2022-01-01T00:00:00', 'Event 1', 'Description 1', 'Location 1', 'https://via.placeholder.com/150', 1.1, 1.1, '123-456-7890', 'Venue 1', 4.5, 'https://www.example.com');
INSERT INTO [Event]
    ([ApiEventID], [EventDate], [EventName], [EventDescription], [EventLocation], [EventImage], [Latitude], [Longitude], [VenuePhoneNumber], [VenueName], [VenueRating], [VenueWebsite])
VALUES
    ('event2', '2022-01-02T00:00:00', 'Event 2', 'Description 2', 'Location 2', 'https://via.placeholder.com/175', 2.2, 2.2, '456-456-7890', 'Venue 2', 4.6, 'https://www.example2.com');
INSERT INTO [Event]
    ([ApiEventID], [EventDate], [EventName], [EventDescription], [EventLocation], [EventImage], [Latitude], [Longitude], [VenuePhoneNumber], [VenueName], [VenueRating], [VenueWebsite])
VALUES
    ('event3', '2028-01-02T00:00:00', 'Event 3', 'Description 3', 'Location 3', null, 2.2, 2.2, '456-456-7890', 'Venue 3', 1.5, 'https://www.example3.com');
INSERT INTO [Event]
    ([ApiEventID], [EventDate], [EventName], [EventDescription], [EventLocation], [EventImage], [Latitude], [Longitude], [VenuePhoneNumber], [VenueName], [VenueRating], [VenueWebsite])
VALUES
    ('event4', '2010-01-02T00:00:00', 'Event 4', 'Description 4', 'Location 4', 'https://via.placeholder.com/200', 2.2, 2.2, '456-456-7890', 'Venue 4', 1.5, 'https://www.example4.com');

-- Get the IDs of the events we just inserted
DECLARE @event1ID int = (SELECT [ID]
FROM [Event]
WHERE [ApiEventID] = 'event1');
DECLARE @event2ID int = (SELECT [ID]
FROM [Event]
WHERE [ApiEventID] = 'event2');

-- Insert into BookmarkList
INSERT INTO [BookmarkList]
    ([UserID], [Title])
Values
    (@user1ID, 'Wishlist events :)')
DECLARE @bookmarkListId int = (SELECT [ID]
FROM [BookmarkList]
WHERE [Title] = 'Wishlist events :)');

-- Insert into FavoriteEvents
INSERT INTO [FavoriteEvents]
    ([BookmarkListID], [EventID])
VALUES
    (@bookmarkListId, @event1ID);
INSERT INTO [FavoriteEvents]
    ([BookmarkListID], [EventID])
VALUES
    (@bookmarkListId, @event2ID);

-- Insert into EventHistory
-- INSERT INTO [EventHistory]
--     ([UserID], [EventID], [ViewedDate])
-- VALUES
--     (@user1ID, @event1ID, '2022-01-01T00:00:00');
-- INSERT INTO [EventHistory]
--     ([UserID], [EventID], [ViewedDate])
-- VALUES
--     (@user2ID, @event2ID, '2022-01-02T00:00:00');

-- Insert into Tag
INSERT INTO [Tag]
    ([Name], [BackgroundColor], [TextColor])
VALUES
    ('Music', '#d35400', '#FFFFFF'),
    ('Show', '#3498db', '#FFFFFF'),
    ('Concert', '#2ecc71', '#FFFFFF'),
    ('Comedy', '#f1c40f', '#FFFFFF');

-- Insert into ScheduledNotification
INSERT INTO [ScheduledNotification]
    ([UserID], [Time], [Type])
VALUES
    (1, '2022-01-02T00:00:00', 'Favorites'),
    (2, '2022-01-02T00:00:00', 'Favorites'),
    (3, '2022-01-02T00:00:00', 'Favorites'),
    (4, '2022-01-02T00:00:00', 'Favorites');

-- Declare the cursor
DECLARE @aspnetUserID nvarchar(128);
DECLARE user_cursor CURSOR FOR 
SELECT [ASPNETUserID] FROM [PG_User];

-- Open the cursor
OPEN user_cursor;

-- Fetch the first row
FETCH NEXT FROM user_cursor INTO @aspnetUserID;

-- Iterate over each user
WHILE @@FETCH_STATUS = 0
BEGIN
    -- Get the ID of the user

    DECLARE @userID int = (SELECT [ID] FROM [PG_User] WHERE [ASPNETUserID] = @aspnetUserID);

    -- Insert a new wishlist for the user into BookmarkList
    INSERT INTO [BookmarkList] ([UserID], [Title]) VALUES (@userID, 'Favorites');

    -- Fetch the next row
    FETCH NEXT FROM user_cursor INTO @aspnetUserID;
END;

-- Close and deallocate the cursor
CLOSE user_cursor;

DEALLOCATE user_cursor;

INSERT INTO [Itinerary] ([UserID], [ItineraryTitle])
VALUES
  (@user1ID, 'New Year Trip'),
  (@user2ID, 'Weekend Getaway');

-- Get the IDs of the itineraries we just inserted
DECLARE @itinerary1ID int = (SELECT TOP 1 [ID] FROM [Itinerary] WHERE [UserID] = @user1ID);
DECLARE @itinerary2ID int = (SELECT TOP 1 [ID] FROM [Itinerary] WHERE [UserID] = @user2ID);

-- Insert into ItineraryEvents - associating events to itineraries
INSERT INTO [ItineraryEvents] ([ItineraryID], [EventID], [ReminderTime], [ReminderCustomTime])
VALUES
  (@itinerary1ID, @event1ID, 'hour', NULL), -- Itinerary 1 with Event 1
  (@itinerary2ID, @event2ID, 'hour', NULL) -- Itinerary 1 with Event 1
