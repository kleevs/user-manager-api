CREATE TABLE [UserManager].[User] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [Email]        NVARCHAR (50)  NOT NULL,
    [Password]     NVARCHAR (100) NOT NULL,
    [LastName]     NVARCHAR (50)  NOT NULL,
    [FirstName]    NVARCHAR (50)  NOT NULL,
    [IsActive]     BIT            NULL,
    [ParentUserId] INT            NULL,
    [BirthDate]    DATE           NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

