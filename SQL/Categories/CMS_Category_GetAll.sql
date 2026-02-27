CREATE OR ALTER PROCEDURE CMS_Category_GetAll
AS
BEGIN
    SELECT c.Id, c.Name, c.ParentCategoryId, p.Name AS ParentName, c.SortOrder, c.IsActive
    FROM Categories c LEFT JOIN Categories p ON p.Id = c.ParentCategoryId
    ORDER BY c.ParentCategoryId, c.SortOrder, c.Name;
END;
GO
