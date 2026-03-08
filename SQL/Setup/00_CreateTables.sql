-- ============================================================
-- CMS Database Setup — Create All Tables
-- Run this script FIRST before any stored procedures
-- ============================================================

-- Users
CREATE TABLE Users (
    Id                BIGINT        IDENTITY(1,1) PRIMARY KEY,
    Name              NVARCHAR(200) NOT NULL,
    Email             NVARCHAR(200) NOT NULL UNIQUE,
    Username          NVARCHAR(100) NOT NULL UNIQUE,
    PhoneNumber       NVARCHAR(20)  NULL,
    PasswordHash      NVARCHAR(500) NOT NULL,
    Role              NVARCHAR(50)  NOT NULL DEFAULT 'Agent',
    Department        NVARCHAR(100) NULL,
    ReportingManagerId BIGINT       NULL REFERENCES Users(Id),
    IsActive          BIT           NOT NULL DEFAULT 1,
    IsLocked          BIT           NOT NULL DEFAULT 0,
    CreatedDateTime   DATETIME2     NOT NULL DEFAULT GETUTCDATE(),
    CreatedBy         BIGINT        NOT NULL DEFAULT 0,
    UpdatedDateTime   DATETIME2     NULL,
    UpdatedBy         BIGINT        NULL,
    LastLoginDateTime DATETIME2     NULL
);

-- Clients
CREATE TABLE Clients (
    Id               BIGINT        IDENTITY(1,1) PRIMARY KEY,
    ClientCode       NVARCHAR(50)  NOT NULL UNIQUE,
    CompanyName      NVARCHAR(300) NOT NULL,
    PrimaryEmail     NVARCHAR(200) NOT NULL,
    PrimaryPhone     NVARCHAR(20)  NULL,
    Address          NVARCHAR(500) NULL,
    ClientType       NVARCHAR(50)  NOT NULL DEFAULT 'Standard',
    AccountManagerId BIGINT        NULL REFERENCES Users(Id),
    IsActive         BIT           NOT NULL DEFAULT 1,
    CreatedDateTime  DATETIME2     NOT NULL DEFAULT GETUTCDATE(),
    CreatedBy        BIGINT        NOT NULL DEFAULT 0,
    UpdatedDateTime  DATETIME2     NULL,
    UpdatedBy        BIGINT        NULL
);

-- ParentCategories
CREATE TABLE ParentCategories (
    Id               BIGINT        IDENTITY(1,1) PRIMARY KEY,
    Name             NVARCHAR(150) NOT NULL UNIQUE,
    SortOrder        INT           NOT NULL DEFAULT 0,
    IsActive         BIT           NOT NULL DEFAULT 1,
    CreatedDateTime  DATETIME2     NOT NULL DEFAULT GETUTCDATE(),
    CreatedBy        BIGINT        NOT NULL DEFAULT 0,
    UpdatedDateTime  DATETIME2     NULL,
    UpdatedBy        BIGINT        NULL
);

-- Categories
CREATE TABLE Categories (
    Id               BIGINT        IDENTITY(1,1) PRIMARY KEY,
    Name             NVARCHAR(150) NOT NULL,
    ParentCategoryId BIGINT        NOT NULL REFERENCES ParentCategories(Id),
    SortOrder        INT           NOT NULL DEFAULT 0,
    IsActive         BIT           NOT NULL DEFAULT 1,
    CreatedDateTime  DATETIME2     NOT NULL DEFAULT GETUTCDATE(),
    CreatedBy        BIGINT        NOT NULL DEFAULT 0,
    UpdatedDateTime  DATETIME2     NULL,
    UpdatedBy        BIGINT        NULL
);
CREATE UNIQUE INDEX UX_Categories_Parent_Name ON Categories(ParentCategoryId, Name);

