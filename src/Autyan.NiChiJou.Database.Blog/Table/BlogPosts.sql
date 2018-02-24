CREATE TABLE [dbo].[BlogPosts]
(
	[Id] BIGINT NOT NULL PRIMARY KEY, 
	[Title] VARCHAR(100) NOT NULL, 
	[Extract] VARCHAR(200) NULL, 
	[Content] NTEXT NOT NULL, 
	[BlogId] BIGINT NOT NULL, 
	[CreatedAt] DATETIMEOFFSET NOT NULL, 
	[CreatedBy] BIGINT NOT NULL,
	[ModifiedAt] DATETIMEOFFSET NULL,
	[ModifiedBy] BIGINT NULL
	CONSTRAINT [FK_BlogPosts_ToBlog] FOREIGN KEY ([BlogId]) REFERENCES [Blogs]([Id])
)

GO

CREATE NONCLUSTERED INDEX [IX_BlogPosts_BlogId] ON [dbo].[BlogPosts] ([BlogId])

GO

CREATE NONCLUSTERED INDEX [IX_BlogPosts_Title] ON [dbo].[BlogPosts] ([Title])