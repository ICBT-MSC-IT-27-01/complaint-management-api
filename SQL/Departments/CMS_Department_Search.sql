CREATE OR ALTER PROCEDURE CMS_Department_Search
    @Q NVARCHAR(150) = NULL,
    @IsActive BIT = NULL,
    @Page INT = 1,
    @PageSize INT = 20
AS
BEGIN
    SET NOCOUNT ON;

    IF @Page < 1 SET @Page = 1;
    IF @PageSize < 1 SET @PageSize = 20;
    IF @PageSize > 100 SET @PageSize = 100;

    ;WITH d AS
    (
        SELECT
            dp.Id,
            dp.DepartmentCode,
            dp.Name,
            dp.Description,
            dp.SortOrder,
            dp.IsActive,
            dp.CreatedDateTime
        FROM Departments dp
        WHERE
            (@Q IS NULL OR @Q = '' OR dp.Name LIKE '%' + @Q + '%' OR dp.DepartmentCode LIKE '%' + @Q + '%')
            AND (@IsActive IS NULL OR dp.IsActive = @IsActive)
    )
    SELECT *
    FROM d
    ORDER BY SortOrder, Name
    OFFSET (@Page - 1) * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY;

    SELECT COUNT(1) AS TotalCount
    FROM Departments dp
    WHERE
        (@Q IS NULL OR @Q = '' OR dp.Name LIKE '%' + @Q + '%' OR dp.DepartmentCode LIKE '%' + @Q + '%')
        AND (@IsActive IS NULL OR dp.IsActive = @IsActive);
END;
GO
