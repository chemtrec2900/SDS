using SDS.Domain.Entities;

namespace SDS.Application.Interfaces;

public interface IAuditService
{
    Task LogActionAsync(Guid tenantId, Guid? userId, EntityType entityType, Guid? entityId, AuditAction action, string? description = null, string? oldValues = null, string? newValues = null, string? ipAddress = null, string? userAgent = null, CancellationToken cancellationToken = default);
    Task<IEnumerable<AuditLog>> GetAuditLogsAsync(Guid tenantId, EntityType? entityType = null, Guid? entityId = null, DateTime? startDate = null, DateTime? endDate = null, CancellationToken cancellationToken = default);
}
