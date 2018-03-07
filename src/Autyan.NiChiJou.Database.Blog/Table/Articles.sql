CREATE TABLE [dbo].[Articles]
(
	[Id] BIGINT NOT NULL PRIMARY KEY, 
	[Title] NVARCHAR(200) NOT NULL, 
	[Extract] NVARCHAR(500) NULL, 
	[BlogId] BIGINT NOT NULL, 
	[Reads] INT NOT NULL,
	[Comments] INT NOT NULL,
	[LastReadAt] datetime2(7) NULL,
	[CreatedAt] datetime2(7) NOT NULL, 
	[CreatedBy] BIGINT NOT NULL,
	[ModifiedAt] datetime2(7) NULL,
	[ModifiedBy] BIGINT NULL
)

GO

CREATE NONCLUSTERED INDEX [IX_Articles_BlogId] ON [dbo].[Articles] ([BlogId])

GO

CREATE NONCLUSTERED INDEX [IX_Articles_Title] ON [dbo].[Articles] ([Title])