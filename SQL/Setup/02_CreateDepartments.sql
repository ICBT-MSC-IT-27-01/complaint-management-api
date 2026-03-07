IF OBJECT_ID('dbo.Departments', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Departments (
        Id               BIGINT        IDENTITY(1,1) PRIMARY KEY,
        DepartmentCode   NVARCHAR(20)  NOT NULL UNIQUE,
        Name             NVARCHAR(150) NOT NULL UNIQUE,
        Description      NVARCHAR(500) NULL,
        SortOrder        INT           NOT NULL DEFAULT 0,
        IsActive         BIT           NOT NULL DEFAULT 1,
        CreatedDateTime  DATETIME2     NOT NULL DEFAULT GETUTCDATE(),
        CreatedBy        BIGINT        NOT NULL DEFAULT 0,
        UpdatedDateTime  DATETIME2     NULL,
        UpdatedBy        BIGINT        NULL
    );
END
GO
