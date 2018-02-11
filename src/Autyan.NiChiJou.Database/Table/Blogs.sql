CREATE TABLE [dbo].[Blogs]
(
	[Id] BIGINT NOT NULL PRIMARY KEY, 
	[BlogName] VARCHAR(200) NOT NULL, 
	[Descriptiong] VARCHAR(200) NULL, 
	[BlogUserId] BIGINT NOT NULL, 
	[CreatedAt] DATETIMEOFFSET NOT NULL, 
	[CreatedBy] BIGINT NOT NULL,
	[ModifiedAt] DATETIMEOFFSET NULL,
	[ModifiedBy] BIGINT NULL
	CONSTRAINT [FK_Blogs_ToBlogUser] FOREIGN KEY ([BlogUserId]) REFERENCES [BlogUsers]([Id])
)

GO

CREATE NONCLUSTERED INDEX [IX_Blogs_BlogUser] ON [dbo].[Blogs] ([BlogUserId])
