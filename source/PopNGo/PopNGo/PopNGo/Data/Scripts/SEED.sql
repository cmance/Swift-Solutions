-- Insert a user into PG_User
INSERT INTO [PG_User] (ASPNETUserID)
VALUES ('user1');

-- Get the ID of the user we just inserted
DECLARE @UserID int;
SET @UserID = SCOPE_IDENTITY();

-- Insert an event into Event
INSERT INTO [Event] (EventID, EventDate, EventName, EventDescription, EventLocation)
VALUES ('event1', '2022-01-01T00:00:00', 'New Year Party', 'A fun party to celebrate the new year', '123 Party Street');

-- Get the EventID of the event we just inserted
DECLARE @EventID nvarchar(255);
SET @EventID = 'event1';

-- Insert a record into FavoriteEvents for the user and event
INSERT INTO [FavoriteEvents] (UserID, EventID)
VALUES (@UserID, @EventID);

-- Insert a record into EventHistory for the user and event
INSERT INTO [EventHistory] (UserID, EventID, ViewedDate)
VALUES (@UserID, @EventID, '2022-01-01T00:00:00');