namespace Cd.Cms.Domain
{
    public class Complaint : BaseDomainEntity
    {
        public string ComplaintNumber { get; private set; } = string.Empty;
        public long? ClientId { get; private set; }
        public string ClientEmail { get; private set; } = string.Empty;
        public string ClientMobile { get; private set; } = string.Empty;
        public long ComplaintChannelId { get; private set; }
        public long ComplaintCategoryId { get; private set; }
        public long ComplaintStatusId { get; private set; }
        public string Priority { get; private set; } = string.Empty;
        public string Subject { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public long? SLAId { get; private set; }
        public DateTime? DueDate { get; private set; }
        public string SlaStatus { get; private set; } = "WithinSLA";
        public bool IsSlaBreached { get; private set; }
        public long? AssignedToUserId { get; private set; }
        public DateTime? AssignedDate { get; private set; }
        public bool IsResolved { get; private set; }
        public DateTime? ResolvedDate { get; private set; }
        public string ResolutionNotes { get; private set; } = string.Empty;
        public bool IsClosed { get; private set; }
        public DateTime? ClosedDate { get; private set; }

        public Complaint() : base() { }

        public Complaint(string subject, string description, string priority,
            long channelId, long categoryId, long createdBy,
            long? clientId = null, string? clientName = null,
            string? clientEmail = null, string? clientMobile = null)
            : base(clientName ?? "N/A", createdBy)
        {
            Subject = subject?.Trim() ?? throw new ArgumentException("Subject required.");
            Description = description?.Trim() ?? throw new ArgumentException("Description required.");
            Priority = priority?.Trim() ?? throw new ArgumentException("Priority required.");
            ComplaintChannelId = channelId;
            ComplaintCategoryId = categoryId;
            ClientId = clientId;
            ClientEmail = clientEmail?.Trim() ?? string.Empty;
            ClientMobile = clientMobile?.Trim() ?? string.Empty;
            ComplaintStatusId = 1; // New
        }

        public void AssignTo(long userId, DateTime? dueDate, long actorUserId)
        { AssignedToUserId = userId; AssignedDate = DateTime.UtcNow; DueDate = dueDate; Touch(actorUserId); }

        public void ChangeStatus(long statusId, long actorUserId)
        { ComplaintStatusId = statusId; Touch(actorUserId); }

        public void MarkSlaBreached(long actorUserId)
        { IsSlaBreached = true; SlaStatus = "Breached"; Touch(actorUserId); }

        public void Resolve(string resolutionNotes, long actorUserId)
        { IsResolved = true; ResolvedDate = DateTime.UtcNow; ResolutionNotes = resolutionNotes; Touch(actorUserId); }

        public void Close(long actorUserId)
        { IsClosed = true; ClosedDate = DateTime.UtcNow; Touch(actorUserId); }
    }
}
