using Microsoft.EntityFrameworkCore;
using SDS.Application.Interfaces;
using SDS.Domain.Entities;
using SDS.Infrastructure.Data;

namespace SDS.Infrastructure.Services;

public class SdsService : ISdsService
{
    private readonly SdsDbContext _context;
    private readonly IAuditService _auditService;

    public SdsService(SdsDbContext context, IAuditService auditService)
    {
        _context = context;
        _auditService = auditService;
    }

    public async Task<SdsDocument> CreateSdsAsync(Guid tenantId, string documentNumber, string? createdBy, CancellationToken cancellationToken = default)
    {
        var sds = new SdsDocument
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            DocumentNumber = documentNumber,
            Version = 1,
            Status = SdsStatus.Draft,
            CreatedBy = createdBy,
            CreatedAt = DateTime.UtcNow,
            IsLatestVersion = true
        };

        // Create default sections
        for (int i = 1; i <= 16; i++)
        {
            sds.Sections.Add(new SdsSection
            {
                Id = Guid.NewGuid(),
                SdsDocumentId = sds.Id,
                SectionNumber = (SectionNumber)i,
                Title = GetSectionTitle((SectionNumber)i),
                Version = 1,
                CreatedAt = DateTime.UtcNow
            });
        }

        _context.SdsDocuments.Add(sds);
        await _context.SaveChangesAsync(cancellationToken);

        await _auditService.LogActionAsync(tenantId, Guid.TryParse(createdBy, out var userId) ? userId : null, 
            EntityType.SdsDocument, sds.Id, AuditAction.Create, 
            $"Created SDS {documentNumber}", cancellationToken: cancellationToken);

