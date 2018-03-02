CREATE TABLE [dbo].[Articles]
(
	[Id] BIGINT NOT NULL PRIMARY KEY, 
	[Title] NVARCHAR(100) NOT NULL, 
	[Extract] NVARCHAR(200) NULL, 
	[BlogId] BIGINT NOT NULL, 
	[Reads] INT NOT NULL,
	[Comments] INT NOT NULL,
	[LastReadAt] DATETIMEOFFSET NULL,
	[CreatedAt] DATETIMEOFFSET NOT NULL, 
	[CreatedBy] BIGINT NOT NULL,
	[ModifiedAt] DATETIMEOFFSET NULL,
	[ModifiedBy] BIGINT NULL
)

GO

CREATE NONCLUSTERED INDEX [IX_Articles_BlogId] ON [dbo].[Articles] ([BlogId])

GO

CREATE NONCLUSTERED INDEX [IX_Articles_Title] ON [dbo].[Articles] ([Title])