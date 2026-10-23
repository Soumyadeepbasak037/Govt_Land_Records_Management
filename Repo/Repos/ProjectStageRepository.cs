using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using GovtLandService.Data.Entities;
using GovtLandService.Data.Interfaces;

namespace GovtLandService.Data.Repositories;

public class ProjectStageRepository : IProjectStageRepository
{
    private readonly IDbConnection _db;

    public ProjectStageRepository(IDbConnection db)
    {
        _db = db;
    }

    private const string SelectColumns = @"
        id AS Id,
        project_id AS ProjectId,
        stage AS Stage,
        status AS Status,
        assigned_to AS AssignedTo,
        started_at AS StartedAt,
        completed_at AS CompletedAt,
        remarks AS Remarks,
        created_at AS CreatedAt";

    public async Task<ProjectStage?> GetByIdAsync(long id, IDbTransaction? transaction = null)
    {
        const string sql = $"SELECT {SelectColumns} FROM public.project_stages WHERE id = @Id;";
        return await _db.QuerySingleOrDefaultAsync<ProjectStage>(sql, new { Id = id }, transaction: transaction);
    }

    public async Task<IEnumerable<ProjectStage>> GetByProjectIdAsync(long projectId, IDbTransaction? transaction = null)
    {
        const string sql = $"SELECT {SelectColumns} FROM public.project_stages WHERE project_id = @ProjectId ORDER BY id;";
        return await _db.QueryAsync<ProjectStage>(sql, new { ProjectId = projectId }, transaction: transaction);
    }

    public async Task<IEnumerable<ProjectStage>> GetByAssigneeIdAsync(long assignedTo, IDbTransaction? transaction = null)
    {
        const string sql = $"SELECT {SelectColumns} FROM public.project_stages WHERE assigned_to = @AssignedTo ORDER BY created_at DESC;";
        return await _db.QueryAsync<ProjectStage>(sql, new { AssignedTo = assignedTo }, transaction: transaction);
    }

    public async Task<long> CreateAsync(ProjectStage stage, IDbTransaction? transaction = null)
    {
        const string sql = @"
            INSERT INTO public.project_stages (project_id, stage, status, assigned_to, started_at, completed_at, remarks, created_at)
            VALUES (@ProjectId, @Stage, @Status, @AssignedTo, @StartedAt, @CompletedAt, @Remarks, COALESCE(@CreatedAt, now()))
            RETURNING id;";
        return await _db.ExecuteScalarAsync<long>(sql, new
        {
            stage.ProjectId,
            stage.Stage,
            stage.Status,
            stage.AssignedTo,
            stage.StartedAt,
            stage.CompletedAt,
            stage.Remarks,
            CreatedAt = stage.CreatedAt == default ? (System.DateTimeOffset?)null : stage.CreatedAt
        }, transaction: transaction);
    }

    public async Task<bool> UpdateAsync(ProjectStage stage, IDbTransaction? transaction = null)
    {
        const string sql = @"
            UPDATE public.project_stages
            SET project_id = @ProjectId,
                stage = @Stage,
                status = @Status,
                assigned_to = @AssignedTo,
                started_at = @StartedAt,
                completed_at = @CompletedAt,
                remarks = @Remarks
            WHERE id = @Id;";
        var rows = await _db.ExecuteAsync(sql, stage, transaction: transaction);
        return rows > 0;
    }

    public async Task<bool> UpdateStatusAsync(long id, string status, string? remarks = null, IDbTransaction? transaction = null)
    {
        const string sql = @"
            UPDATE public.project_stages
            SET status = @Status,
                started_at = CASE WHEN @Status = 'IN_PROGRESS' AND started_at IS NULL THEN now() ELSE started_at END,
                completed_at = CASE WHEN @Status = 'COMPLETED' THEN now() ELSE completed_at END,
                remarks = COALESCE(@Remarks, remarks)
            WHERE id = @Id;";
        var rows = await _db.ExecuteAsync(sql, new { Id = id, Status = status, Remarks = remarks }, transaction: transaction);
        return rows > 0;
    }

    public async Task<bool> DeleteAsync(long id, IDbTransaction? transaction = null)
    {
        const string sql = "DELETE FROM public.project_stages WHERE id = @Id;";
        var rows = await _db.ExecuteAsync(sql, new { Id = id }, transaction: transaction);
        return rows > 0;
    }
}
