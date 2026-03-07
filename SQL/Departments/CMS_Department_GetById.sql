CREATE OR ALTER PROCEDURE CMS_Department_GetById
    @Id BIGINT
AS
BEGIN
    SET NOCOUNT ON;

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
