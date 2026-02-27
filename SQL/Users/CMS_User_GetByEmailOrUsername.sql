CREATE OR ALTER PROCEDURE CMS_User_GetByEmailOrUsername @EmailOrUsername NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Name, Email, Username, PhoneNumber, PasswordHash, Role,
           IsActive, IsLocked, CreatedDateTime, LastLoginDateTime
    FROM Users
    WHERE (Email = @EmailOrUsername OR Username = @EmailOrUsername) AND IsActive = 1;
END;
GO
