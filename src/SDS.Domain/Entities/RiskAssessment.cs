namespace SDS.Domain.Entities;

public class RiskAssessment
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public Guid? SdsDocumentId { get; set; }
    public Guid? ChemicalInventoryId { get; set; }
    public Guid CreatedBy { get; set; }
    
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ExposureScenarios { get; set; } // JSON
    public string? ControlMeasures { get; set; } // JSON
    public string? HazardRanking { get; set; } // JSON
    public string? MitigationPlan { get; set; } // JSON
    
    public string? RiskLevel { get; set; }
    public DateTime AssessmentDate { get; set; } = DateTime.UtcNow;
    public DateTime? NextReviewDate { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    
    // Navigation properties
    public virtual Tenant Tenant { get; set; } = null!;
    public virtual SdsDocument? SdsDocument { get; set; }
    public virtual ChemicalInventory? ChemicalInventory { get; set; }
}