-- Departments
CREATE TABLE Departments (
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

-- Teams
CREATE TABLE Teams (
    Id               BIGINT         IDENTITY(1,1) PRIMARY KEY,
    TeamCode         NVARCHAR(20)   NOT NULL UNIQUE,
    TeamName         NVARCHAR(150)  NOT NULL UNIQUE,
    LeadUserId       BIGINT         NULL REFERENCES Users(Id),
    IsActive         BIT            NOT NULL DEFAULT 1,
    CreatedDateTime  DATETIME2      NOT NULL DEFAULT GETUTCDATE(),
    CreatedBy        BIGINT         NOT NULL DEFAULT 0,
    UpdatedDateTime  DATETIME2      NULL,
    UpdatedBy        BIGINT         NULL
);

CREATE TABLE TeamMembers (
    TeamId           BIGINT        NOT NULL REFERENCES Teams(Id),
    UserId           BIGINT        NOT NULL REFERENCES Users(Id),
    CreatedDateTime  DATETIME2     NOT NULL DEFAULT GETUTCDATE(),
    CreatedBy        BIGINT        NOT NULL DEFAULT 0,
    PRIMARY KEY (TeamId, UserId),
    CONSTRAINT UX_TeamMembers_User UNIQUE (UserId)
);

-- Roles and Permissions
CREATE TABLE Roles (
    Id               BIGINT        IDENTITY(1,1) PRIMARY KEY,
    Role             NVARCHAR(100) NOT NULL UNIQUE,
    DisplayName      NVARCHAR(150) NOT NULL,
    IsSystem         BIT           NOT NULL DEFAULT 1,
    IsActive         BIT           NOT NULL DEFAULT 1,
    CreatedDateTime  DATETIME2     NOT NULL DEFAULT GETUTCDATE(),
    CreatedBy        BIGINT        NOT NULL DEFAULT 0,
    UpdatedDateTime  DATETIME2     NULL,
    UpdatedBy        BIGINT        NULL
);

CREATE TABLE PermissionModules (
    Id               BIGINT        IDENTITY(1,1) PRIMARY KEY,
    Module           NVARCHAR(100) NOT NULL UNIQUE,
    DisplayName      NVARCHAR(150) NOT NULL,
    IsActive         BIT           NOT NULL DEFAULT 1,
    CreatedDateTime  DATETIME2     NOT NULL DEFAULT GETUTCDATE()
);

CREATE TABLE RolePermissions (
    RoleId           BIGINT        NOT NULL REFERENCES Roles(Id),
    ModuleId         BIGINT        NOT NULL REFERENCES PermissionModules(Id),
    CanRead          BIT           NOT NULL DEFAULT 0,
    CanWrite         BIT           NOT NULL DEFAULT 0,
    CanDelete        BIT           NOT NULL DEFAULT 0,
    UpdatedDateTime  DATETIME2     NOT NULL DEFAULT GETUTCDATE(),
    UpdatedBy        BIGINT        NOT NULL DEFAULT 0,
    PRIMARY KEY (RoleId, ModuleId)
);

CREATE TABLE PermissionAuditTrail (
    Id               BIGINT        IDENTITY(1,1) PRIMARY KEY,
    RoleId           BIGINT        NULL REFERENCES Roles(Id),
    Action           NVARCHAR(50)  NOT NULL,
    Details          NVARCHAR(MAX) NULL,
    ChangedByUserId  BIGINT        NULL REFERENCES Users(Id),
    ChangedDateTime  DATETIME2     NOT NULL DEFAULT GETUTCDATE()
);

-- Seed Roles
INSERT INTO Roles (Role, DisplayName, IsSystem, IsActive, CreatedBy) VALUES
('SystemAdministrator', 'System Administrator', 1, 1, 0),
('Manager', 'Manager', 1, 1, 0),
('Supervisor', 'Supervisor', 1, 1, 0),
('CallCenterAgent', 'Call Center Agent', 1, 1, 0),
('TechnicalEngineer', 'Technical Engineer', 1, 1, 0),
('BillingOfficer', 'Billing Officer', 1, 1, 0),
('Client', 'Client', 1, 1, 0);

-- Seed Permission Modules
INSERT INTO PermissionModules (Module, DisplayName, IsActive) VALUES
('Users', 'Users', 1),
('RolesPermissions', 'Roles & Permissions', 1),
('Teams', 'Teams', 1),
('Complaints', 'Complaints', 1),
('Reports', 'Reports', 1),
('TeamMembers', 'Team Members', 1),
('ComplaintStatus', 'Complaint Status', 1),
('ComplaintNotes', 'Complaint Notes', 1),
('TechnicalUpdates', 'Technical Updates', 1),
('BillingRecords', 'Billing Records', 1),
('CreateComplaint', 'Create Complaint', 1),
('ViewOwnComplaints', 'View Own Complaints', 1);

-- Seed Default Role Permissions from PRD
INSERT INTO RolePermissions (RoleId, ModuleId, CanRead, CanWrite, CanDelete, UpdatedBy)
SELECT r.Id, m.Id,
       CASE WHEN x.CanRead = 1 THEN 1 ELSE 0 END,
       CASE WHEN x.CanWrite = 1 THEN 1 ELSE 0 END,
       CASE WHEN x.CanDelete = 1 THEN 1 ELSE 0 END,
       0
FROM Roles r
INNER JOIN (
    SELECT 'SystemAdministrator' AS Role, 'Users' AS Module, 1 AS CanRead, 1 AS CanWrite, 1 AS CanDelete UNION ALL
    SELECT 'SystemAdministrator', 'RolesPermissions', 1, 1, 1 UNION ALL
    SELECT 'SystemAdministrator', 'Teams', 1, 1, 1 UNION ALL
    SELECT 'SystemAdministrator', 'Complaints', 1, 1, 1 UNION ALL
    SELECT 'Manager', 'Complaints', 1, 1, 0 UNION ALL
    SELECT 'Manager', 'Teams', 1, 0, 0 UNION ALL
    SELECT 'Manager', 'Reports', 1, 0, 0 UNION ALL
    SELECT 'Supervisor', 'Complaints', 1, 1, 0 UNION ALL
    SELECT 'Supervisor', 'TeamMembers', 1, 1, 0 UNION ALL
    SELECT 'Supervisor', 'ComplaintStatus', 1, 1, 0 UNION ALL
    SELECT 'CallCenterAgent', 'Complaints', 1, 1, 0 UNION ALL
    SELECT 'CallCenterAgent', 'ComplaintNotes', 1, 1, 0 UNION ALL
    SELECT 'TechnicalEngineer', 'Complaints', 1, 1, 0 UNION ALL
    SELECT 'TechnicalEngineer', 'TechnicalUpdates', 1, 1, 0 UNION ALL
    SELECT 'BillingOfficer', 'Complaints', 1, 1, 0 UNION ALL
    SELECT 'BillingOfficer', 'BillingRecords', 1, 1, 0 UNION ALL
    SELECT 'Client', 'CreateComplaint', 1, 1, 0 UNION ALL
    SELECT 'Client', 'ViewOwnComplaints', 1, 0, 0
) x ON x.Role = r.Role
INNER JOIN PermissionModules m ON m.Module = x.Module;

-- SLA Policies
CREATE TABLE SLAPolicies (
    Id                     BIGINT        IDENTITY(1,1) PRIMARY KEY,
    CategoryId             BIGINT        NOT NULL REFERENCES Categories(Id),
    Priority               NVARCHAR(20)  NOT NULL,
    ResponseTimeHours      INT           NOT NULL,
    ResolutionTimeHours    INT           NOT NULL,
    EscalationThresholdPct INT           NOT NULL DEFAULT 80,
    IsActive               BIT           NOT NULL DEFAULT 1,
    CreatedDateTime        DATETIME2     NOT NULL DEFAULT GETUTCDATE(),
    CreatedBy              BIGINT        NOT NULL DEFAULT 0,
    CONSTRAINT UQ_SlaPolicy UNIQUE (CategoryId, Priority)
);

-- Complaint Status Lookup
CREATE TABLE ComplaintStatuses (
    Id    BIGINT        IDENTITY(1,1) PRIMARY KEY,
    Name  NVARCHAR(50)  NOT NULL UNIQUE
);
INSERT INTO ComplaintStatuses (Name) VALUES
    ('New'),('Assigned'),('InProgress'),('Pending'),('Escalated'),('Resolved'),('Closed'),('Rejected');

-- Complaint Channels Lookup
CREATE TABLE ComplaintChannels (
    Id   BIGINT       IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(50) NOT NULL UNIQUE
);
INSERT INTO ComplaintChannels (Name) VALUES ('Phone'),('Email'),('Portal'),('WalkIn');

-- Complaints
CREATE TABLE Complaints (
    Id                  BIGINT         IDENTITY(1,1) PRIMARY KEY,
    ComplaintNumber     NVARCHAR(20)   NULL UNIQUE,
    ClientId            BIGINT         NULL REFERENCES Clients(Id),
    Name                NVARCHAR(300)  NULL,   -- ClientName snapshot
    ClientEmail         NVARCHAR(200)  NULL,
    ClientMobile        NVARCHAR(20)   NULL,
    ComplaintChannelId  BIGINT         NOT NULL REFERENCES ComplaintChannels(Id),
    ComplaintCategoryId BIGINT         NOT NULL REFERENCES Categories(Id),
    SubCategoryId       BIGINT         NULL REFERENCES Categories(Id),
    Subject             NVARCHAR(300)  NOT NULL,
    Description         NVARCHAR(MAX)  NOT NULL,
    Priority            NVARCHAR(20)   NOT NULL DEFAULT 'Medium',
    ComplaintStatusId   BIGINT         NOT NULL DEFAULT 1 REFERENCES ComplaintStatuses(Id),
    AssignedToUserId    BIGINT         NULL REFERENCES Users(Id),
    AssignedDate        DATETIME2      NULL,
    DueDate             DATETIME2      NULL,
    SlaStatus           NVARCHAR(20)   NOT NULL DEFAULT 'WithinSLA',
    IsSlaBreached       BIT            NOT NULL DEFAULT 0,
    IsResolved          BIT            NOT NULL DEFAULT 0,
    ResolvedDate        DATETIME2      NULL,
    ResolutionNotes     NVARCHAR(MAX)  NULL,
    IsClosed            BIT            NOT NULL DEFAULT 0,
    ClosedDate          DATETIME2      NULL,
    IsActive            BIT            NOT NULL DEFAULT 1,
    CreatedDateTime     DATETIME2      NOT NULL DEFAULT GETUTCDATE(),
    CreatedBy           BIGINT         NOT NULL DEFAULT 0,
    UpdatedDateTime     DATETIME2      NULL,
    UpdatedBy           BIGINT         NULL
);

-- Auto-generate ComplaintNumber trigger
CREATE TRIGGER trg_Complaints_RefNumber
ON Complaints AFTER INSERT AS
BEGIN
    UPDATE Complaints
    SET ComplaintNumber = 'CMP-' + FORMAT(YEAR(GETUTCDATE()),'0000')
        + RIGHT('00000' + CAST(Id AS NVARCHAR), 5)
    WHERE Id IN (SELECT Id FROM inserted);
END;
GO

-- Cases
CREATE TABLE Cases (
    Id               BIGINT        IDENTITY(1,1) PRIMARY KEY,
    CaseNumber       NVARCHAR(20)  NULL UNIQUE,
    ComplaintId      BIGINT        NOT NULL REFERENCES Complaints(Id),
    AssignedToUserId BIGINT        NULL REFERENCES Users(Id),
    Status           NVARCHAR(30)  NOT NULL DEFAULT 'Open',
    Notes            NVARCHAR(MAX) NULL,
    OpenedAt         DATETIME2     NOT NULL DEFAULT GETUTCDATE(),
    ClosedAt         DATETIME2     NULL
);

CREATE TRIGGER trg_Cases_Number ON Cases AFTER INSERT AS
BEGIN
    UPDATE Cases SET CaseNumber = 'CASE-' + FORMAT(YEAR(GETUTCDATE()),'0000')
        + RIGHT('00000' + CAST(Id AS NVARCHAR), 5)
    WHERE Id IN (SELECT Id FROM inserted);
END;
GO

-- CaseActivities
CREATE TABLE CaseActivities (
    Id                BIGINT        IDENTITY(1,1) PRIMARY KEY,
    CaseId            BIGINT        NOT NULL REFERENCES Cases(Id),
    ActivityType      NVARCHAR(50)  NOT NULL DEFAULT 'Note',
    Description       NVARCHAR(MAX) NOT NULL,
    PerformedByUserId BIGINT        NOT NULL REFERENCES Users(Id),
    CreatedDateTime   DATETIME2     NOT NULL DEFAULT GETUTCDATE()
);

-- Complaint History
CREATE TABLE ComplaintHistory (
    Id                BIGINT        IDENTITY(1,1) PRIMARY KEY,
    ComplaintId       BIGINT        NOT NULL REFERENCES Complaints(Id),
    Action            NVARCHAR(50)  NOT NULL,
    OldStatus         NVARCHAR(50)  NULL,
    NewStatus         NVARCHAR(50)  NULL,
    Note              NVARCHAR(MAX) NULL,
    PerformedByUserId BIGINT        NOT NULL REFERENCES Users(Id),
    CreatedDateTime   DATETIME2     NOT NULL DEFAULT GETUTCDATE()
);

-- Escalations
CREATE TABLE Escalations (
    Id                BIGINT        IDENTITY(1,1) PRIMARY KEY,
    ComplaintId       BIGINT        NOT NULL REFERENCES Complaints(Id),
    EscalatedByUserId BIGINT        NOT NULL REFERENCES Users(Id),
    EscalatedToUserId BIGINT        NOT NULL REFERENCES Users(Id),
    Reason            NVARCHAR(MAX) NOT NULL,
    EscalationType    NVARCHAR(30)  NOT NULL DEFAULT 'Manual',
    EscalatedAt       DATETIME2     NOT NULL DEFAULT GETUTCDATE(),
    ResolvedAt        DATETIME2     NULL
);

-- Attachments
CREATE TABLE Attachments (
    Id            BIGINT         IDENTITY(1,1) PRIMARY KEY,
    ComplaintId   BIGINT         NOT NULL REFERENCES Complaints(Id),
    FileName      NVARCHAR(300)  NOT NULL,
    FileType      NVARCHAR(100)  NOT NULL,
    FileSizeBytes BIGINT         NOT NULL,
    StoredPath    NVARCHAR(1000) NOT NULL,
    UploadedBy    BIGINT         NOT NULL REFERENCES Users(Id),
    UploadedAt    DATETIME2      NOT NULL DEFAULT GETUTCDATE(),
    IsActive      BIT            NOT NULL DEFAULT 1
);

-- AuditLogs (immutable — never UPDATE or DELETE)
CREATE TABLE AuditLogs (
    Id                BIGINT         IDENTITY(1,1) PRIMARY KEY,
    EntityType        NVARCHAR(100)  NOT NULL,
    EntityId          BIGINT         NOT NULL,
    Action            NVARCHAR(30)   NOT NULL,
    OldValues         NVARCHAR(MAX)  NULL,
    NewValues         NVARCHAR(MAX)  NULL,
    PerformedByUserId BIGINT         NULL REFERENCES Users(Id),
    IPAddress         NVARCHAR(50)   NULL,
    CreatedDateTime   DATETIME2      NOT NULL DEFAULT GETUTCDATE()
);

-- Seed default admin user (temporary plaintext password: Admin@123 — change immediately!)
INSERT INTO Users (Name, Email, Username, PasswordHash, Role, IsActive, CreatedBy)
VALUES ('System Admin', 'admin@cms.com', 'admin',
    'Admin@123', 'Admin', 1, 0);
GO
