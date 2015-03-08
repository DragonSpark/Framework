CREATE TABLE [dbo].[EntityWithReferences] (
    [Id]                 UNIQUEIDENTIFIER NOT NULL,
    [Title]              NVARCHAR (128)   NULL,
    [ReferencedEntityId] UNIQUEIDENTIFIER NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [EntityWithReference_ReferencedEntity] FOREIGN KEY ([ReferencedEntityId]) REFERENCES [dbo].[ReferencedEntities] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION
);

