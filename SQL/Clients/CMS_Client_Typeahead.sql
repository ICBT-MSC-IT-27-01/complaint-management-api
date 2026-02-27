CREATE OR ALTER PROCEDURE CMS_Client_Typeahead @Q NVARCHAR(200)
AS
BEGIN
    SELECT TOP 10 Id, ClientCode, CompanyName AS Name, PrimaryEmail, ClientType, IsActive,
           NULL AS PrimaryPhone, NULL AS Address, NULL AS AccountManagerId,
           '' AS AccountManagerName, CreatedDateTime
    FROM Clients WHERE IsActive=1 AND (CompanyName LIKE '%'+@Q+'%' OR ClientCode LIKE '%'+@Q+'%')
    ORDER BY CompanyName;
END;
GO
