CREATE TABLE [dbo].[ReferencedEntities] (
    [Id]          UNIQUEIDENTIFIER NOT NULL,
    [Title]       NVARCHAR (128)   NULL,
    [Description] NVARCHAR (128)   NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

