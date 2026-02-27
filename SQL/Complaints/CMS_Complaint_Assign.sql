CREATE OR ALTER PROCEDURE CMS_Complaint_Assign
    @ComplaintId      BIGINT,
    @AssignedToUserId BIGINT,
    @DueDate          DATETIME2    = NULL,
    @Note             NVARCHAR(MAX)= NULL,
    @ActorUserId      BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Complaints SET
        AssignedToUserId = @AssignedToUserId,
        AssignedDate     = GETUTCDATE(),
        DueDate          = ISNULL(@DueDate, DueDate),
        ComplaintStatusId= 2, -- Assigned
        UpdatedDateTime  = GETUTCDATE(),
        UpdatedBy        = @ActorUserId
    WHERE Id = @ComplaintId;

    INSERT INTO ComplaintHistory (ComplaintId, Action, NewStatus, Note, PerformedByUserId)
    VALUES (@ComplaintId, 'Assign', 'Assigned', @Note, @ActorUserId);
END;
GO
