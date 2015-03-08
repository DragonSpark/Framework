CREATE TABLE [dbo].[EdmMetadata] (
    [Id]        INT            IDENTITY (1, 1) NOT NULL,
    [ModelHash] NVARCHAR (128) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

