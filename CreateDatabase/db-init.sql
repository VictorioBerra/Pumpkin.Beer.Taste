USE [master]
GO

IF DB_ID('BlindTasteTest') IS NOT NULL
  set noexec on 

CREATE DATABASE [BlindTasteTest];
GO

USE [BlindTasteTest]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE SCHEMA app
GO

CREATE LOGIN pumpkinuser WITH PASSWORD = '$(password)'
GO

CREATE USER pumpkinuser FOR LOGIN pumpkinuser WITH DEFAULT_SCHEMA=[app]
GO

EXEC sp_addrolemember N'db_owner', N'pumpkinuser'
GO

PRINT 'Database and user created successfully'
