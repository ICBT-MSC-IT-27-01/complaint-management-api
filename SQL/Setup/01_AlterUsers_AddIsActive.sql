-- Add IsActive column to Users table if it does not already exist.
IF COL_LENGTH('dbo.Users', 'IsActive') IS NULL
BEGIN
    ALTER TABLE dbo.Users
    ADD IsActive BIT NOT NULL CONSTRAINT DF_Users_IsActive DEFAULT (1);
END;
GO
