using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using GovtLandService.Data.Entities;
using GovtLandService.Data.Interfaces;

namespace GovtLandService.Data.Repositories;

public class DistrictRepository : IDistrictRepository
{
    private readonly IDbConnection _db;

    public DistrictRepository(IDbConnection db)
    {
        _db = db;
    }

    private const string SelectColumns = "id AS Id, state_id AS StateId, name AS Name";

    public async Task<District?> GetByIdAsync(int id, IDbTransaction? transaction = null)
    {
        const string sql = $"SELECT {SelectColumns} FROM public.districts WHERE id = @Id;";
        return await _db.QuerySingleOrDefaultAsync<District>(sql, new { Id = id }, transaction: transaction);
    }

    public async Task<IEnumerable<District>> GetAllAsync(IDbTransaction? transaction = null)
    {
        const string sql = $"SELECT {SelectColumns} FROM public.districts ORDER BY name;";
        return await _db.QueryAsync<District>(sql, transaction: transaction);
    }

    public async Task<IEnumerable<District>> GetByStateIdAsync(int stateId, IDbTransaction? transaction = null)
    {
        const string sql = $"SELECT {SelectColumns} FROM public.districts WHERE state_id = @StateId ORDER BY name;";
        return await _db.QueryAsync<District>(sql, new { StateId = stateId }, transaction: transaction);
    }

    public async Task<District?> GetByStateAndNameAsync(int stateId, string name, IDbTransaction? transaction = null)
    {
        const string sql = $"SELECT {SelectColumns} FROM public.districts WHERE state_id = @StateId AND name = @Name;";
        return await _db.QuerySingleOrDefaultAsync<District>(sql, new { StateId = stateId, Name = name }, transaction: transaction);
    }

    public async Task<int> CreateAsync(District district, IDbTransaction? transaction = null)
    {
        const string sql = @"
            INSERT INTO public.districts (state_id, name)
            VALUES (@StateId, @Name)
            RETURNING id;";
        return await _db.ExecuteScalarAsync<int>(sql, district, transaction: transaction);
    }

    public async Task<bool> UpdateAsync(District district, IDbTransaction? transaction = null)
    {
        const string sql = @"
            UPDATE public.districts
            SET state_id = @StateId,
                name = @Name
            WHERE id = @Id;";
        var rows = await _db.ExecuteAsync(sql, district, transaction: transaction);
        return rows > 0;
    }

    public async Task<bool> DeleteAsync(int id, IDbTransaction? transaction = null)
    {
        const string sql = "DELETE FROM public.districts WHERE id = @Id;";
        var rows = await _db.ExecuteAsync(sql, new { Id = id }, transaction: transaction);
        return rows > 0;
    }
}
