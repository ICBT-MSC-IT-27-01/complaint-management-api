CREATE OR ALTER PROCEDURE CMS_Role_GetAll
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        r.Id,
        r.Role,
        r.DisplayName,
        r.IsSystem,
        r.IsActive
    FROM Roles r
    WHERE r.IsActive = 1
    ORDER BY r.DisplayName;
END;
GO
