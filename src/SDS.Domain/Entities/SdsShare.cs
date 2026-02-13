namespace SDS.Domain.Entities;

public enum ShareScope
{
    Public,
    Authenticated,
    TenantOnly,
    SpecificUsers
}

public class SdsShare
{
    public Guid Id { get; set; }
    public Guid SdsDocumentId { get; set; }
    public ShareScope Scope { get; set; }
    public string? ShareToken { get; set; }
    public string? ShareUrl { get; set; }
    
    // Expiry
    public DateTime? ExpiresAt { get; set; }
    public int? MaxAccessCount { get; set; }
    public int AccessCount { get; set; } = 0;
    
    // Email sharing
    public string? SharedViaEmail { get; set; }
    public string? SharedViaSms { get; set; }
    
    // Embedding
    public bool AllowEmbedding { get; set; } = false;
    public string? AllowedDomains { get; set; } // Comma-separated
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? CreatedBy { get; set; }
    public bool IsActive { get; set; } = true;
    
    // Navigation properties
    public virtual SdsDocument SdsDocument { get; set; } = null!;
}
