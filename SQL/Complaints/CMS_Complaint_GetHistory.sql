CREATE OR ALTER PROCEDURE CMS_Complaint_GetHistory
    @ComplaintId BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT h.Id, h.Action, h.OldStatus, h.NewStatus, h.Note,
           u.Name AS PerformedByName, h.CreatedDateTime
    FROM ComplaintHistory h
    LEFT JOIN Users u ON u.Id = h.PerformedByUserId
    WHERE h.ComplaintId = @ComplaintId
    ORDER BY h.CreatedDateTime DESC;
END;
GO
