CREATE OR ALTER PROCEDURE CMS_Complaint_GetById
    @ComplaintId BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT
        c.Id, c.ComplaintNumber, c.Subject, c.Description, c.Priority,
        cs.Name AS Status, c.ComplaintStatusId,
        ch.Name AS Channel, c.ComplaintChannelId,
        cat.Name AS Category, c.ComplaintCategoryId,
        c.SubCategoryId,
        c.ClientId, c.Name AS ClientName, c.ClientEmail, c.ClientMobile,
        c.AssignedToUserId, u.Name AS AssignedToName, c.AssignedDate,
        c.DueDate, c.SlaStatus, c.IsSlaBreached,
        c.IsResolved, c.ResolvedDate, c.ResolutionNotes,
        c.IsClosed, c.ClosedDate,
        c.IsActive, c.CreatedDateTime, c.CreatedBy,
        cu.Name AS CreatedByName, c.UpdatedDateTime
    FROM Complaints c
    LEFT JOIN ComplaintStatuses cs ON cs.Id = c.ComplaintStatusId
    LEFT JOIN ComplaintChannels ch ON ch.Id = c.ComplaintChannelId
    LEFT JOIN Categories cat       ON cat.Id = c.ComplaintCategoryId
    LEFT JOIN Users u              ON u.Id   = c.AssignedToUserId
    LEFT JOIN Users cu             ON cu.Id  = c.CreatedBy
    WHERE c.Id = @ComplaintId;
END;
GO
