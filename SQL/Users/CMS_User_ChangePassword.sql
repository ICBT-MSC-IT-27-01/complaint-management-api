CREATE OR ALTER PROCEDURE CMS_User_ChangePassword
    @Id BIGINT, @NewPasswordHash NVARCHAR(500), @ActorUserId BIGINT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Users
    SET PasswordHash = @NewPasswordHash,
        UpdatedDateTime = GETUTCDATE(),
        UpdatedBy = @ActorUserId
    WHERE Id = @Id
      AND IsActive = 1;
END;
GO
