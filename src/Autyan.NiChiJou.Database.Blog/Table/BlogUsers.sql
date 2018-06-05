CREATE TABLE [dbo].[BlogUsers]
(
	[Id] BIGINT NOT NULL PRIMARY KEY, 
	[NickName] NVARCHAR(50) NOT NULL, 
	[AvatorUrl] NVARCHAR(200) NULL, 
	[MemberCode] NVARCHAR(50) NOT NULL, 
	[Gender] TINYINT NULL,
	[CreatedAt] DATETIME2 NOT NULL, 
	[CreatedBy] BIGINT NOT NULL,
	[ModifiedAt] DATETIME2 NULL,
	[ModifiedBy] BIGINT NULL
)

GO

CREATE NONCLUSTERED INDEX [IX_BlogUsers_MemberCode] ON [dbo].[BlogUsers] ([MemberCode])
