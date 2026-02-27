CREATE OR ALTER PROCEDURE CMS_Case_GetActivities @CaseId BIGINT
AS
BEGIN
    SELECT a.Id, a.CaseId, a.ActivityType, a.Description,
           a.PerformedByUserId, u.Name AS PerformedByName, a.CreatedDateTime
    FROM CaseActivities a JOIN Users u ON u.Id=a.PerformedByUserId
    WHERE a.CaseId=@CaseId ORDER BY a.CreatedDateTime DESC;
END;
GO
