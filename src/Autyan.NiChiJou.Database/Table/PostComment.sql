CREATE TABLE [dbo].[PostComment]
(
	[Id] BIGINT NOT NULL PRIMARY KEY NONCLUSTERED IDENTITY, 
	[Content] NVARCHAR(2000) NOT NULL, 
	[BlogUserId] BIGINT NOT NULL, 
	[PostId] BIGINT NOT NULL, 
	[ToComment] BIGINT NULL, 
	[CreateAt] DATETIMEOFFSET NOT NULL, 
	[CreateBy] BIGINT NOT NULL,
	[ModifyAt] DATETIMEOFFSET NULL,
	[ModifyBy] BIGINT NULL
	CONSTRAINT [FK_PostComment_ToPost] FOREIGN KEY ([PostId]) REFERENCES [BlogPost]([Id]), 
	CONSTRAINT [FK_PostComment_ToBlogUser] FOREIGN KEY ([BlogUserId]) REFERENCES [BlogUser]([Id])
)

GO

CREATE CLUSTERED INDEX [IX_PostComment_BlogUserId_PostId] ON [dbo].[PostComment] ([BlogUserId],[PostId])
