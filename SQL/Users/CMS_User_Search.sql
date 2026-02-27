CREATE OR ALTER PROCEDURE CMS_User_Search
    @Keyword  NVARCHAR(200) = NULL,
    @Role     NVARCHAR(50)  = NULL,
    @IsActive BIT           = NULL,
    @Page     INT           = 1,
    @PageSize INT           = 20
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Offset INT = (@Page - 1) * @PageSize;
    SELECT Id, Name, Email, Username, PhoneNumber, Role, IsActive, IsLocked, CreatedDateTime, LastLoginDateTime
    FROM Users
    WHERE (@Keyword  IS NULL OR Name LIKE '%' + @Keyword + '%' OR Email LIKE '%' + @Keyword + '%')
      AND (@Role     IS NULL OR Role = @Role)
      AND (@IsActive IS NULL OR IsActive = @IsActive)
    ORDER BY CreatedDateTime DESC
    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

    SELECT COUNT_BIG(*) FROM Users
    WHERE (@Keyword  IS NULL OR Name LIKE '%' + @Keyword + '%' OR Email LIKE '%' + @Keyword + '%')
      AND (@Role     IS NULL OR Role = @Role)
      AND (@IsActive IS NULL OR IsActive = @IsActive);
END;
GO
