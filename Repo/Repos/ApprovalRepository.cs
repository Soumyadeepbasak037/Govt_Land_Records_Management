using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using GovtLandService.Data.Entities;
using GovtLandService.Data.Interfaces;

namespace GovtLandService.Data.Repositories;

public class ApprovalRepository : IApprovalRepository
{
    private readonly IDbConnection _db;

    public ApprovalRepository(IDbConnection db)
    {
        _db = db;
    }

    private const string SelectColumns = @"
        id AS Id,
        project_id AS ProjectId,
        stage_id AS StageId,
        approved_by AS ApprovedBy,
        decision AS Decision,
        remarks AS Remarks,
        decided_at AS DecidedAt";

    public async Task<Approval?> GetByIdAsync(long id, IDbTransaction? transaction = null)
    {
        const string sql = $"SELECT {SelectColumns} FROM public.approvals WHERE id = @Id;";
        return await _db.QuerySingleOrDefaultAsync<Approval>(sql, new { Id = id }, transaction: transaction);
    }

    public async Task<IEnumerable<Approval>> GetAllAsync(IDbTransaction? transaction = null)
    {
        const string sql = $"SELECT {SelectColumns} FROM public.approvals ORDER BY decided_at DESC;";
        return await _db.QueryAsync<Approval>(sql, transaction: transaction);
    }

    public async Task<IEnumerable<Approval>> GetByProjectIdAsync(long projectId, IDbTransaction? transaction = null)
    {
        const string sql = $"SELECT {SelectColumns} FROM public.approvals WHERE project_id = @ProjectId ORDER BY decided_at DESC;";
        return await _db.QueryAsync<Approval>(sql, new { ProjectId = projectId }, transaction: transaction);
    }

    public async Task<IEnumerable<Approval>> GetByStageIdAsync(long stageId, IDbTransaction? transaction = null)
    {
        const string sql = $"SELECT {SelectColumns} FROM public.approvals WHERE stage_id = @StageId ORDER BY decided_at DESC;";
        return await _db.QueryAsync<Approval>(sql, new { StageId = stageId }, transaction: transaction);
    }

    public async Task<IEnumerable<Approval>> GetByApproverIdAsync(long approvedBy, IDbTransaction? transaction = null)
    {
        const string sql = $"SELECT {SelectColumns} FROM public.approvals WHERE approved_by = @ApprovedBy ORDER BY decided_at DESC;";
        return await _db.QueryAsync<Approval>(sql, new { ApprovedBy = approvedBy }, transaction: transaction);
    }

    public async Task<long> CreateAsync(Approval approval, IDbTransaction? transaction = null)
    {
        const string sql = @"
            INSERT INTO public.approvals (project_id, stage_id, approved_by, decision, remarks, decided_at)
            VALUES (@ProjectId, @StageId, @ApprovedBy, @Decision, @Remarks, COALESCE(@DecidedAt, now()))
            RETURNING id;";
        return await _db.ExecuteScalarAsync<long>(sql, new
        {
            approval.ProjectId,
            approval.StageId,
            approval.ApprovedBy,
            approval.Decision,
            approval.Remarks,
            DecidedAt = approval.DecidedAt == default ? (System.DateTimeOffset?)null : approval.DecidedAt
        }, transaction: transaction);
    }

    public async Task<bool> UpdateAsync(Approval approval, IDbTransaction? transaction = null)
    {
        const string sql = @"
            UPDATE public.approvals
            SET project_id = @ProjectId,
                stage_id = @StageId,
                approved_by = @ApprovedBy,
                decision = @Decision,
                remarks = @Remarks,
                decided_at = @DecidedAt
            WHERE id = @Id;";
        var rows = await _db.ExecuteAsync(sql, approval, transaction: transaction);
        return rows > 0;
    }

    public async Task<bool> DeleteAsync(long id, IDbTransaction? transaction = null)
    {
        const string sql = "DELETE FROM public.approvals WHERE id = @Id;";
        var rows = await _db.ExecuteAsync(sql, new { Id = id }, transaction: transaction);
        return rows > 0;
    }
}
