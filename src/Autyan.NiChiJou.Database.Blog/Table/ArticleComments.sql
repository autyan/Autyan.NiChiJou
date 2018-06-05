CREATE TABLE [dbo].[ArticleComments]
(
	[Id] BIGINT NOT NULL PRIMARY KEY, 
	[Content] NVARCHAR(4000) NOT NULL, 
	[CommentedBy] BIGINT NULL, 
	[PostId] BIGINT NOT NULL, 
	[ToComment] BIGINT NULL, 
	[CreatedAt] datetime2(7) NOT NULL, 
	[CreatedBy] BIGINT NOT NULL,
	[ModifiedAt] datetime2(7) NULL,
	[ModifiedBy] BIGINT NULL
)

GO

CREATE NONCLUSTERED INDEX [IX_ArticleComments_PostId] ON [dbo].[ArticleComments] ([PostId])
