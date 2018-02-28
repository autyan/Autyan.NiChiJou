CREATE TABLE [dbo].[Article]
(
	[Id] BIGINT NOT NULL PRIMARY KEY, 
	[Title] NVARCHAR(100) NOT NULL, 
	[Extract] NVARCHAR(200) NULL, 
	[Content] NVARCHAR(MAX) NOT NULL, 
	[BlogId] BIGINT NOT NULL, 
	[CreatedAt] DATETIMEOFFSET NOT NULL, 
	[CreatedBy] BIGINT NOT NULL,
	[ModifiedAt] DATETIMEOFFSET NULL,
	[ModifiedBy] BIGINT NULL
	CONSTRAINT [FK_BlogPosts_ToBlog] FOREIGN KEY ([BlogId]) REFERENCES [Blogs]([Id])
)

GO

CREATE NONCLUSTERED INDEX [IX_Article_BlogId] ON [dbo].[Article] ([BlogId])

GO

CREATE NONCLUSTERED INDEX [IX_Article_Title] ON [dbo].[Article] ([Title])