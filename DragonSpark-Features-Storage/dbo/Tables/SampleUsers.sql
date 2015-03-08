CREATE TABLE [dbo].[SampleUsers] (
    [Name]                NVARCHAR (128) NOT NULL,
    [SomeStringProperty]  NVARCHAR (128) NULL,
    [SomeDateProperty]    DATETIME       NULL,
    [SomeBooleanProperty] BIT            NOT NULL,
    [DisplayName]         NVARCHAR (128) NULL,
    [FirstName]           NVARCHAR (128) NULL,
    [LastName]            NVARCHAR (128) NULL,
    [LastActivity]        DATETIME       NULL,
    [MembershipNumber]    BIGINT         NULL,
    [JoinedDate]          DATETIME       NULL,
    [IsEnabled]           BIT            NOT NULL,
    [RolesSource]         NVARCHAR (128) NULL,
    PRIMARY KEY CLUSTERED ([Name] ASC)
);

