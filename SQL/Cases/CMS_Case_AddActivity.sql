CREATE OR ALTER PROCEDURE CMS_Case_AddActivity
    @CaseId BIGINT, @ActivityType NVARCHAR(50), @Description NVARCHAR(MAX), @ActorUserId BIGINT
AS
BEGIN
    INSERT INTO CaseActivities (CaseId, ActivityType, Description, PerformedByUserId)
    VALUES (@CaseId, @ActivityType, @Description, @ActorUserId);
END;
GO
