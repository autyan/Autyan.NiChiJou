CREATE TABLE [dbo].[ArticleContents]
(
	[Id] BIGINT NOT NULL PRIMARY KEY,
	[ArticleId] BIGINT NOT NULL,
	[CreatedAt] DATETIMEOFFSET NOT NULL, 
	[CreatedBy] BIGINT NOT NULL,
	[ModifiedAt] DATETIMEOFFSET NULL,
	[ModifiedBy] BIGINT NULL
)

GO

CREATE NONCLUSTERED INDEX [IX_ArticleContents_ArticleId] ON [dbo].[ArticleContents] ([ArticleId])
