using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using GovtLandService.Data.Entities;
using GovtLandService.Data.Interfaces;

namespace GovtLandService.Data.Repositories;

public class AuditLogRepository : IAuditLogRepository
{
    private readonly IDbConnection _db;

    public AuditLogRepository(IDbConnection db)
    {
        _db = db;
    }

    private const string SelectColumns = @"
        id AS Id,
        user_id AS UserId,
        action AS Action,
        entity_type AS EntityType,
        entity_id AS EntityId,
        old_value::text AS OldValue,
        new_value::text AS NewValue,
        created_at AS CreatedAt";

    public async Task<AuditLog?> GetByIdAsync(long id, IDbTransaction? transaction = null)
    {
        const string sql = $"SELECT {SelectColumns} FROM public.audit_logs WHERE id = @Id;";
        return await _db.QuerySingleOrDefaultAsync<AuditLog>(sql, new { Id = id }, transaction: transaction);
    }

    public async Task<IEnumerable<AuditLog>> GetAllAsync(int limit = 100, int offset = 0, IDbTransaction? transaction = null)
    {
        const string sql = $"SELECT {SelectColumns} FROM public.audit_logs ORDER BY created_at DESC LIMIT @Limit OFFSET @Offset;";
        return await _db.QueryAsync<AuditLog>(sql, new { Limit = limit, Offset = offset }, transaction: transaction);
    }

    public async Task<IEnumerable<AuditLog>> GetByUserIdAsync(long userId, IDbTransaction? transaction = null)
    {
        const string sql = $"SELECT {SelectColumns} FROM public.audit_logs WHERE user_id = @UserId ORDER BY created_at DESC;";
        return await _db.QueryAsync<AuditLog>(sql, new { UserId = userId }, transaction: transaction);
    }

    public async Task<IEnumerable<AuditLog>> GetByEntityAsync(string entityType, long entityId, IDbTransaction? transaction = null)
    {
        const string sql = @"
            SELECT " + SelectColumns + @"
            FROM public.audit_logs
            WHERE entity_type = @EntityType AND entity_id = @EntityId
            ORDER BY created_at DESC;";
        return await _db.QueryAsync<AuditLog>(sql, new { EntityType = entityType, EntityId = entityId }, transaction: transaction);
    }

    public async Task<long> CreateAsync(AuditLog auditLog, IDbTransaction? transaction = null)
    {
        const string sql = @"
            INSERT INTO public.audit_logs (user_id, action, entity_type, entity_id, old_value, new_value, created_at)
            VALUES (@UserId, @Action, @EntityType, @EntityId, @OldValue::jsonb, @NewValue::jsonb, COALESCE(@CreatedAt, now()))
            RETURNING id;";
        return await _db.ExecuteScalarAsync<long>(sql, new
        {
            auditLog.UserId,
            auditLog.Action,
            auditLog.EntityType,
            auditLog.EntityId,
            auditLog.OldValue,
            auditLog.NewValue,
            CreatedAt = auditLog.CreatedAt == default ? (System.DateTimeOffset?)null : auditLog.CreatedAt
        }, transaction: transaction);
    }

    public async Task<bool> DeleteAsync(long id, IDbTransaction? transaction = null)
    {
        const string sql = "DELETE FROM public.audit_logs WHERE id = @Id;";
        var rows = await _db.ExecuteAsync(sql, new { Id = id }, transaction: transaction);
        return rows > 0;
    }
}
