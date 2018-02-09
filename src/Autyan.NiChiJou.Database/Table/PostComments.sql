CREATE TABLE [dbo].[PostComments]
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
	CONSTRAINT [FK_PostComments_ToPost] FOREIGN KEY ([PostId]) REFERENCES [BlogPosts]([Id]), 
	CONSTRAINT [FK_PostComments_ToBlogUser] FOREIGN KEY ([BlogUserId]) REFERENCES [BlogUsers]([Id])
)

GO

CREATE INDEX [IX_PostComments_BlogUserId] ON [dbo].[PostComments] ([BlogUserId])

GO

CREATE INDEX [IX_PostComments_PostId] ON [dbo].[PostComments] ([PostId])
