USE [PopNGoDB];

-- Drop the foreign key constraints
ALTER TABLE [EventHistory] DROP CONSTRAINT FK_EventHistory_UserID;
ALTER TABLE [EventHistory] DROP CONSTRAINT FK_EventHistory_EventID;
ALTER TABLE [FavoriteEvents] DROP CONSTRAINT FK_FavoriteEvents_UserID;

-- Drop the tables
DROP TABLE IF EXISTS [FavoriteEvents];
DROP TABLE IF EXISTS [EventHistory];
DROP TABLE IF EXISTS [Event];
DROP TABLE IF EXISTS [PG_User];

-- Drop the database
DROP DATABASE IF EXISTS [PopNGoDB];