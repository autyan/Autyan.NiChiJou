﻿CREATE TABLE [dbo].[ArticleComments]
(
	[Id] BIGINT NOT NULL PRIMARY KEY, 
	[Content] NVARCHAR(4000) NOT NULL, 
	[CommentedBy] NVARCHAR(4000) NULL, 
	[PostId] BIGINT NOT NULL, 
	[ToComment] BIGINT NULL, 
	[CreatedAt] DATETIMEOFFSET NOT NULL, 
	[CreatedBy] BIGINT NOT NULL,
	[ModifiedAt] DATETIMEOFFSET NULL,
	[ModifiedBy] BIGINT NULL
)

GO

CREATE NONCLUSTERED INDEX [IX_ArticleComments_BlogUserId] ON [dbo].[ArticleComments] ([BlogUserId])

GO

CREATE NONCLUSTERED INDEX [IX_ArticleComments_PostId] ON [dbo].[ArticleComments] ([PostId])
