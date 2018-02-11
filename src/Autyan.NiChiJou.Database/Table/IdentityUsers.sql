CREATE TABLE [dbo].[IdentityUsers]
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


CREATE UNIQUE NONCLUSTERED INDEX [IX_IdentityUsers_LoginName] ON [dbo].[IdentityUsers] ([LoginName])

GO

CREATE NONCLUSTERED INDEX [IX_IdentityUsers_Email] ON [dbo].[IdentityUsers] ([Email])

GO

CREATE NONCLUSTERED INDEX [IX_IdentityUsers_PhoneNumber] ON [dbo].[IdentityUsers] ([PhoneNumber])