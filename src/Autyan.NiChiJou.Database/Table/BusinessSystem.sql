CREATE TABLE [dbo].[BusinessSystem]
(
	[Id] BIGINT NOT NULL PRIMARY KEY, 
    [Code] NVARCHAR(50) NOT NULL, 
    [MainDomain] NVARCHAR(200) NOT NULL, 
    [CreatedAt] DATETIMEOFFSET NOT NULL, 
    [CreatedBy] BIGINT NOT NULL, 
    [ModifiedAt] DATETIMEOFFSET NULL, 
    [ModifiedBy] BIGINT NULL
)

GO

CREATE UNIQUE INDEX [IX_BusinessSystem_Code] ON [dbo].[BusinessSystem] ([Code])
