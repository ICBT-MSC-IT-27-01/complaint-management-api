CREATE OR ALTER PROCEDURE CMS_SlaPolicy_Update
    @Id BIGINT,
    @CategoryId BIGINT,
    @Priority NVARCHAR(20),
    @ResponseTimeHours INT,
    @ResolutionTimeHours INT,
    @EscalationThresholdPct INT = 80,
    @ActorUserId BIGINT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM SLAPolicies WHERE Id = @Id)
    BEGIN
        THROW 50001, 'SLA policy does not exist.', 1;
    END

    IF NOT EXISTS (SELECT 1 FROM Categories WHERE Id = @CategoryId AND IsActive = 1)
    BEGIN
        THROW 50002, 'Category does not exist or is inactive.', 1;
    END

    IF EXISTS (
        SELECT 1
        FROM SLAPolicies
        WHERE CategoryId = @CategoryId
          AND Priority = @Priority
          AND Id <> @Id
    )
    BEGIN
        THROW 50003, 'SLA policy already exists for this category and priority.', 1;
    END

    UPDATE SLAPolicies
    SET CategoryId = @CategoryId,
        Priority = @Priority,
        ResponseTimeHours = @ResponseTimeHours,
        ResolutionTimeHours = @ResolutionTimeHours,
        EscalationThresholdPct = @EscalationThresholdPct
    WHERE Id = @Id;
END;
GO
