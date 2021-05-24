CREATE TABLE [dbo].[Users]
(
	[Id] INT NOT NULL PRIMARY KEY,
	[FullName] NVARCHAR(50) NOT NULL, 
    [UserName] NVARCHAR(50) NOT NULL, 
    [Password] NVARCHAR(50) NULL 
)
