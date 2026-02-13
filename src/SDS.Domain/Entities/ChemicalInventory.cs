namespace SDS.Domain.Entities;

public class ChemicalInventory
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public string ChemicalName { get; set; } = string.Empty;
    public string? CasNumber { get; set; }
    public string? SupplierName { get; set; }
    public decimal? Quantity { get; set; }
    public string? Unit { get; set; }
    public string? Location { get; set; }
    
    // Link to SDS
    public Guid? SdsDocumentId { get; set; }
    
    // Risk assessment
    public string? RiskLevel { get; set; }
    public DateTime? LastRiskAssessment { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    
    // Navigation properties
    public virtual Tenant Tenant { get; set; } = null!;
    public virtual SdsDocument? SdsDocument { get; set; }
}
