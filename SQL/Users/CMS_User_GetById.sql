CREATE OR ALTER PROCEDURE CMS_User_GetById @Id BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Name, Email, Username, PhoneNumber, Role,
           IsActive, IsLocked, CreatedDateTime, LastLoginDateTime
    FROM Users WHERE Id = @Id;
END;
GO
