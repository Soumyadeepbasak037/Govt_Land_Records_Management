using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using GovtLandService.Data.Entities;
using GovtLandService.Data.Interfaces;

namespace GovtLandService.Data.Repositories;

public class RehabilitationRepository : IRehabilitationRepository
{
    private readonly IDbConnection _db;

    public RehabilitationRepository(IDbConnection db)
    {
        _db = db;
    }

    private const string SelectColumns = @"
        id AS Id,
        project_id AS ProjectId,
        owner_id AS OwnerId,
        affected AS Affected,
        displaced AS Displaced,
        assistance_type AS AssistanceType,
        assistance_status AS AssistanceStatus,
        completed_at AS CompletedAt,
        remarks AS Remarks";

    public async Task<Rehabilitation?> GetByIdAsync(long id, IDbTransaction? transaction = null)
    {
        const string sql = $"SELECT {SelectColumns} FROM public.rehabilitation WHERE id = @Id;";
        return await _db.QuerySingleOrDefaultAsync<Rehabilitation>(sql, new { Id = id }, transaction: transaction);
    }

    public async Task<IEnumerable<Rehabilitation>> GetAllAsync(IDbTransaction? transaction = null)
    {
        const string sql = $"SELECT {SelectColumns} FROM public.rehabilitation ORDER BY id DESC;";
        return await _db.QueryAsync<Rehabilitation>(sql, transaction: transaction);
    }

    public async Task<IEnumerable<Rehabilitation>> GetByProjectIdAsync(long projectId, IDbTransaction? transaction = null)
    {
        const string sql = $"SELECT {SelectColumns} FROM public.rehabilitation WHERE project_id = @ProjectId ORDER BY id;";
        return await _db.QueryAsync<Rehabilitation>(sql, new { ProjectId = projectId }, transaction: transaction);
    }

    public async Task<IEnumerable<Rehabilitation>> GetByOwnerIdAsync(long ownerId, IDbTransaction? transaction = null)
    {
        const string sql = $"SELECT {SelectColumns} FROM public.rehabilitation WHERE owner_id = @OwnerId ORDER BY id;";
        return await _db.QueryAsync<Rehabilitation>(sql, new { OwnerId = ownerId }, transaction: transaction);
    }

    public async Task<long> CreateAsync(Rehabilitation rehabilitation, IDbTransaction? transaction = null)
    {
        const string sql = @"
            INSERT INTO public.rehabilitation (project_id, owner_id, affected, displaced, assistance_type, assistance_status, completed_at, remarks)
            VALUES (@ProjectId, @OwnerId, @Affected, @Displaced, @AssistanceType, @AssistanceStatus, @CompletedAt, @Remarks)
            RETURNING id;";
        return await _db.ExecuteScalarAsync<long>(sql, rehabilitation, transaction: transaction);
    }

    public async Task<bool> UpdateAsync(Rehabilitation rehabilitation, IDbTransaction? transaction = null)
    {
        const string sql = @"
            UPDATE public.rehabilitation
            SET project_id = @ProjectId,
                owner_id = @OwnerId,
                affected = @Affected,
                displaced = @Displaced,
                assistance_type = @AssistanceType,
                assistance_status = @AssistanceStatus,
                completed_at = @CompletedAt,
                remarks = @Remarks
            WHERE id = @Id;";
        var rows = await _db.ExecuteAsync(sql, rehabilitation, transaction: transaction);
        return rows > 0;
    }

    public async Task<bool> DeleteAsync(long id, IDbTransaction? transaction = null)
    {
        const string sql = "DELETE FROM public.rehabilitation WHERE id = @Id;";
        var rows = await _db.ExecuteAsync(sql, new { Id = id }, transaction: transaction);
        return rows > 0;
    }
}
