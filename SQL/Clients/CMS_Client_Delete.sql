CREATE OR ALTER PROCEDURE CMS_Client_Delete @Id BIGINT, @ActorUserId BIGINT
AS
BEGIN
    UPDATE Clients SET IsActive=0, UpdatedDateTime=GETUTCDATE(), UpdatedBy=@ActorUserId WHERE Id=@Id;
END;
GO
