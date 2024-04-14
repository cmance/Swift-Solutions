USE [PopNGoDB];

-- Drop the foreign key constraints
ALTER TABLE [EventHistory] DROP CONSTRAINT FK_EventHistory_UserID;
ALTER TABLE [EventHistory] DROP CONSTRAINT FK_EventHistory_EventID;

ALTER TABLE [FavoriteEvents] DROP CONSTRAINT FK_FavoriteEvents_EventID;
ALTER TABLE [FavoriteEvents] DROP CONSTRAINT FK_FavoriteEvents_BookmarkListID;

ALTER TABLE [TicketLink] DROP CONSTRAINT FK_TicketLink_EventID;

ALTER TABLE [ScheduledNotification] DROP CONSTRAINT FK_ScheduledNotification_UserID;

-- Drop the tables
DROP TABLE [PG_User];
DROP TABLE [TicketLink];
DROP TABLE [TAG];
DROP TABLE [FavoriteEvents];
DROP TABLE [BookmarkList];
DROP TABLE [EventHistory];
DROP TABLE [Event];
DROP TABLE [ScheduledNotification];