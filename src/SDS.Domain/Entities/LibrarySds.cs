namespace SDS.Domain.Entities;

public class LibrarySds
{
    public Guid LibraryId { get; set; }
    public Guid SdsDocumentId { get; set; }
    public DateTime AddedAt { get; set; } = DateTime.UtcNow;
    public string? AddedBy { get; set; }
    
    // Navigation properties
    public virtual Library Library { get; set; } = null!;
    public virtual SdsDocument SdsDocument { get; set; } = null!;
}
