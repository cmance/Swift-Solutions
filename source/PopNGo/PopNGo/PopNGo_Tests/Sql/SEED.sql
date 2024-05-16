-- Uses SQLLite syntax

-- Insert into PG_User
INSERT INTO [PG_User] ([ASPNETUserID])
    VALUES
    ('6b6c5d33-e6d7-4d18-a565-a657eaf7a8a5'),
    ('bb60f066-da0e-46b5-83d0-4d73d306dc64'),
    ('c4081410-2b09-4992-803a-ee550c92c76a'),
    ('2ce786bf-706b-4ba4-87b3-b3f0cbc6a1ad');

-- Insert into Event
INSERT INTO [Event] ([ApiEventID], [EventDate], [EventName], [EventDescription], [EventLocation], [EventImage]) 
VALUES ('event1', '2022-01-01T00:00:00', 'Event 1', 'Description 1', 'Location 1', 'https://via.placeholder.com/150');
INSERT INTO [Event] ([ApiEventID], [EventDate], [EventName], [EventDescription], [EventLocation], [EventImage]) 
VALUES ('event2', '2022-01-02T00:00:00', 'Event 2', 'Description 2', 'Location 2', 'https://via.placeholder.com/150');
INSERT INTO [Event] ([ApiEventID], [EventDate], [EventName], [EventDescription], [EventLocation], [EventImage], [Latitude], [Longitude], [VenuePhoneNumber], [VenueName], [VenueRating], [VenueWebsite])
VALUES ('event3', '2028-01-02T00:00:00', 'Event 3', 'Description 3', 'Location 3', null, 2.2, 2.2, '456-456-7890', 'Venue 3', 1.5, 'https://www.example3.com');
INSERT INTO [Event] ([ApiEventID], [EventDate], [EventName], [EventDescription], [EventLocation], [EventImage], [Latitude], [Longitude], [VenuePhoneNumber], [VenueName], [VenueRating], [VenueWebsite])
VALUES ('event4', '2010-01-02T00:00:00', 'Event 4', 'Description 4', 'Location 4', 'https://via.placeholder.com/200', 2.2, 2.2, '456-456-7890', 'Venue 4', 1.5, 'https://www.example4.com');
INSERT INTO [Event] ([ApiEventID], [EventDate], [EventName], [EventDescription], [EventLocation], [EventImage], [Latitude], [Longitude], [VenuePhoneNumber], [VenueName], [VenueRating], [VenueWebsite])
VALUES ('event5', '2027-01-02T00:00:00', 'Event 5', 'Description 5', 'Location 5', 'event5.jpg', 2.2, 2.2, '456-456-7890', 'Venue 5', 1.5, 'https://www.example5.com');
INSERT INTO [Event] ([ApiEventID], [EventDate], [EventName], [EventDescription], [EventLocation], [EventImage], [Latitude], [Longitude], [VenuePhoneNumber], [VenueName], [VenueRating], [VenueWebsite])
VALUES ('event6', '2029-01-02T00:00:00', 'Event 6', 'Description 6', 'Location 6', 'event6.jpg', 2.2, 2.2, '456-456-7890', 'Venue 6', 1.5, 'https://www.example6.com');

-- Insert into BookmarkList
INSERT INTO [BookmarkList] ([UserID], [Title]) VALUES (1, 'Wishlist events :)');
INSERT INTO [BookmarkList] ([UserID], [Title]) VALUES (2, 'Concerts!');

-- Insert into FavoriteEvents
INSERT INTO [FavoriteEvents] ([BookmarkListID], [EventID]) VALUES (1, 1);
INSERT INTO [FavoriteEvents] ([BookmarkListID], [EventID]) VALUES (2, 2);

-- Insert into EventHistory
INSERT INTO [EventHistory] ([UserID], [EventID], [ViewedDate]) VALUES (1, 1, '2022-01-01T00:00:00');
INSERT INTO [EventHistory] ([UserID], [EventID], [ViewedDate]) VALUES (2, 2, '2022-01-02T00:00:00');

-- Insert into Tag
INSERT INTO [Tag] ([Name], [BackgroundColor], [TextColor])
    VALUES
    ('Music', '#d35400', '#FFFFFF'),
    ('Show', '#3498db', '#FFFFFF'),
    ('Concert', '#2ecc71', '#FFFFFF'),
    ('Comedy', '#f1c40f', '#FFFFFF');

-- Insert into ScheduledNotification
INSERT INTO [ScheduledNotification] ([UserID], [Time], [Type])
    VALUES
    (1, '2022-01-02T00:00:00', 'Favorites'),
    (2, '2022-01-02T00:00:00', 'Favorites'),
    (3, '2022-01-02T00:00:00', 'Favorites'),
    (4, '2022-01-02T00:00:00', 'Favorites');