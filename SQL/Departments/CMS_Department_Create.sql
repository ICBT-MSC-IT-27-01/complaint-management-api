CREATE OR ALTER PROCEDURE CMS_Department_Create
    @Name NVARCHAR(150),
    @Description NVARCHAR(500) = NULL,
    @SortOrder INT = 0,
    @ActorUserId BIGINT
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM Departments WHERE Name = @Name)
    BEGIN
        THROW 50001, 'Department name already exists.', 1;
    END

    INSERT INTO Departments (DepartmentCode, Name, Description, SortOrder, CreatedBy)
    VALUES ('', @Name, @Description, @SortOrder, @ActorUserId);

    DECLARE @Id BIGINT = SCOPE_IDENTITY();
    DECLARE @DepartmentCode NVARCHAR(20) = 'DEP-' + RIGHT('00000' + CAST(@Id AS NVARCHAR(20)), 5);

    UPDATE Departments
    SET DepartmentCode = @DepartmentCode
    WHERE Id = @Id;

    SELECT
        d.Id,
        d.DepartmentCode,
        d.Name,
        d.Description,
        d.SortOrder,
        d.IsActive,
        d.CreatedDateTime
    FROM Departments d
    WHERE d.Id = @Id;
END;
GO
