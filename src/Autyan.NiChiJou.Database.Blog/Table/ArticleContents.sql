CREATE TABLE [dbo].[ArticleContents]
(
	[Id] BIGINT NOT NULL PRIMARY KEY,
	[ArticleId] BIGINT NOT NULL,
	[Content] NVARCHAR(MAX) NOT NULL,
	[CreatedAt] datetime2(7) NOT NULL, 
	[CreatedBy] BIGINT NOT NULL,
	[ModifiedAt] datetime2(7) NULL,
	[ModifiedBy] BIGINT NULL
)

GO

CREATE NONCLUSTERED INDEX [IX_ArticleContents_ArticleId] ON [dbo].[ArticleContents] ([ArticleId])
