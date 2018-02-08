CREATE TABLE [dbo].[LoginUser]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY(0, 1), 
	[LoginName] NVARCHAR(50) NOT NULL, 
	[Email] NVARCHAR(50) NULL, 
	[EmailConfirmed] BIT NOT NULL, 
	[PhoneNumber] NVARCHAR(50) NULL, 
	[PhoneNumberConfirmed] BIT NOT NULL, 
	[PasswordHash] NVARCHAR(200) NOT NULL, 
	[SecuritySalt] NVARCHAR(200) NOT NULL, 
	[CreateAt] DATETIMEOFFSET NOT NULL, 
	[CreateBy] BIGINT NOT NULL,
	[ModifyAt] DATETIMEOFFSET NULL,
	[ModifyBy] BIGINT NULL
)
