namespace SDS.Domain.Entities;

public enum LibraryType
{
    TenantLibrary,
    UserLibrary,
    MainRepository
}

public class Library
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public Guid? OwnerId { get; set; } // Null for tenant library
    public LibraryType Type { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    
    // Navigation properties
    public virtual Tenant Tenant { get; set; } = null!;
    public virtual User? Owner { get; set; }
    public virtual ICollection<LibrarySds> LibrarySds { get; set; } = new List<LibrarySds>();
}
