CREATE TABLE [dbo].[ActivityLogs]
(
	[Id] BIGINT NOT NULL PRIMARY KEY, 
	[OperateUserId] BIGINT NOT NULL, 
	[ActivityType] TINYINT NOT NULL DEFAULT 5, 
	[Content] NVARCHAR(1000) NOT NULL, 
	[OperateIpAddress] NVARCHAR(50) NULL DEFAULT '*', 
	[ClientType] TINYINT NOT NULL, 
	[CreatedAt] DATETIMEOFFSET NOT NULL, 
	[CreatedBy] BIGINT NOT NULL
	CONSTRAINT [FK_ActivityLogs_ToLoginUser] FOREIGN KEY ([OperateUserId]) REFERENCES [BlogUsers]([Id])
)

GO

CREATE NONCLUSTERED INDEX [IX_ActivityLogs_OperateUserId_ActivityType_ClientType] ON [dbo].[ActivityLogs] ([OperateUserId], [ActivityType], [ClientType])

GO

CREATE NONCLUSTERED INDEX [IX_ActivityLogs_OperateUserId_ClientType_ActivityType] ON [dbo].[ActivityLogs] ([OperateUserId], [ClientType], [ActivityType])
