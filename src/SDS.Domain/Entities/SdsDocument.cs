namespace SDS.Domain.Entities;

public enum SdsStatus
{
    Draft,
    UnderReview,
    Approved,
    Rejected,
    Archived
}

public class SdsDocument
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public string DocumentNumber { get; set; } = string.Empty;
    public string? RevisionNumber { get; set; }
    public int Version { get; set; } = 1;
    public SdsStatus Status { get; set; } = SdsStatus.Draft;
    
    // Document metadata
    public string? Title { get; set; }
    public string? ProductName { get; set; }
    public string? CasNumber { get; set; }
    public string? SupplierName { get; set; }
    public DateTime? EffectiveDate { get; set; }
    public DateTime? ReviewDate { get; set; }
    public DateTime? NextReviewDate { get; set; }
    
    // File storage
    public string? FilePath { get; set; }
    public string? FileName { get; set; }
    public long? FileSize { get; set; }
    public string? ContentType { get; set; }
    
    // Version control
    public Guid? PreviousVersionId { get; set; }
    public bool IsLatestVersion { get; set; } = true;
    
    // Privacy & access
    public bool IsRestricted { get; set; } = false;
    public bool IsInMainRepository { get; set; } = false;
    public bool IsShared { get; set; } = false;
    
    // Audit
    public string? CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? UpdatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? ApprovedBy { get; set; }
    public DateTime? ApprovedAt { get; set; }
    
    // Navigation properties
    public virtual Tenant Tenant { get; set; } = null!;
    public virtual ICollection<SdsSection> Sections { get; set; } = new List<SdsSection>();
    public virtual ICollection<SdsReview> Reviews { get; set; } = new List<SdsReview>();
    public virtual ICollection<SdsShare> Shares { get; set; } = new List<SdsShare>();
    public virtual ICollection<LibrarySds> LibrarySds { get; set; } = new List<LibrarySds>();
    public virtual ICollection<QrCode> QrCodes { get; set; } = new List<QrCode>();
    public virtual ICollection<Ghslabel> GhsLabels { get; set; } = new List<Ghslabel>();
}
