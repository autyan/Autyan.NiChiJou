CREATE TABLE [dbo].[Blogs]
(
	[Id] BIGINT NOT NULL PRIMARY KEY, 
	[BlogName] NVARCHAR(200) NOT NULL, 
	[Description] NVARCHAR(200) NULL, 
	[BlogUserId] BIGINT NOT NULL, 
	[CreatedAt] datetime2(7) NOT NULL, 
	[CreatedBy] BIGINT NOT NULL,
	[ModifiedAt] datetime2(7) NULL,
	[ModifiedBy] BIGINT NULL
)

GO

CREATE NONCLUSTERED INDEX [IX_Blogs_BlogUser] ON [dbo].[Blogs] ([BlogUserId])
