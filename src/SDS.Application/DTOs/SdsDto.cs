using SDS.Domain.Entities;

namespace SDS.Application.DTOs;

public class SdsDto
{
    public Guid Id { get; set; }
    public string DocumentNumber { get; set; } = string.Empty;
    public string? RevisionNumber { get; set; }
    public int Version { get; set; }
    public SdsStatus Status { get; set; }
    public string? Title { get; set; }
    public string? ProductName { get; set; }
    public string? CasNumber { get; set; }
    public string? SupplierName { get; set; }
    public DateTime? EffectiveDate { get; set; }
    public DateTime? ReviewDate { get; set; }
    public DateTime? NextReviewDate { get; set; }
    public bool IsRestricted { get; set; }
    public bool IsShared { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? ApprovedBy { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public List<SdsSectionDto> Sections { get; set; } = new();
}

public class SdsSectionDto
{
    public Guid Id { get; set; }
    public SectionNumber SectionNumber { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Content { get; set; }
    public string? HtmlContent { get; set; }
    public int Version { get; set; }
    public bool HasChanges { get; set; }
}

public class CreateSdsRequest
{
    public string DocumentNumber { get; set; } = string.Empty;
    public string? ProductName { get; set; }
    public string? CasNumber { get; set; }
    public string? SupplierName { get; set; }
}

public class UpdateSdsSectionRequest
{
    public SectionNumber SectionNumber { get; set; }
    public string Content { get; set; } = string.Empty;
    public string? HtmlContent { get; set; }
}

public class ReviewSdsRequest
{
    public ReviewStatus Status { get; set; }
    public string? Comments { get; set; }
    public string? ChangeRequest { get; set; }
}
