CREATE OR ALTER PROCEDURE CMS_RolePermission_GetByRole
    @Role NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @RoleId BIGINT;
    SELECT @RoleId = Id FROM Roles WHERE Role = @Role AND IsActive = 1;

    IF @RoleId IS NULL
    BEGIN
        THROW 50030, 'Role not found.', 1;
    END

    SELECT
        m.Module,
        ISNULL(rp.CanRead, 0) AS CanRead,
        ISNULL(rp.CanWrite, 0) AS CanWrite,
        ISNULL(rp.CanDelete, 0) AS CanDelete
    FROM PermissionModules m
    LEFT JOIN RolePermissions rp
        ON rp.ModuleId = m.Id
       AND rp.RoleId = @RoleId
    WHERE m.IsActive = 1
    ORDER BY m.DisplayName;
END;
GO
