CREATE OR ALTER PROCEDURE CMS_Complaint_Close
    @ComplaintId BIGINT,
    @ActorUserId BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Complaints SET
        IsClosed          = 1,
        ClosedDate        = GETUTCDATE(),
        ComplaintStatusId = 7, -- Closed
        UpdatedDateTime   = GETUTCDATE(),
        UpdatedBy         = @ActorUserId
    WHERE Id = @ComplaintId;

    UPDATE Cases SET Status = 'Closed', ClosedAt = GETUTCDATE()
    WHERE ComplaintId = @ComplaintId;

    INSERT INTO ComplaintHistory (ComplaintId, Action, NewStatus, PerformedByUserId)
    VALUES (@ComplaintId, 'Close', 'Closed', @ActorUserId);
END;
GO
