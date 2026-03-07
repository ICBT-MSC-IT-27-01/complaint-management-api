CREATE OR ALTER PROCEDURE CMS_Report_GetDashboard
    @ActorUserId BIGINT,
    @ActorEmail NVARCHAR(200) = NULL,
    @Role NVARCHAR(50),
    @Period NVARCHAR(20) = '30d',
    @From DATETIME2 = NULL,
    @To DATETIME2 = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @RoleNorm NVARCHAR(50) = UPPER(ISNULL(@Role, ''));
    DECLARE @FromDate DATETIME2 = ISNULL(@From,
        CASE
            WHEN @Period='year' THEN DATEADD(YEAR,-1,GETUTCDATE())
            WHEN @Period='quarter' THEN DATEADD(MONTH,-3,GETUTCDATE())
            ELSE DATEADD(DAY,-30,GETUTCDATE())
        END);
    DECLARE @ToDate DATETIME2 = ISNULL(@To, GETUTCDATE());

    ;WITH FilteredComplaints AS
    (
        SELECT c.*
        FROM Complaints c
        WHERE c.IsActive=1
          AND c.CreatedDateTime >= @FromDate
          AND c.CreatedDateTime <= @ToDate
          AND
          (
              @RoleNorm IN ('ADMIN','SUPERVISOR')
              OR (@RoleNorm = 'AGENT' AND c.AssignedToUserId = @ActorUserId)
              OR (@RoleNorm = 'CLIENT' AND ((@ActorEmail IS NOT NULL AND c.ClientEmail = @ActorEmail) OR c.CreatedBy = @ActorUserId))
              OR (@RoleNorm NOT IN ('ADMIN','SUPERVISOR','AGENT','CLIENT') AND c.CreatedBy = @ActorUserId)
          )
    )

    -- KPI row
    SELECT
        COUNT(*) AS TotalComplaints,
        SUM(CASE WHEN ComplaintStatusId NOT IN (6,7) THEN 1 ELSE 0 END) AS OpenComplaints,
        SUM(CASE WHEN IsResolved=1 AND CAST(ResolvedDate AS DATE)=CAST(GETUTCDATE() AS DATE) THEN 1 ELSE 0 END) AS ResolvedToday,
        SUM(CASE WHEN SlaStatus='Breached' THEN 1 ELSE 0 END) AS SlaBreached,
        SUM(CASE WHEN SlaStatus='AtRisk' THEN 1 ELSE 0 END) AS SlaAtRisk,
        SUM(
            CASE
                WHEN ComplaintStatusId NOT IN (6,7)
                     AND
                     (
                         (@RoleNorm IN ('ADMIN','SUPERVISOR'))
                         OR (@RoleNorm = 'AGENT' AND AssignedToUserId=@ActorUserId)
                         OR (@RoleNorm = 'CLIENT' AND ((@ActorEmail IS NOT NULL AND ClientEmail=@ActorEmail) OR CreatedBy=@ActorUserId))
                         OR (@RoleNorm NOT IN ('ADMIN','SUPERVISOR','AGENT','CLIENT') AND CreatedBy=@ActorUserId)
                     )
                THEN 1
                ELSE 0
            END
        ) AS MyOpenComplaints,
        AVG(CASE WHEN IsResolved=1 AND ResolvedDate IS NOT NULL
            THEN DATEDIFF(MINUTE, CreatedDateTime, ResolvedDate) / 60.0 END) AS AvgResolutionHours
    FROM FilteredComplaints;

    -- By status
    SELECT cs.Name AS Status, COUNT(c.Id) AS Count
    FROM ComplaintStatuses cs
    LEFT JOIN FilteredComplaints c ON c.ComplaintStatusId=cs.Id
    GROUP BY cs.Id, cs.Name ORDER BY cs.Id;

    -- By priority
    SELECT Priority, COUNT(*) AS Count FROM FilteredComplaints
    GROUP BY Priority ORDER BY Priority;
END;
GO
