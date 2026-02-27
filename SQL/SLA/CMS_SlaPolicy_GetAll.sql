CREATE OR ALTER PROCEDURE CMS_SlaPolicy_GetAll
AS
BEGIN
    SELECT s.Id, s.CategoryId, c.Name AS CategoryName, s.Priority,
           s.ResponseTimeHours, s.ResolutionTimeHours, s.EscalationThresholdPct, s.IsActive
    FROM SLAPolicies s JOIN Categories c ON c.Id=s.CategoryId
    ORDER BY c.Name, s.Priority;
END;
GO
