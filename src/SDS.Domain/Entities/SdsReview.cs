namespace SDS.Domain.Entities;

public enum ReviewStatus
{
    Pending,
    Approved,
    Rejected,
    ChangesRequested
}

public class SdsReview
{
    public Guid Id { get; set; }
    public Guid SdsDocumentId { get; set; }
    public Guid ReviewerId { get; set; }
    public ReviewStatus Status { get; set; } = ReviewStatus.Pending;
    public string? Comments { get; set; }
    public string? ChangeRequest { get; set; }
    
    // Diff tracking
    public string? DiffSummary { get; set; }
    public string? ChangedSections { get; set; } // JSON array of section numbers
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ReviewedAt { get; set; }
    
    // Navigation properties
    public virtual SdsDocument SdsDocument { get; set; } = null!;
    public virtual User Reviewer { get; set; } = null!;
}
