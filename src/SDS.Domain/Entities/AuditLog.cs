namespace SDS.Domain.Entities;

public enum AuditAction
{
    Create,
    Update,
    Delete,
    View,
    Download,
    Share,
    Approve,
    Reject,
    Import,
    Export
}

public enum EntityType
{
    SdsDocument,
    User,
    Tenant,
    Library,
    Review,
    Share,
    Label,
    QrCode
}

public class AuditLog
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public Guid? UserId { get; set; }
    public EntityType EntityType { get; set; }
    public Guid? EntityId { get; set; }
    public AuditAction Action { get; set; }
    
    public string? Description { get; set; }
    public string? OldValues { get; set; } // JSON
    public string? NewValues { get; set; } // JSON
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
    
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public virtual Tenant Tenant { get; set; } = null!;
    public virtual User? User { get; set; }
}
