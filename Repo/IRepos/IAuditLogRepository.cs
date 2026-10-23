using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using GovtLandService.Data.Entities;

namespace GovtLandService.Data.Interfaces;

public interface IAuditLogRepository
{
    Task<AuditLog?> GetByIdAsync(long id, IDbTransaction? transaction = null);
    Task<IEnumerable<AuditLog>> GetAllAsync(int limit = 100, int offset = 0, IDbTransaction? transaction = null);
    Task<IEnumerable<AuditLog>> GetByUserIdAsync(long userId, IDbTransaction? transaction = null);
    Task<IEnumerable<AuditLog>> GetByEntityAsync(string entityType, long entityId, IDbTransaction? transaction = null);
    Task<long> CreateAsync(AuditLog auditLog, IDbTransaction? transaction = null);
    Task<bool> DeleteAsync(long id, IDbTransaction? transaction = null);
}
