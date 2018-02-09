CREATE TABLE [dbo].[BlogUsers]
(
	[Id] BIGINT NOT NULL PRIMARY KEY, 
	[NickName] VARCHAR(50) NOT NULL, 
	[AvatorUrl] VARCHAR(200) NULL, 
	[LoginUserId] BIGINT NOT NULL, 
	[Gender] TINYINT NOT NULL,
	[CreatedAt] DATETIMEOFFSET NOT NULL, 
	[CreatedBy] BIGINT NOT NULL,
	[ModifiedAt] DATETIMEOFFSET NULL,
	[ModifiedBy] BIGINT NULL
	CONSTRAINT [FK_BlogUsers_ToLoginUser] FOREIGN KEY ([LoginUserId]) REFERENCES [LoginUsers]([Id])
)

GO

CREATE INDEX [IX_BlogUsers_LoginUserId] ON [dbo].[BlogUsers] ([LoginUserId])
