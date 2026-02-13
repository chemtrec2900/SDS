namespace SDS.Domain.Entities;

public class ApiKey
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string KeyHash { get; set; } = string.Empty; // Hashed API key
    public string? Scopes { get; set; } // JSON array of scopes
    
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ExpiresAt { get; set; }
    public DateTime? LastUsedAt { get; set; }
    public string? CreatedBy { get; set; }
    
    // Navigation properties
    public virtual Tenant Tenant { get; set; } = null!;
}
