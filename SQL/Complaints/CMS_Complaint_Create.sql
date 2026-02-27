-- CMS_Complaint_Create
-- Creates a new complaint and automatically creates the linked Case
CREATE OR ALTER PROCEDURE CMS_Complaint_Create
    @ClientId            BIGINT         = NULL,
    @ClientName          NVARCHAR(300)  = NULL,
    @ClientEmail         NVARCHAR(200)  = NULL,
    @ClientMobile        NVARCHAR(20)   = NULL,
    @ComplaintChannelId  BIGINT,
    @ComplaintCategoryId BIGINT,
    @SubCategoryId       BIGINT         = NULL,
    @Subject             NVARCHAR(300),
    @Description         NVARCHAR(MAX),
    @Priority            NVARCHAR(20)   = 'Medium',
    @ActorUserId         BIGINT
AS
BEGIN
    SET NOCOUNT ON;

    -- Calculate SLA due date
    DECLARE @DueDate DATETIME2 = NULL;
    DECLARE @ResolutionHours INT;
    SELECT @ResolutionHours = ResolutionTimeHours
    FROM SLAPolicies
    WHERE CategoryId = @ComplaintCategoryId AND Priority = @Priority AND IsActive = 1;
    IF @ResolutionHours IS NOT NULL
        SET @DueDate = DATEADD(HOUR, @ResolutionHours, GETUTCDATE());

    -- Insert complaint
    INSERT INTO Complaints (
        ClientId, Name, ClientEmail, ClientMobile,
        ComplaintChannelId, ComplaintCategoryId, SubCategoryId,
        Subject, Description, Priority, DueDate, CreatedBy
    )
    VALUES (
        @ClientId, @ClientName, @ClientEmail, @ClientMobile,
        @ComplaintChannelId, @ComplaintCategoryId, @SubCategoryId,
        @Subject, @Description, @Priority, @DueDate, @ActorUserId
    );

    DECLARE @ComplaintId BIGINT = SCOPE_IDENTITY();

    -- Auto-create linked Case
    INSERT INTO Cases (ComplaintId, AssignedToUserId, Status)
    VALUES (@ComplaintId, NULL, 'Open');

    -- Log history
    INSERT INTO ComplaintHistory (ComplaintId, Action, NewStatus, PerformedByUserId)
    VALUES (@ComplaintId, 'Create', 'New', @ActorUserId);

    -- Return full complaint detail
    EXEC CMS_Complaint_GetById @ComplaintId;
END;
GO
