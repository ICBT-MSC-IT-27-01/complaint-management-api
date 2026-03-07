CREATE OR ALTER PROCEDURE CMS_SlaPolicy_Create
    @CategoryId BIGINT,
    @Priority NVARCHAR(20),
    @ResponseTimeHours INT,
    @ResolutionTimeHours INT,
    @EscalationThresholdPct INT = 80,
    @ActorUserId BIGINT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM Categories WHERE Id = @CategoryId AND IsActive = 1)
    BEGIN
        THROW 50001, 'Category does not exist or is inactive.', 1;
    END

    IF EXISTS (
        SELECT 1
        FROM SLAPolicies
        WHERE CategoryId = @CategoryId
          AND Priority = @Priority
    )
    BEGIN
        THROW 50002, 'SLA policy already exists for this category and priority.', 1;
    END

    INSERT INTO SLAPolicies
    (
        CategoryId,
        Priority,
        ResponseTimeHours,
        ResolutionTimeHours,
        EscalationThresholdPct,
        IsActive,
        CreatedBy
    )
    VALUES
    (
        @CategoryId,
        @Priority,
        @ResponseTimeHours,
        @ResolutionTimeHours,
        @EscalationThresholdPct,
        1,
        @ActorUserId
    );

    DECLARE @Id BIGINT = SCOPE_IDENTITY();

    SELECT s.Id, s.CategoryId, c.Name AS CategoryName, s.Priority,
           s.ResponseTimeHours, s.ResolutionTimeHours, s.EscalationThresholdPct, s.IsActive
    FROM SLAPolicies s
    JOIN Categories c ON c.Id = s.CategoryId
    WHERE s.Id = @Id;
END;
GO
