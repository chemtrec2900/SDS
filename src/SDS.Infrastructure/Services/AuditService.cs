using Microsoft.EntityFrameworkCore;
using SDS.Application.Interfaces;
using SDS.Domain.Entities;
using SDS.Infrastructure.Data;

namespace SDS.Infrastructure.Services;

public class AuditService : IAuditService
{
    private readonly SdsDbContext _context;

    public AuditService(SdsDbContext context)
    {
        _context = context;
    }

    public async Task LogActionAsync(Guid tenantId, Guid? userId, EntityType entityType, Guid? entityId, AuditAction action, string? description = null, string? oldValues = null, string? newValues = null, string? ipAddress = null, string? userAgent = null, CancellationToken cancellationToken = default)
    {
        var auditLog = new AuditLog
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            UserId = userId,
            EntityType = entityType,
            EntityId = entityId,
            Action = action,
            Description = description,
            OldValues = oldValues,
            NewValues = newValues,
            IpAddress = ipAddress,
            UserAgent = userAgent,
            Timestamp = DateTime.UtcNow
        };

        _context.AuditLogs.Add(auditLog);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<AuditLog>> GetAuditLogsAsync(Guid tenantId, EntityType? entityType = null, Guid? entityId = null, DateTime? startDate = null, DateTime? endDate = null, CancellationToken cancellationToken = default)
    {
        var query = _context.AuditLogs
            .Where(a => a.TenantId == tenantId);

        if (entityType.HasValue)
            query = query.Where(a => a.EntityType == entityType.Value);

        if (entityId.HasValue)
            query = query.Where(a => a.EntityId == entityId.Value);

        if (startDate.HasValue)
            query = query.Where(a => a.Timestamp >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(a => a.Timestamp <= endDate.Value);

        return await query
            .OrderByDescending(a => a.Timestamp)
            .ToListAsync(cancellationToken);
    }
}
