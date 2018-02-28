CREATE TABLE [dbo].[BlogUsers]
(
	[Id] BIGINT NOT NULL PRIMARY KEY, 
	[NickName] NVARCHAR(50) NOT NULL, 
	[AvatorUrl] NVARCHAR(200) NULL, 
	[UserMemberCode] NVARCHAR(50) NOT NULL, 
	[Gender] TINYINT NOT NULL,
	[CreatedAt] DATETIMEOFFSET NOT NULL, 
	[CreatedBy] BIGINT NOT NULL,
	[ModifiedAt] DATETIMEOFFSET NULL,
	[ModifiedBy] BIGINT NULL
)

GO

CREATE NONCLUSTERED INDEX [IX_BlogUsers_LoginUserId] ON [dbo].[BlogUsers] ([UserMemberCode])
