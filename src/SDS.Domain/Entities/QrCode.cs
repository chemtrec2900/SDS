namespace SDS.Domain.Entities;

public class QrCode
{
    public Guid Id { get; set; }
    public Guid SdsDocumentId { get; set; }
    public string Code { get; set; } = string.Empty; // QR code data
    public string? ImageUrl { get; set; } // Generated QR code image
    public string? PublicUrl { get; set; } // Public URL that QR code points to
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? CreatedBy { get; set; }
    public bool IsActive { get; set; } = true;
    
    // Navigation properties
    public virtual SdsDocument SdsDocument { get; set; } = null!;
}
