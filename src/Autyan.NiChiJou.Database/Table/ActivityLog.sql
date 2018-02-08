CREATE TABLE [dbo].[ActivityLog]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
	[OperateUserId] BIGINT NOT NULL, 
	[ActivityType] TINYINT NOT NULL DEFAULT 5, 
	[Content] NVARCHAR(1000) NOT NULL, 
	[OperateIpAddress] NVARCHAR(50) NULL DEFAULT '*', 
	[ClientType] TINYINT NOT NULL, 
	[CreateAt] DATETIMEOFFSET NOT NULL, 
	[CreateBy] BIGINT NOT NULL
	CONSTRAINT [FK_ActivityLog_ToLoginUser] FOREIGN KEY ([OperateUserId]) REFERENCES [LoginUser]([Id])
)

GO

CREATE NONCLUSTERED INDEX [IX_ActivityLog_OperateUserId_ActivityType_ClientType] ON [dbo].[ActivityLog] ([OperateUserId], [ActivityType], [ClientType])

GO

CREATE NONCLUSTERED INDEX [IX_ActivityLog_OperateUserId_ClientType_ActivityType] ON [dbo].[ActivityLog] ([OperateUserId], [ClientType], [ActivityType])
