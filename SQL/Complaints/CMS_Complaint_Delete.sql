CREATE OR ALTER PROCEDURE CMS_Complaint_Delete
    @ComplaintId BIGINT,
    @ActorUserId BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Complaints SET IsActive = 0, UpdatedDateTime = GETUTCDATE(), UpdatedBy = @ActorUserId
    WHERE Id = @ComplaintId;
END;
GO
