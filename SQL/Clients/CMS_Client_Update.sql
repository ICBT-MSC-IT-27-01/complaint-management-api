CREATE OR ALTER PROCEDURE CMS_Client_Update
    @Id BIGINT, @CompanyName NVARCHAR(300), @PrimaryEmail NVARCHAR(200),
    @PrimaryPhone NVARCHAR(20)=NULL, @Address NVARCHAR(500)=NULL,
    @ClientType NVARCHAR(50), @AccountManagerId BIGINT=NULL, @ActorUserId BIGINT
AS
BEGIN
    UPDATE Clients SET CompanyName=@CompanyName, PrimaryEmail=@PrimaryEmail,
        PrimaryPhone=@PrimaryPhone, Address=@Address, ClientType=@ClientType,
        AccountManagerId=@AccountManagerId, UpdatedDateTime=GETUTCDATE(), UpdatedBy=@ActorUserId
    WHERE Id=@Id;
END;
GO
