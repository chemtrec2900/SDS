namespace SDS.Domain.Entities;

public class Ghslabel
{
    public Guid Id { get; set; }
    public Guid SdsDocumentId { get; set; }
    public string? ProductName { get; set; }
    public string? SignalWord { get; set; } // Danger, Warning
    public string? HazardStatements { get; set; } // H-codes
    public string? PrecautionaryStatements { get; set; } // P-codes
    public string? Pictograms { get; set; } // JSON array of pictogram codes
    
    // Generated label
    public string? LabelImageUrl { get; set; }
    public string? LabelPdfUrl { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? CreatedBy { get; set; }
    
    // Navigation properties
    public virtual SdsDocument SdsDocument { get; set; } = null!;
}
