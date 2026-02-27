CREATE OR ALTER PROCEDURE CMS_Client_GetById @Id BIGINT
AS
BEGIN
    SELECT c.Id, c.ClientCode, c.CompanyName, c.PrimaryEmail, c.PrimaryPhone,
           c.Address, c.ClientType, c.AccountManagerId, u.Name AS AccountManagerName,
           c.IsActive, c.CreatedDateTime
    FROM Clients c LEFT JOIN Users u ON u.Id = c.AccountManagerId
    WHERE c.Id = @Id;
END;
GO
