CREATE OR ALTER PROCEDURE CMS_Department_Delete
    @Id BIGINT,
    @ActorUserId BIGINT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM Departments WHERE Id = @Id)
    BEGIN
        THROW 50004, 'Department not found.', 1;
    END

    UPDATE Departments
    SET
        IsActive = 0,
        UpdatedDateTime = GETUTCDATE(),
        UpdatedBy = @ActorUserId
    WHERE Id = @Id;
END;
GO
