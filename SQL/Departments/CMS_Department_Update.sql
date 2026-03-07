CREATE OR ALTER PROCEDURE CMS_Department_Update
    @Id BIGINT,
    @Name NVARCHAR(150),
    @Description NVARCHAR(500) = NULL,
    @SortOrder INT = 0,
    @IsActive BIT,
    @ActorUserId BIGINT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM Departments WHERE Id = @Id)
    BEGIN
        THROW 50002, 'Department not found.', 1;
    END

    IF EXISTS (SELECT 1 FROM Departments WHERE Name = @Name AND Id <> @Id)
    BEGIN
        THROW 50003, 'Department name already exists.', 1;
    END

    UPDATE Departments
    SET
        Name = @Name,
        Description = @Description,
        SortOrder = @SortOrder,
        IsActive = @IsActive,
        UpdatedDateTime = GETUTCDATE(),
        UpdatedBy = @ActorUserId
    WHERE Id = @Id;
END;
GO
