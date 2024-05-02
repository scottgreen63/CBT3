namespace CBT3_Domain.Common;

public abstract class BaseAuditableEntity : BaseEntity, IAuditableEntity
{
    protected BaseAuditableEntity(BaseId<string> id) : base(id)
    {
    }

    public string? CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
}
