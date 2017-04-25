
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 04/20/2017 20:32:56
-- Generated from EDMX file: C:\Users\Kotimonni\Documents\Visual Studio 2017\Projects\WpfApp2\WpfApp2\TietokantaEntity.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [Tietokanta];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[jasenetSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[jasenetSet];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'jasenetSet'
CREATE TABLE [dbo].[jasenetSet] (
    [avain] int IDENTITY(1,1) NOT NULL,
    [hetu] nvarchar(max)  NOT NULL,
    [snimi] nvarchar(max)  NOT NULL,
    [enimet] nvarchar(max)  NOT NULL,
    [osoite] nvarchar(max)  NOT NULL,
    [postinumero] int  NOT NULL,
    [postitoimipaikka] nvarchar(max)  NOT NULL,
    [puhelinnumero] nvarchar(max)  NOT NULL,
    [email] nvarchar(max)  NOT NULL,
    [liittymispv] datetime  NOT NULL,
    [maksettu] int  NOT NULL,
    [lisatietoa] nvarchar(max)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [avain] in table 'jasenetSet'
ALTER TABLE [dbo].[jasenetSet]
ADD CONSTRAINT [PK_jasenetSet]
    PRIMARY KEY CLUSTERED ([avain] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------