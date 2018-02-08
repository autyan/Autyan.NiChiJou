﻿CREATE TABLE [dbo].[Blog]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
	[BlogName] VARCHAR(200) NOT NULL, 
	[Descriptiong] VARCHAR(200) NULL, 
	[BlogUserId] BIGINT NOT NULL, 
	[CreateAt] DATETIMEOFFSET NOT NULL, 
	[CreateBy] BIGINT NOT NULL,
	[ModifyAt] DATETIMEOFFSET NULL,
	[ModifyBy] BIGINT NULL
	CONSTRAINT [FK_Blog_ToBlogUser] FOREIGN KEY ([BlogUserId]) REFERENCES [BlogUser]([Id])
)

GO

CREATE INDEX [IX_Blog_BlogUser] ON [dbo].[Blog] ([BlogUserId])
