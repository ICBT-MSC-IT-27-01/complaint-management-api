namespace Cd.Cms.Application.DTOs.Attachments
{
    public class AttachmentDto
    {
        public long Id { get; set; }
        public long ComplaintId { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string FileType { get; set; } = string.Empty;
        public long FileSizeBytes { get; set; }
        public string StoredPath { get; set; } = string.Empty;
        public string UploadedByName { get; set; } = string.Empty;
        public DateTime UploadedAt { get; set; }
    }
}
