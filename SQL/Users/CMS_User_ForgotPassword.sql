CREATE OR ALTER PROCEDURE CMS_User_ForgotPassword
    @Id BIGINT,
    @Email NVARCHAR(200),
    @TemporaryPassword NVARCHAR(6),
    @TemporaryPasswordHash NVARCHAR(500),
    @ActorUserId BIGINT = 0,
    @MailProfileName NVARCHAR(128) = N'CMS Mail Profile'
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @MailBody NVARCHAR(MAX);

    IF NOT EXISTS (
        SELECT 1
        FROM Users
        WHERE Id = @Id
          AND Email = @Email
          AND IsActive = 1
    )
    BEGIN
        SELECT CAST(0 AS BIT) AS IsSuccess;
        RETURN;
    END

    BEGIN TRY
        BEGIN TRAN;

        UPDATE Users
        SET PasswordHash = @TemporaryPasswordHash,
            UpdatedDateTime = GETUTCDATE(),
            UpdatedBy = @ActorUserId
        WHERE Id = @Id;

        SET @MailBody = N'Your temporary password is: ' + @TemporaryPassword +
                        N'. Use it to log in and change your password immediately.';

        EXEC msdb.dbo.sp_send_dbmail
            @profile_name = @MailProfileName,
            @recipients = @Email,
            @subject = N'Complaint Management System - Temporary Password',
            @body = @MailBody;

        COMMIT TRAN;
        SELECT CAST(1 AS BIT) AS IsSuccess;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRAN;
        THROW;
    END CATCH
END;
GO