CREATE TABLE [dbo].[BasicEntities] (
    [Id]                     UNIQUEIDENTIFIER NOT NULL,
    [Title]                  NVARCHAR (128)   NULL,
    [BasicStringProperty]    NVARCHAR (128)   NULL,
    [AnotherStringProperty]  NVARCHAR (128)   NULL,
    [AnotherStringProperty2] NVARCHAR (128)   NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

