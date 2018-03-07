CREATE TABLE [dbo].[ServiceTokens]
(
	[Id] BIGINT NOT NULL PRIMARY KEY, 
	[ServiceName] NVARCHAR(50) NOT NULL, 
	[AppId] NVARCHAR(50) NOT NULL, 
	[ApiKey] NVARCHAR(50) NOT NULL, 
	[IsEnabled] BIT NOT NULL,
	[CreatedAt] datetime2(7) NOT NULL, 
	[CreatedBy] BIGINT NOT NULL, 
	[ModifiedAt] datetime2(7) NULL, 
	[ModifiedBy] BIGINT NULL
)
