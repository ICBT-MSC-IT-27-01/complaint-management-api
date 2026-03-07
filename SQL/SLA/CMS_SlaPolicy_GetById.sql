CREATE OR ALTER PROCEDURE CMS_SlaPolicy_GetById
    @Id BIGINT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT s.Id, s.CategoryId, c.Name AS CategoryName, s.Priority,
           s.ResponseTimeHours, s.ResolutionTimeHours, s.EscalationThresholdPct, s.IsActive
    FROM SLAPolicies s
    JOIN Categories c ON c.Id = s.CategoryId
    WHERE s.Id = @Id;
END;
GO
