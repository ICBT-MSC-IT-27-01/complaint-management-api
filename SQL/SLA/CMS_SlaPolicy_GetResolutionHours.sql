CREATE OR ALTER PROCEDURE CMS_SlaPolicy_GetResolutionHours
    @CategoryId BIGINT, @Priority NVARCHAR(20)
AS
BEGIN
    SELECT ResolutionTimeHours FROM SLAPolicies
    WHERE CategoryId=@CategoryId AND Priority=@Priority AND IsActive=1;
END;
GO
