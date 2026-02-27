CREATE OR ALTER PROCEDURE CMS_Complaint_Escalate
    @ComplaintId      BIGINT,
    @Reason           NVARCHAR(MAX),
    @EscalatedToUserId BIGINT,
    @EscalationType   NVARCHAR(30) = 'Manual',
    @ActorUserId      BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Complaints SET
        ComplaintStatusId = 5, -- Escalated
        UpdatedDateTime   = GETUTCDATE(),
        UpdatedBy         = @ActorUserId
    WHERE Id = @ComplaintId;

    INSERT INTO Escalations (ComplaintId, EscalatedByUserId, EscalatedToUserId, Reason, EscalationType)
    VALUES (@ComplaintId, @ActorUserId, @EscalatedToUserId, @Reason, @EscalationType);

    INSERT INTO ComplaintHistory (ComplaintId, Action, NewStatus, Note, PerformedByUserId)
    VALUES (@ComplaintId, 'Escalate', 'Escalated', @Reason, @ActorUserId);
END;
GO
