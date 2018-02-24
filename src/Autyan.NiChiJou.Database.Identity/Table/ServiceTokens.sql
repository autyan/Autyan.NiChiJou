CREATE TABLE [dbo].[ServiceTokens]
(
	[Id] BIGINT NOT NULL PRIMARY KEY, 
    [ServiceName] NVARCHAR(50) NOT NULL, 
    [AppId] NVARCHAR(50) NOT NULL, 
    [AppKey] NVARCHAR(50) NOT NULL, 
	[IsEnabled] BIT NOT NULL,
    [CreatedAt] DATETIMEOFFSET NOT NULL, 
    [CreatedBy] BIGINT NOT NULL, 
    [ModifiedAt] DATETIMEOFFSET NULL, 
    [ModifiedBy] BIGINT NULL
)
