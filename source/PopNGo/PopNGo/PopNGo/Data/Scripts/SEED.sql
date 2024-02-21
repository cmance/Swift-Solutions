-- Seed the PG_User table
INSERT INTO [PG_User] ([ASPNETUserID])
VALUES 
('User1'),
('User2'),
('User3'),
('User4'),
('User5');

-- Seed the Event table
INSERT INTO [Event] ([EventID], [EventDate], [EventName], [EventDescription], [EventLocation])
VALUES 
('E1', '2022-12-01T00:00:00', 'Event 1', 'This is event 1', 'Location 1'),
('E2', '2022-12-02T00:00:00', 'Event 2', 'This is event 2', 'Location 2'),
('E3', '2022-12-03T00:00:00', 'Event 3', 'This is event 3', 'Location 3'),
('E4', '2022-12-04T00:00:00', 'Event 4', 'This is event 4', 'Location 4'),
('E5', '2022-12-05T00:00:00', 'Event 5', 'This is event 5', 'Location 5');

-- Seed the FavoriteEvents table
INSERT INTO [FavoriteEvents] ([UserID], [EventID])
VALUES 
(1, 1),  -- User 1 favorites Event 1
(2, 2),  -- User 2 favorites Event 2
(3, 3),  -- User 3 favorites Event 3
(4, 4),  -- User 4 favorites Event 4
(5, 5);  -- User 5 favorites Event 5

-- Seed the EventHistory table
INSERT INTO [EventHistory] ([UserID], [EventID], [ViewedDate])
VALUES 
(1, 1, '2022-12-01T00:00:00'),  -- User 1 viewed Event 1
(2, 2, '2022-12-02T00:00:00'),  -- User 2 viewed Event 2
(3, 3, '2022-12-03T00:00:00'),  -- User 3 viewed Event 3
(4, 4, '2022-12-04T00:00:00'),  -- User 4 viewed Event 4
(5, 5, '2022-12-05T00:00:00');  -- User 5 viewed Event 5