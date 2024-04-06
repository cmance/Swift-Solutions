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

-- Insert into FavoriteEvents
INSERT INTO [FavoriteEvents] ([UserID], [EventID]) VALUES (1, 1);
INSERT INTO [FavoriteEvents] ([UserID], [EventID]) VALUES (2, 2);

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