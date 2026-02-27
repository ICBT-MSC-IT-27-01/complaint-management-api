CREATE OR ALTER PROCEDURE CMS_User_Delete @Id BIGINT, @ActorUserId BIGINT
AS
BEGIN
    UPDATE Users SET IsActive=0, UpdatedDateTime=GETUTCDATE(), UpdatedBy=@ActorUserId WHERE Id=@Id;
END;
GO
