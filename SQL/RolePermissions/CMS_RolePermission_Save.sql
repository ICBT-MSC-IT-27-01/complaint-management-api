CREATE OR ALTER PROCEDURE CMS_RolePermission_Save
    @Role NVARCHAR(100),
    @PermissionsJson NVARCHAR(MAX),
    @ActorUserId BIGINT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @RoleId BIGINT;
    SELECT @RoleId = Id FROM Roles WHERE Role = @Role AND IsActive = 1;

    IF @RoleId IS NULL
    BEGIN
        THROW 50031, 'Role not found.', 1;
    END

    IF @PermissionsJson IS NULL OR LTRIM(RTRIM(@PermissionsJson)) = ''
    BEGIN
        THROW 50032, 'Permissions are required.', 1;
    END

    DECLARE @Input TABLE
    (
        Module NVARCHAR(100) NOT NULL,
        CanRead BIT NOT NULL,
        CanWrite BIT NOT NULL,
        CanDelete BIT NOT NULL
    );

    INSERT INTO @Input (Module, CanRead, CanWrite, CanDelete)
    SELECT
        j.Module,
        ISNULL(j.CanRead, 0),
        ISNULL(j.CanWrite, 0),
        ISNULL(j.CanDelete, 0)
    FROM OPENJSON(@PermissionsJson)
    WITH
    (
        Module NVARCHAR(100) '$.Module',
        CanRead BIT '$.CanRead',
        CanWrite BIT '$.CanWrite',
        CanDelete BIT '$.CanDelete'
    ) j
    WHERE j.Module IS NOT NULL AND LTRIM(RTRIM(j.Module)) <> '';

    IF NOT EXISTS (SELECT 1 FROM @Input)
    BEGIN
        THROW 50033, 'No valid permission rows found.', 1;
    END

    INSERT INTO PermissionModules (Module, DisplayName, IsActive)
    SELECT DISTINCT i.Module, i.Module, 1
    FROM @Input i
    WHERE NOT EXISTS (SELECT 1 FROM PermissionModules pm WHERE pm.Module = i.Module);

    ;WITH src AS
    (
        SELECT
            pm.Id AS ModuleId,
            i.CanRead,
            i.CanWrite,
            i.CanDelete
        FROM @Input i
        INNER JOIN PermissionModules pm ON pm.Module = i.Module
    )
    MERGE RolePermissions AS target
    USING src
       ON target.RoleId = @RoleId AND target.ModuleId = src.ModuleId
    WHEN MATCHED THEN
        UPDATE SET
            CanRead = src.CanRead,
            CanWrite = src.CanWrite,
            CanDelete = src.CanDelete,
            UpdatedDateTime = GETUTCDATE(),
            UpdatedBy = @ActorUserId
    WHEN NOT MATCHED BY TARGET THEN
        INSERT (RoleId, ModuleId, CanRead, CanWrite, CanDelete, UpdatedDateTime, UpdatedBy)
        VALUES (@RoleId, src.ModuleId, src.CanRead, src.CanWrite, src.CanDelete, GETUTCDATE(), @ActorUserId)
    WHEN NOT MATCHED BY SOURCE AND target.RoleId = @RoleId THEN
        DELETE;

    INSERT INTO PermissionAuditTrail (RoleId, Action, Details, ChangedByUserId)
    VALUES (@RoleId, 'SavePermissions', CONCAT('Saved ', (SELECT COUNT(1) FROM @Input), ' permission rows.'), @ActorUserId);
END;
GO
