CREATE OR ALTER PROCEDURE CMS_Department_Typeahead
    @Q NVARCHAR(150)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 20
        d.Id,
        d.DepartmentCode,
        d.Name,
        d.Description,
        d.SortOrder,
        d.IsActive,
        d.CreatedDateTime
    FROM Departments d
    WHERE
        d.IsActive = 1
        AND (@Q IS NULL OR @Q = '' OR d.Name LIKE '%' + @Q + '%' OR d.DepartmentCode LIKE '%' + @Q + '%')
    ORDER BY d.SortOrder, d.Name;
END;
GO
