CREATE TABLE [dbo].[Notifications] (
    [Id]                 UNIQUEIDENTIFIER NOT NULL,
    [UserName]           NVARCHAR (128)   NULL,
    [SendEmail]          BIT              NOT NULL,
    [IsActive]           BIT              NOT NULL,
    [IsHidden]           BIT              NOT NULL,
    [IsImportant]        BIT              NOT NULL,
    [Title]              NVARCHAR (1024)  NULL,
    [Message]            NVARCHAR (MAX)   NULL,
    [Created]            DATETIME         NOT NULL,
    [ImageSourceStorage] NVARCHAR (128)   NULL,
    [Action]             NVARCHAR (128)   NULL,
    [From_Name]          NVARCHAR (128)   NULL,
    [From_FullName]      NVARCHAR (128)   NULL,
    [RequestIdsStorage]  NVARCHAR (128)   NULL,
    [Discriminator]      NVARCHAR (128)   NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

