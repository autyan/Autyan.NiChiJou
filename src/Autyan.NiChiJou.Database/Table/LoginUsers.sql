CREATE TABLE [dbo].[LoginUsers]
(
	[Id] BIGINT NOT NULL PRIMARY KEY, 
	[LoginName] NVARCHAR(50) NOT NULL, 
	[Email] NVARCHAR(50) NULL, 
	[EmailConfirmed] BIT NOT NULL, 
	[PhoneNumber] NVARCHAR(50) NULL, 
	[PhoneNumberConfirmed] BIT NOT NULL, 
	[PasswordHash] NVARCHAR(200) NOT NULL, 
	[SecuritySalt] NVARCHAR(200) NOT NULL, 
	[CreatedAt] DATETIMEOFFSET NOT NULL, 
	[CreatedBy] BIGINT NOT NULL,
	[ModifiedAt] DATETIMEOFFSET NULL,
	[ModifiedBy] BIGINT NULL
)

GO

CREATE UNIQUE INDEX [IX_LoginUsers_LoginName] ON [dbo].[LoginUsers] ([LoginName])
