CREATE OR ALTER PROCEDURE CMS_Complaint_UpdateStatus
    @ComplaintId BIGINT,
    @StatusId    BIGINT,
    @Note        NVARCHAR(MAX) = NULL,
    @ActorUserId BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @OldStatus NVARCHAR(50), @NewStatus NVARCHAR(50);
    SELECT @OldStatus = cs.Name FROM Complaints c JOIN ComplaintStatuses cs ON cs.Id = c.ComplaintStatusId WHERE c.Id = @ComplaintId;
    SELECT @NewStatus = Name FROM ComplaintStatuses WHERE Id = @StatusId;

    UPDATE Complaints SET
        ComplaintStatusId = @StatusId,
        UpdatedDateTime   = GETUTCDATE(),
        UpdatedBy         = @ActorUserId
    WHERE Id = @ComplaintId;

    INSERT INTO ComplaintHistory (ComplaintId, Action, OldStatus, NewStatus, Note, PerformedByUserId)
    VALUES (@ComplaintId, 'StatusChange', @OldStatus, @NewStatus, @Note, @ActorUserId);
END;
GO
