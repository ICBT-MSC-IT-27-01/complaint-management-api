CREATE OR ALTER PROCEDURE CMS_Case_GetById @CaseId BIGINT
AS
BEGIN
    SELECT ca.Id, ca.CaseNumber, ca.ComplaintId, c.ComplaintNumber,
           ca.Status, ca.AssignedToUserId, u.Name AS AssignedToName,
           ca.Notes, ca.OpenedAt, ca.ClosedAt
    FROM Cases ca
    JOIN Complaints c ON c.Id=ca.ComplaintId
    LEFT JOIN Users u ON u.Id=ca.AssignedToUserId
    WHERE ca.Id=@CaseId;
END;
GO
