namespace Cd.Cms.Domain
{
    public abstract class BaseDomainEntity
    {
        public long Id { get; protected set; }
        public string Name { get; protected set; } = string.Empty;
        public DateTime CreatedDateTime { get; protected set; }
        public long CreatedBy { get; protected set; }
        public bool IsActive { get; protected set; }
        public DateTime? UpdatedDateTime { get; protected set; }
        public long? UpdatedBy { get; protected set; }

        protected BaseDomainEntity() { IsActive = true; CreatedDateTime = DateTime.UtcNow; }
        protected BaseDomainEntity(string name, long createdBy) : this()
        { Name = name?.Trim() ?? string.Empty; CreatedBy = createdBy; }

        public void Activate(long actorUserId) { IsActive = true; Touch(actorUserId); }
        public void Deactivate(long actorUserId) { IsActive = false; Touch(actorUserId); }
        public void Touch(long actorUserId) { UpdatedDateTime = DateTime.UtcNow; UpdatedBy = actorUserId; }
        public void SetId(long id) { if (id > 0) Id = id; }
    }
}
