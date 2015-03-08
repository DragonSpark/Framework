CREATE TABLE [dbo].[InstallationEntries] (
    [Id]               UNIQUEIDENTIFIER NOT NULL,
    [VersionStorage]   NVARCHAR (128)   NOT NULL,
    [InstallationDate] DATETIME         NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC, [VersionStorage] ASC)
);

