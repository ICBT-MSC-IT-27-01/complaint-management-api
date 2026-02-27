CREATE OR ALTER PROCEDURE CMS_Client_Create
    @CompanyName NVARCHAR(300), @PrimaryEmail NVARCHAR(200),
    @PrimaryPhone NVARCHAR(20)=NULL, @Address NVARCHAR(500)=NULL,
    @ClientType NVARCHAR(50)='Standard', @AccountManagerId BIGINT=NULL, @ActorUserId BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Code NVARCHAR(50) = 'CLT-' + CAST(NEXT VALUE FOR IF EXISTS (SELECT 1 FROM sys.sequences WHERE name='seq_client') 1 ELSE 1 AS NVARCHAR);
    -- Simple code generation
    INSERT INTO Clients (ClientCode, CompanyName, PrimaryEmail, PrimaryPhone, Address, ClientType, AccountManagerId, CreatedBy)
    VALUES ('CLT-' + FORMAT(GETDATE(),'yyyyMMddHHmmss'), @CompanyName, @PrimaryEmail, @PrimaryPhone, @Address, @ClientType, @AccountManagerId, @ActorUserId);
    DECLARE @Id BIGINT = SCOPE_IDENTITY();
    EXEC CMS_Client_GetById @Id;
END;
GO
