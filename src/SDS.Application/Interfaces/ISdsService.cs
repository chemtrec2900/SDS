using SDS.Domain.Entities;

namespace SDS.Application.Interfaces;

public interface ISdsService
{
    Task<SdsDocument> CreateSdsAsync(Guid tenantId, string documentNumber, string? createdBy, CancellationToken cancellationToken = default);
    Task<SdsDocument?> GetSdsByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<SdsDocument?> GetLatestSdsByDocumentNumberAsync(Guid tenantId, string documentNumber, CancellationToken cancellationToken = default);
    Task<IEnumerable<SdsDocument>> SearchSdsAsync(Guid tenantId, string? searchTerm, string? casNumber, string? supplier, CancellationToken cancellationToken = default);
    Task<SdsDocument> UpdateSdsAsync(SdsDocument sds, CancellationToken cancellationToken = default);
    Task<SdsSection> UpdateSectionAsync(Guid sdsId, SectionNumber sectionNumber, string content, string? updatedBy, CancellationToken cancellationToken = default);
    Task<SdsDocument> CreateNewVersionAsync(Guid sdsId, string? createdBy, CancellationToken cancellationToken = default);
    Task<IEnumerable<SdsDocument>> GetVersionHistoryAsync(Guid sdsId, CancellationToken cancellationToken = default);
    Task<SdsReview> SubmitForReviewAsync(Guid sdsId, Guid reviewerId, CancellationToken cancellationToken = default);
    Task<SdsReview> ReviewSdsAsync(Guid reviewId, ReviewStatus status, string? comments, Guid reviewerId, CancellationToken cancellationToken = default);
    Task<SdsReview?> GetPendingReviewAsync(Guid sdsId, CancellationToken cancellationToken = default);
}
