namespace SDS.Domain.Entities;

public enum RoleType
{
    Admin,
    Author,
    Reviewer,
    Compliance,
    InventoryManager,
    Viewer,
    SupportStaff,
    Management,
    DownstreamUser
}

public class Role
{
    public Guid Id { get; set; }
    public RoleType Type { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    
    // Navigation properties
    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