        return sds;
    }

    public async Task<SdsDocument?> GetSdsByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.SdsDocuments
            .Include(s => s.Sections)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public async Task<SdsDocument?> GetLatestSdsByDocumentNumberAsync(Guid tenantId, string documentNumber, CancellationToken cancellationToken = default)
    {
        return await _context.SdsDocuments
            .Include(s => s.Sections)
            .Where(s => s.TenantId == tenantId && s.DocumentNumber == documentNumber && s.IsLatestVersion)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<SdsDocument>> SearchSdsAsync(Guid tenantId, string? searchTerm, string? casNumber, string? supplier, CancellationToken cancellationToken = default)
    {
        var query = _context.SdsDocuments
            .Include(s => s.Sections)
            .Where(s => s.TenantId == tenantId && s.IsLatestVersion);

        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(s => 
                s.ProductName!.Contains(searchTerm) || 
                s.DocumentNumber.Contains(searchTerm) ||
                s.CasNumber!.Contains(searchTerm));
        }

        if (!string.IsNullOrEmpty(casNumber))
        {
            query = query.Where(s => s.CasNumber == casNumber);
        }

        if (!string.IsNullOrEmpty(supplier))
        {
            query = query.Where(s => s.SupplierName!.Contains(supplier));
        }

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<SdsDocument> UpdateSdsAsync(SdsDocument sds, CancellationToken cancellationToken = default)
    {
        sds.UpdatedAt = DateTime.UtcNow;
        _context.SdsDocuments.Update(sds);
        await _context.SaveChangesAsync(cancellationToken);
        return sds;
    }

    public async Task<SdsSection> UpdateSectionAsync(Guid sdsId, SectionNumber sectionNumber, string content, string? updatedBy, CancellationToken cancellationToken = default)
    {
        var section = await _context.SdsSections
            .FirstOrDefaultAsync(s => s.SdsDocumentId == sdsId && s.SectionNumber == sectionNumber, cancellationToken);

        if (section == null)
            throw new InvalidOperationException($"Section {sectionNumber} not found for SDS {sdsId}");

        section.Content = content;
        section.UpdatedAt = DateTime.UtcNow;
        section.UpdatedBy = updatedBy;
        section.HasChanges = true;
        section.Version++;

        await _context.SaveChangesAsync(cancellationToken);
        return section;
    }

    public async Task<SdsDocument> CreateNewVersionAsync(Guid sdsId, string? createdBy, CancellationToken cancellationToken = default)
    {
        var existingSds = await GetSdsByIdAsync(sdsId);
        if (existingSds == null)
            throw new InvalidOperationException($"SDS {sdsId} not found");

        // Mark old version as not latest
        existingSds.IsLatestVersion = false;
        _context.SdsDocuments.Update(existingSds);

        // Create new version
        var newVersion = new SdsDocument
        {
            Id = Guid.NewGuid(),
            TenantId = existingSds.TenantId,
            DocumentNumber = existingSds.DocumentNumber,
            RevisionNumber = existingSds.RevisionNumber,
            Version = existingSds.Version + 1,
            Status = SdsStatus.Draft,
            PreviousVersionId = existingSds.Id,
            IsLatestVersion = true,
            CreatedBy = createdBy,
            CreatedAt = DateTime.UtcNow,
            ProductName = existingSds.ProductName,
            CasNumber = existingSds.CasNumber,
            SupplierName = existingSds.SupplierName
        };

        // Copy sections
        foreach (var section in existingSds.Sections)
        {
            newVersion.Sections.Add(new SdsSection
            {
                Id = Guid.NewGuid(),
                SdsDocumentId = newVersion.Id,
                SectionNumber = section.SectionNumber,
                Title = section.Title,
                Content = section.Content,
                HtmlContent = section.HtmlContent,
                Version = section.Version,
                CreatedAt = DateTime.UtcNow
            });
        }

        _context.SdsDocuments.Add(newVersion);
        await _context.SaveChangesAsync(cancellationToken);

        return newVersion;
    }

    public async Task<IEnumerable<SdsDocument>> GetVersionHistoryAsync(Guid sdsId, CancellationToken cancellationToken = default)
    {
        var sds = await GetSdsByIdAsync(sdsId);
        if (sds == null)
            return Enumerable.Empty<SdsDocument>();

        var versions = new List<SdsDocument> { sds };
        var current = sds;

        // Traverse backwards through versions
        while (current.PreviousVersionId.HasValue)
        {
            var previous = await GetSdsByIdAsync(current.PreviousVersionId.Value);
            if (previous != null)
            {
                versions.Add(previous);
                current = previous;
            }
            else
            {
                break;
            }
        }

        return versions.OrderBy(v => v.Version);
    }

    public async Task<SdsReview> SubmitForReviewAsync(Guid sdsId, Guid reviewerId, CancellationToken cancellationToken = default)
    {
        var sds = await GetSdsByIdAsync(sdsId);
        if (sds == null)
            throw new InvalidOperationException($"SDS {sdsId} not found");

        sds.Status = SdsStatus.UnderReview;
        await UpdateSdsAsync(sds);

        var review = new SdsReview
        {
            Id = Guid.NewGuid(),
            SdsDocumentId = sdsId,
            ReviewerId = reviewerId,
            Status = ReviewStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        _context.SdsReviews.Add(review);
        await _context.SaveChangesAsync(cancellationToken);

        return review;
    }

    public async Task<SdsReview> ReviewSdsAsync(Guid reviewId, ReviewStatus status, string? comments, Guid reviewerId, CancellationToken cancellationToken = default)
    {
        var review = await _context.SdsReviews
            .Include(r => r.SdsDocument)
            .FirstOrDefaultAsync(r => r.Id == reviewId, cancellationToken);

        if (review == null)
            throw new InvalidOperationException($"Review {reviewId} not found");

        review.Status = status;
        review.Comments = comments;
        review.ReviewedAt = DateTime.UtcNow;

        var sds = review.SdsDocument;
        if (status == ReviewStatus.Approved)
        {
            sds.Status = SdsStatus.Approved;
            sds.ApprovedBy = reviewerId.ToString();
            sds.ApprovedAt = DateTime.UtcNow;
        }
        else if (status == ReviewStatus.Rejected)
        {
            sds.Status = SdsStatus.Rejected;
        }

        await _context.SaveChangesAsync(cancellationToken);

        return review;
    }

    public async Task<SdsReview?> GetPendingReviewAsync(Guid sdsId, CancellationToken cancellationToken = default)
    {
        return await _context.SdsReviews
            .Where(r => r.SdsDocumentId == sdsId && r.Status == ReviewStatus.Pending)
            .OrderByDescending(r => r.CreatedAt)
            .FirstOrDefaultAsync(cancellationToken);
    }

    private static string GetSectionTitle(SectionNumber sectionNumber) => sectionNumber switch
    {
        SectionNumber.Section1 => "Identification",
        SectionNumber.Section2 => "Hazard(s) identification",
        SectionNumber.Section3 => "Composition/information on ingredients",
        SectionNumber.Section4 => "First-aid measures",
        SectionNumber.Section5 => "Fire-fighting measures",
        SectionNumber.Section6 => "Accidental release measures",
        SectionNumber.Section7 => "Handling and storage",
        SectionNumber.Section8 => "Exposure controls/personal protection",
        SectionNumber.Section9 => "Physical and chemical properties",
        SectionNumber.Section10 => "Stability and reactivity",
        SectionNumber.Section11 => "Toxicological information",
        SectionNumber.Section12 => "Ecological information",
        SectionNumber.Section13 => "Disposal considerations",
        SectionNumber.Section14 => "Transport information",
        SectionNumber.Section15 => "Regulatory information",
        SectionNumber.Section16 => "Other information",
        _ => $"Section {(int)sectionNumber}"
    };
}
