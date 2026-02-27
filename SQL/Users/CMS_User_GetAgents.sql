CREATE OR ALTER PROCEDURE CMS_User_GetAgents
AS
BEGIN
    SELECT Id, Name, Email, Username, PhoneNumber, Role, IsActive, IsLocked, CreatedDateTime, LastLoginDateTime
    FROM Users WHERE Role IN ('Agent','Supervisor') AND IsActive = 1 ORDER BY Name;
END;
GO
