CREATE OR ALTER PROCEDURE CMS_Complaint_Resolve
    @ComplaintId      BIGINT,
    @ResolutionSummary NVARCHAR(MAX),
    @RootCause        NVARCHAR(MAX) = NULL,
    @FixApplied       NVARCHAR(MAX) = NULL,
    @ActorUserId      BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Complaints SET
        IsResolved        = 1,
        ResolvedDate      = GETUTCDATE(),
        ResolutionNotes   = @ResolutionSummary,
        ComplaintStatusId = 6, -- Resolved
        UpdatedDateTime   = GETUTCDATE(),
        UpdatedBy         = @ActorUserId
    WHERE Id = @ComplaintId;

    UPDATE Cases SET Status = 'Resolved', ClosedAt = GETUTCDATE()
    WHERE ComplaintId = @ComplaintId AND Status != 'Closed';

    INSERT INTO ComplaintHistory (ComplaintId, Action, NewStatus, Note, PerformedByUserId)
    VALUES (@ComplaintId, 'Resolve', 'Resolved', @ResolutionSummary, @ActorUserId);
END;
GO
