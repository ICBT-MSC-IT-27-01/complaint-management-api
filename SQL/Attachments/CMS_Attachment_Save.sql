CREATE OR ALTER PROCEDURE CMS_Attachment_Save
    @ComplaintId BIGINT, @FileName NVARCHAR(300), @FileType NVARCHAR(100),
    @FileSizeBytes BIGINT, @StoredPath NVARCHAR(1000), @ActorUserId BIGINT
AS
BEGIN
    INSERT INTO Attachments (ComplaintId, FileName, FileType, FileSizeBytes, StoredPath, UploadedBy)
    VALUES (@ComplaintId, @FileName, @FileType, @FileSizeBytes, @StoredPath, @ActorUserId);
    DECLARE @Id BIGINT = SCOPE_IDENTITY();
    SELECT a.Id, a.ComplaintId, a.FileName, a.FileType, a.FileSizeBytes, a.StoredPath,
           u.Name AS UploadedByName, a.UploadedAt
    FROM Attachments a JOIN Users u ON u.Id=a.UploadedBy WHERE a.Id=@Id;
END;
GO
