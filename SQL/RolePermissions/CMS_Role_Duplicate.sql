CREATE OR ALTER PROCEDURE CMS_Role_Duplicate
    @Role NVARCHAR(100),
    @NewRole NVARCHAR(100),
    @ActorUserId BIGINT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @RoleId BIGINT;
    SELECT @RoleId = Id FROM Roles WHERE Role = @Role AND IsActive = 1;

    IF @RoleId IS NULL
    BEGIN
        THROW 50034, 'Source role not found.', 1;
    END

    IF @NewRole IS NULL OR LTRIM(RTRIM(@NewRole)) = ''
    BEGIN
        THROW 50035, 'New role is required.', 1;
    END

    IF EXISTS (SELECT 1 FROM Roles WHERE Role = @NewRole)
    BEGIN
        THROW 50036, 'New role already exists.', 1;
    END

    INSERT INTO Roles (Role, DisplayName, IsSystem, IsActive, CreatedBy)
    VALUES (@NewRole, @NewRole, 0, 1, @ActorUserId);

    DECLARE @NewRoleId BIGINT = SCOPE_IDENTITY();

    INSERT INTO RolePermissions (RoleId, ModuleId, CanRead, CanWrite, CanDelete, UpdatedBy)
    SELECT
        @NewRoleId,
        rp.ModuleId,
        rp.CanRead,
        rp.CanWrite,
        rp.CanDelete,
        @ActorUserId
    FROM RolePermissions rp
    WHERE rp.RoleId = @RoleId;

    INSERT INTO PermissionAuditTrail (RoleId, Action, Details, ChangedByUserId)
    VALUES (@NewRoleId, 'DuplicateRole', CONCAT('Duplicated from role ', @Role, '.'), @ActorUserId);
END;
GO
