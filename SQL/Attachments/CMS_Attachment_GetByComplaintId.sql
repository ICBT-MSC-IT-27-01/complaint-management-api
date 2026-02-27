CREATE OR ALTER PROCEDURE CMS_Attachment_GetByComplaintId @ComplaintId BIGINT
AS
BEGIN
    SELECT a.Id, a.ComplaintId, a.FileName, a.FileType, a.FileSizeBytes, a.StoredPath,
           u.Name AS UploadedByName, a.UploadedAt
    FROM Attachments a JOIN Users u ON u.Id=a.UploadedBy
    WHERE a.ComplaintId=@ComplaintId AND a.IsActive=1 ORDER BY a.UploadedAt DESC;
END;
GO
