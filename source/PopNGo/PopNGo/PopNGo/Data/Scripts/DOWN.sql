USE [PopNGoDB];

-- Drop the foreign key constraints
ALTER TABLE [EventHistory] DROP CONSTRAINT FK_EventHistory_UserID;
ALTER TABLE [EventHistory] DROP CONSTRAINT FK_EventHistory_EventID;

ALTER TABLE [FavoriteEvents] DROP CONSTRAINT FK_FavoriteEvents_EventID;
ALTER TABLE [FavoriteEvents] DROP CONSTRAINT FK_FavoriteEvents_BookmarkListID;

ALTER TABLE [TicketLink] DROP CONSTRAINT FK_TicketLink_EventID;

ALTER TABLE [ScheduledNotification] DROP CONSTRAINT FK_ScheduledNotification_UserID;

ALTER TABLE [WeatherForecast] DROP CONSTRAINT FK_WeatherForecast_WeatherId;

ALTER TABLE [EmailHistory] DROP CONSTRAINT FK_EmailHistory_UserID;

ALTER TABLE [ItineraryEvents] DROP CONSTRAINT FK_ItineraryEvents_ItineraryID;
ALTER TABLE [ItineraryEvents] DROP CONSTRAINT FK_ItineraryEvents_EventID;

ALTER TABLE [ItineraryNotifications] DROP CONSTRAINT FK_ItineraryNotifications_ItineraryID;

ALTER TABLE [Itinerary] DROP CONSTRAINT FK_Itinerary_UserID;

ALTER TABLE [EventTag] DROP CONSTRAINT FK_EventTag_TagId;
ALTER TABLE [EventTag] DROP CONSTRAINT FK_EventTag_EventId;

ALTER TABLE [RecommendedEvent] DROP CONSTRAINT FK_RecommendedEvent_EventID;
ALTER TABLE [RecommendedEvent] DROP CONSTRAINT FK_RecommendedEvent_UserID;


-- Drop the tables
DROP TABLE [PG_User];
DROP TABLE [TicketLink];
DROP TABLE [TAG];
DROP TABLE [FavoriteEvents];
DROP TABLE [BookmarkList];
DROP TABLE [EventHistory];
DROP TABLE [Event];
DROP TABLE [ScheduledNotification];
DROP TABLE [WeatherForecast];
DROP TABLE [Weather];
DROP TABLE [EmailHistory];
DROP TABLE [SearchRecord];
DROP TABLE [AccountRecord];
DROP TABLE [ItineraryEvents];
DROP TABLE [ItineraryNotifications];
DROP TABLE [Itinerary];
DROP TABLE [EventTag];
DROP TABLE [RecommendedEvent];
