namespace SDS.Domain.Entities;

public enum NotificationType
{
    ReviewDue,
    RegulatoryChange,
    SdsApproved,
    SdsRejected,
    ShareAccess,
    SystemAlert
}

public enum NotificationChannel
{
    Email,
    Teams,
    InApp,
    Sms
}

public class Notification
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public Guid? UserId { get; set; } // Null for tenant-wide notifications
    public NotificationType Type { get; set; }
    public NotificationChannel Channel { get; set; }
    
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string? LinkUrl { get; set; }
    public Guid? RelatedEntityId { get; set; }
    
    public bool IsRead { get; set; } = false;
    public DateTime? ReadAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? SentAt { get; set; }
    
    // Navigation properties
    public virtual Tenant Tenant { get; set; } = null!;
    public virtual User? User { get; set; }
}
