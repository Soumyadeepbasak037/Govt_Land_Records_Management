using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using GovtLandService.Data.Entities;
using GovtLandService.Data.Interfaces;

namespace GovtLandService.Data.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly IDbConnection _db;

    public ProjectRepository(IDbConnection db)
    {
        _db = db;
    }

    private const string SelectColumns = @"
        id AS Id,
        name AS Name,
        description AS Description,
        project_type AS ProjectType,
        implementing_agency AS ImplementingAgency,
        state_id AS StateId,
        district_id AS DistrictId,
        total_required_area AS TotalRequiredArea,
        status AS Status,
        created_by AS CreatedBy,
        created_at AS CreatedAt,
        updated_at AS UpdatedAt,
        ST_AsGeoJSON(geometry) AS GeometryGeoJson";

    public async Task<Project?> GetByIdAsync(long id, IDbTransaction? transaction = null)
    {
        const string sql = $"SELECT {SelectColumns} FROM public.projects WHERE id = @Id;";
        return await _db.QuerySingleOrDefaultAsync<Project>(sql, new { Id = id }, transaction: transaction);
    }

    public async Task<IEnumerable<Project>> GetAllAsync(IDbTransaction? transaction = null)
    {
        const string sql = $"SELECT {SelectColumns} FROM public.projects ORDER BY created_at DESC;";
        return await _db.QueryAsync<Project>(sql, transaction: transaction);
    }

    public async Task<IEnumerable<Project>> GetByStateIdAsync(int stateId, IDbTransaction? transaction = null)
    {
        const string sql = $"SELECT {SelectColumns} FROM public.projects WHERE state_id = @StateId ORDER BY created_at DESC;";
        return await _db.QueryAsync<Project>(sql, new { StateId = stateId }, transaction: transaction);
    }

    public async Task<IEnumerable<Project>> GetByDistrictIdAsync(int districtId, IDbTransaction? transaction = null)
    {
        const string sql = $"SELECT {SelectColumns} FROM public.projects WHERE district_id = @DistrictId ORDER BY created_at DESC;";
        return await _db.QueryAsync<Project>(sql, new { DistrictId = districtId }, transaction: transaction);
    }

    public async Task<IEnumerable<Project>> GetByCreatorIdAsync(long createdBy, IDbTransaction? transaction = null)
    {
        const string sql = $"SELECT {SelectColumns} FROM public.projects WHERE created_by = @CreatedBy ORDER BY created_at DESC;";
        return await _db.QueryAsync<Project>(sql, new { CreatedBy = createdBy }, transaction: transaction);
    }

    public async Task<long> CreateAsync(Project project, IDbTransaction? transaction = null)
    {
        const string sql = @"
            INSERT INTO public.projects (
                name, description, project_type, implementing_agency,
                state_id, district_id, total_required_area, status,
                created_by, created_at, updated_at, geometry
            )
            VALUES (
                @Name, @Description, @ProjectType, @ImplementingAgency,
                @StateId, @DistrictId, @TotalRequiredArea, COALESCE(@Status, 'DRAFT'),
                @CreatedBy, now(), now(),
                COALESCE(@Geometry, CASE WHEN @GeometryGeoJson IS NOT NULL THEN ST_SetSRID(ST_Multi(ST_GeomFromGeoJSON(@GeometryGeoJson)), 4326) ELSE NULL END)
            )
            RETURNING id;";
        return await _db.ExecuteScalarAsync<long>(sql, project, transaction: transaction);
    }

    public async Task<bool> UpdateAsync(Project project, IDbTransaction? transaction = null)
    {
        const string sql = @"
            UPDATE public.projects
            SET name = @Name,
                description = @Description,
                project_type = @ProjectType,
                implementing_agency = @ImplementingAgency,
                state_id = @StateId,
                district_id = @DistrictId,
                total_required_area = @TotalRequiredArea,
                status = @Status,
                geometry = COALESCE(@Geometry, CASE WHEN @GeometryGeoJson IS NOT NULL THEN ST_SetSRID(ST_Multi(ST_GeomFromGeoJSON(@GeometryGeoJson)), 4326) ELSE geometry END),
                updated_at = now()
            WHERE id = @Id;";
        var rows = await _db.ExecuteAsync(sql, project, transaction: transaction);
        return rows > 0;
    }

    public async Task<bool> UpdateStatusAsync(long id, string status, IDbTransaction? transaction = null)
    {
        const string sql = @"
            UPDATE public.projects
            SET status = @Status,
                updated_at = now()
            WHERE id = @Id;";
        var rows = await _db.ExecuteAsync(sql, new { Id = id, Status = status }, transaction: transaction);
        return rows > 0;
    }

    public async Task<bool> DeleteAsync(long id, IDbTransaction? transaction = null)
    {
        const string sql = "DELETE FROM public.projects WHERE id = @Id;";
        var rows = await _db.ExecuteAsync(sql, new { Id = id }, transaction: transaction);
        return rows > 0;
    }
}
