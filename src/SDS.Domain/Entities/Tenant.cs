namespace SDS.Domain.Entities;

public class Tenant
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Domain { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    
    // Tenant configuration
    public string? OidcAuthority { get; set; }
    public string? OidcClientId { get; set; }
    public bool RequireMfa { get; set; } = false;
    public bool BlockLegacyAuth { get; set; } = true;
    
    // Library settings
    public bool EnableMainRepository { get; set; } = true;
    public bool RestrictToOwnLibrary { get; set; } = false;
    
    // Navigation properties
    public virtual ICollection<User> Users { get; set; } = new List<User>();
    public virtual ICollection<SdsDocument> SdsDocuments { get; set; } = new List<SdsDocument>();
    public virtual ICollection<Library> Libraries { get; set; } = new List<Library>();
}
