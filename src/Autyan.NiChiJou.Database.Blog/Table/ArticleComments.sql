CREATE TABLE [dbo].[ArticleComments]
(
	[Id] BIGINT NOT NULL PRIMARY KEY, 
	[Content] NVARCHAR(2000) NOT NULL, 
	[BlogUserId] BIGINT NOT NULL, 
	[PostId] BIGINT NOT NULL, 
	[ToComment] BIGINT NULL, 
	[CreatedAt] DATETIMEOFFSET NOT NULL, 
	[CreatedBy] BIGINT NOT NULL,
	[ModifiedAt] DATETIMEOFFSET NULL,
	[ModifiedBy] BIGINT NULL
	CONSTRAINT [FK_PostComments_ToArticle] FOREIGN KEY ([PostId]) REFERENCES [Articles]([Id]), 
	CONSTRAINT [FK_PostComments_ToBlogUser] FOREIGN KEY ([BlogUserId]) REFERENCES [BlogUsers]([Id])
)

GO

CREATE NONCLUSTERED INDEX [IX_ArticleComments_BlogUserId] ON [dbo].[ArticleComments] ([BlogUserId])

GO

CREATE NONCLUSTERED INDEX [IX_ArticleComments_PostId] ON [dbo].[ArticleComments] ([PostId])
