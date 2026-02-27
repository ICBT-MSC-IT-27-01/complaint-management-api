using Cd.Cms.Application.Contracts.Repositories;
using Cd.Cms.Application.Contracts.Services;
using Cd.Cms.Application.DTOs.Attachments;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Cd.Cms.Application.Services
{
    public class AttachmentService : IAttachmentService
    {
        private readonly IAttachmentRepository _repo;
        private readonly string _uploadRoot;
        private static readonly string[] AllowedTypes = { ".pdf", ".doc", ".docx", ".jpg", ".jpeg", ".png", ".xlsx", ".txt" };

        public AttachmentService(IAttachmentRepository repo, IConfiguration config)
        {
            _repo = repo;
            _uploadRoot = config["Attachments:StoragePath"] ?? Path.Combine(Directory.GetCurrentDirectory(), "uploads");
        }

        public Task<List<AttachmentDto>> GetByComplaintIdAsync(long complaintId) => _repo.GetByComplaintIdAsync(complaintId);

        public async Task<AttachmentDto> UploadAsync(long complaintId, IFormFile file, long actorUserId)
        {
            if (file == null || file.Length == 0) throw new ArgumentException("No file provided.");
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!AllowedTypes.Contains(ext)) throw new ArgumentException($"File type {ext} is not allowed.");
            if (file.Length > 10 * 1024 * 1024) throw new ArgumentException("File exceeds 10MB limit.");

            var folder = Path.Combine(_uploadRoot, complaintId.ToString());
            Directory.CreateDirectory(folder);
            var storedName = $"{Guid.NewGuid()}{ext}";
            var storedPath = Path.Combine(folder, storedName);

            using (var stream = new FileStream(storedPath, FileMode.Create))
                await file.CopyToAsync(stream);

            return await _repo.SaveAsync(complaintId, file.FileName, file.ContentType, file.Length, storedPath, actorUserId);
        }

        public Task DeleteAsync(long id, long actorUserId) => _repo.DeleteAsync(id, actorUserId);
    }
}
