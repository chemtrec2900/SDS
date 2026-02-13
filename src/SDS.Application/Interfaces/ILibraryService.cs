using SDS.Domain.Entities;

namespace SDS.Application.Interfaces;

public interface ILibraryService
{
    Task<Library> CreateLibraryAsync(Guid tenantId, Guid? ownerId, LibraryType type, string name, CancellationToken cancellationToken = default);
    Task<Library?> GetLibraryByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Library>> GetUserLibrariesAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Library>> GetTenantLibrariesAsync(Guid tenantId, CancellationToken cancellationToken = default);
    Task AddSdsToLibraryAsync(Guid libraryId, Guid sdsId, string? addedBy, CancellationToken cancellationToken = default);
    Task RemoveSdsFromLibraryAsync(Guid libraryId, Guid sdsId, CancellationToken cancellationToken = default);
    Task<IEnumerable<SdsDocument>> GetLibrarySdsAsync(Guid libraryId, CancellationToken cancellationToken = default);
}
