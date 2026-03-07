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
    SET XACT_ABORT ON;

    IF @ComplaintChannelId IS NULL OR @ComplaintChannelId <= 0
        THROW 50001, 'ComplaintChannelId is required.', 1;

    IF @ComplaintCategoryId IS NULL OR @ComplaintCategoryId <= 0
        THROW 50002, 'ComplaintCategoryId is required.', 1;

    IF NOT EXISTS (SELECT 1 FROM ComplaintChannels WHERE Id = @ComplaintChannelId)
        THROW 50003, 'Invalid ComplaintChannelId.', 1;

    IF NOT EXISTS (SELECT 1 FROM Categories WHERE Id = @ComplaintCategoryId)
        THROW 50004, 'Invalid ComplaintCategoryId.', 1;

    IF @SubCategoryId IS NOT NULL
       AND NOT EXISTS (SELECT 1 FROM Categories WHERE Id = @SubCategoryId)
        THROW 50005, 'Invalid SubCategoryId.', 1;

    IF NOT EXISTS (SELECT 1 FROM Users WHERE Id = @ActorUserId)
        THROW 50006, 'Invalid ActorUserId.', 1;

    -- Calculate SLA due date
    DECLARE @DueDate DATETIME2 = NULL;
    DECLARE @ResolutionHours INT;
    SELECT @ResolutionHours = ResolutionTimeHours
    FROM SLAPolicies
    WHERE CategoryId = @ComplaintCategoryId AND Priority = @Priority AND IsActive = 1;
    IF @ResolutionHours IS NOT NULL
        SET @DueDate = DATEADD(HOUR, @ResolutionHours, GETUTCDATE());

    DECLARE @ComplaintId BIGINT;

    BEGIN TRY
        BEGIN TRANSACTION;
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

        SET @ComplaintId = SCOPE_IDENTITY();

        IF @ComplaintId IS NULL
            THROW 50007, 'Failed to create complaint.', 1;

        -- Auto-create linked Case
        INSERT INTO Cases (ComplaintId, AssignedToUserId, Status)
        VALUES (@ComplaintId, NULL, 'Open');

        -- Log history
        INSERT INTO ComplaintHistory (ComplaintId, Action, NewStatus, PerformedByUserId)
        VALUES (@ComplaintId, 'Create', 'New', @ActorUserId);

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0
            ROLLBACK TRANSACTION;
        THROW;
    END CATCH

    -- Return full complaint detail
    EXEC CMS_Complaint_GetById @ComplaintId;
END;
GO
