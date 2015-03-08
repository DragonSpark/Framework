CREATE TABLE [dbo].[CollectionEntities] (
    [Id]                     UNIQUEIDENTIFIER NOT NULL,
    [Title]                  NVARCHAR (128)   NULL,
    [Description]            NVARCHAR (128)   NULL,
    [EntityWithCollectionId] UNIQUEIDENTIFIER NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [EntityWithCollection_Entities] FOREIGN KEY ([EntityWithCollectionId]) REFERENCES [dbo].[EntityWithCollections] ([Id]) ON DELETE CASCADE ON UPDATE NO ACTION
);

