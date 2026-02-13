namespace SDS.Domain.Entities;

public enum SectionNumber
{
    Section1 = 1,   // Identification
    Section2 = 2,   // Hazard(s) identification
    Section3 = 3,   // Composition/information on ingredients
    Section4 = 4,   // First-aid measures
    Section5 = 5,   // Fire-fighting measures
    Section6 = 6,   // Accidental release measures
    Section7 = 7,   // Handling and storage
    Section8 = 8,   // Exposure controls/personal protection
    Section9 = 9,   // Physical and chemical properties
    Section10 = 10, // Stability and reactivity
    Section11 = 11, // Toxicological information
    Section12 = 12, // Ecological information
    Section13 = 13, // Disposal considerations
    Section14 = 14, // Transport information
    Section15 = 15, // Regulatory information
    Section16 = 16  // Other information
}

public class SdsSection
{
    public Guid Id { get; set; }
    public Guid SdsDocumentId { get; set; }
    public SectionNumber SectionNumber { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Content { get; set; }
    public string? HtmlContent { get; set; }
    public string? JsonData { get; set; } // For structured data
    
    // Version tracking
    public int Version { get; set; } = 1;
    public bool HasChanges { get; set; } = false;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }
    
    // Navigation properties
    public virtual SdsDocument SdsDocument { get; set; } = null!;
}
