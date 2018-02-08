CREATE TABLE [dbo].[BlogUser]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
	[NickName] VARCHAR(50) NOT NULL, 
	[AvatorUrl] VARCHAR(200) NULL, 
	[LoginUserId] BIGINT NOT NULL, 
	[Gender] TINYINT NOT NULL,
	[CreateAt] DATETIMEOFFSET NOT NULL, 
	[CreateBy] BIGINT NOT NULL,
	[ModifyAt] DATETIMEOFFSET NULL,
	[ModifyBy] BIGINT NULL
	CONSTRAINT [FK_BlogUser_ToLoginUser] FOREIGN KEY ([LoginUserId]) REFERENCES [LoginUser]([Id])
)

GO

CREATE INDEX [IX_BlogUser_LoginUserId] ON [dbo].[BlogUser] ([LoginUserId])
