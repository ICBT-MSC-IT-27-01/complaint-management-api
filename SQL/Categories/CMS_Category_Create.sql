CREATE OR ALTER PROCEDURE CMS_Category_Create
    @Name NVARCHAR(150), @ParentCategoryId BIGINT=NULL, @SortOrder INT=0, @ActorUserId BIGINT
AS
BEGIN
    INSERT INTO Categories (Name, ParentCategoryId, SortOrder, CreatedBy)
    VALUES (@Name, @ParentCategoryId, @SortOrder, @ActorUserId);
    DECLARE @Id BIGINT = SCOPE_IDENTITY();
    SELECT c.Id, c.Name, c.ParentCategoryId, p.Name AS ParentName, c.SortOrder, c.IsActive
    FROM Categories c LEFT JOIN Categories p ON p.Id=c.ParentCategoryId WHERE c.Id=@Id;
END;
GO
