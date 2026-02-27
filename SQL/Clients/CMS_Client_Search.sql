CREATE OR ALTER PROCEDURE CMS_Client_Search
    @Q NVARCHAR(300)=NULL, @ClientType NVARCHAR(50)=NULL, @IsActive BIT=NULL,
    @Page INT=1, @PageSize INT=20
AS
BEGIN
    DECLARE @Offset INT = (@Page-1)*@PageSize;
    SELECT c.Id, c.ClientCode, c.CompanyName, c.PrimaryEmail, c.PrimaryPhone,
           c.Address, c.ClientType, c.AccountManagerId, u.Name AS AccountManagerName,
           c.IsActive, c.CreatedDateTime
    FROM Clients c LEFT JOIN Users u ON u.Id = c.AccountManagerId
    WHERE (@Q IS NULL OR c.CompanyName LIKE '%'+@Q+'%' OR c.PrimaryEmail LIKE '%'+@Q+'%' OR c.ClientCode LIKE '%'+@Q+'%')
      AND (@ClientType IS NULL OR c.ClientType=@ClientType)
      AND (@IsActive IS NULL OR c.IsActive=@IsActive)
    ORDER BY c.CompanyName
    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
    SELECT COUNT_BIG(*) FROM Clients WHERE IsActive=ISNULL(@IsActive,IsActive);
END;
GO
