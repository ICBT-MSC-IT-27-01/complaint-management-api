CREATE OR ALTER PROCEDURE CMS_RolePermission_GetAuditTrail
    @Role NVARCHAR(100) = NULL,
    @Top INT = 100
AS
BEGIN
    SET NOCOUNT ON;

    IF @Top < 1 SET @Top = 100;
    IF @Top > 500 SET @Top = 500;

    SELECT TOP (@Top)
        a.Id,
        ISNULL(r.Role, '') AS Role,
        a.Action,
        ISNULL(u.Name, '') AS ChangedBy,
        a.ChangedDateTime,
        ISNULL(a.Details, '') AS Details
    FROM PermissionAuditTrail a
    LEFT JOIN Roles r ON r.Id = a.RoleId
    LEFT JOIN Users u ON u.Id = a.ChangedByUserId
    WHERE (@Role IS NULL OR @Role = '' OR r.Role = @Role)
    ORDER BY a.ChangedDateTime DESC, a.Id DESC;
END;
GO
