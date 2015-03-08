CREATE TABLE [dbo].[IdentityClaims] (
    [Id]              UNIQUEIDENTIFIER NOT NULL,
    [UserName]        NVARCHAR (128)   NULL,
    [Issuer]          NVARCHAR (128)   NULL,
    [OriginalIssuer]  NVARCHAR (128)   NULL,
    [Type]            NVARCHAR (128)   NULL,
    [Value]           NVARCHAR (128)   NULL,
    [ValueType]       NVARCHAR (128)   NULL,
    [SampleUser_Name] NVARCHAR (128)   NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [SampleUser_Claims] FOREIGN KEY ([SampleUser_Name]) REFERENCES [dbo].[SampleUsers] ([Name]) ON DELETE NO ACTION ON UPDATE NO ACTION,
    CONSTRAINT [User_Claims] FOREIGN KEY ([UserName]) REFERENCES [dbo].[Users] ([Name]) ON DELETE NO ACTION ON UPDATE NO ACTION
);

