using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using GovtLandService.Data.Entities;
using GovtLandService.Data.Interfaces;

namespace GovtLandService.Data.Repositories;

public class VillageRepository : IVillageRepository
{
    private readonly IDbConnection _db;

    public VillageRepository(IDbConnection db)
    {
        _db = db;
    }

    private const string SelectColumns = "id AS Id, district_id AS DistrictId, name AS Name";

    public async Task<Village?> GetByIdAsync(long id, IDbTransaction? transaction = null)
    {
        const string sql = $"SELECT {SelectColumns} FROM public.villages WHERE id = @Id;";
        return await _db.QuerySingleOrDefaultAsync<Village>(sql, new { Id = id }, transaction: transaction);
    }

    public async Task<IEnumerable<Village>> GetAllAsync(IDbTransaction? transaction = null)
    {
        const string sql = $"SELECT {SelectColumns} FROM public.villages ORDER BY name;";
        return await _db.QueryAsync<Village>(sql, transaction: transaction);
    }

    public async Task<IEnumerable<Village>> GetByDistrictIdAsync(int districtId, IDbTransaction? transaction = null)
    {
        const string sql = $"SELECT {SelectColumns} FROM public.villages WHERE district_id = @DistrictId ORDER BY name;";
        return await _db.QueryAsync<Village>(sql, new { DistrictId = districtId }, transaction: transaction);
    }

    public async Task<Village?> GetByDistrictAndNameAsync(int districtId, string name, IDbTransaction? transaction = null)
    {
        const string sql = $"SELECT {SelectColumns} FROM public.villages WHERE district_id = @DistrictId AND name = @Name;";
        return await _db.QuerySingleOrDefaultAsync<Village>(sql, new { DistrictId = districtId, Name = name }, transaction: transaction);
    }

    public async Task<long> CreateAsync(Village village, IDbTransaction? transaction = null)
    {
        const string sql = @"
            INSERT INTO public.villages (district_id, name)
            VALUES (@DistrictId, @Name)
            RETURNING id;";
        return await _db.ExecuteScalarAsync<long>(sql, village, transaction: transaction);
    }

    public async Task<bool> UpdateAsync(Village village, IDbTransaction? transaction = null)
    {
        const string sql = @"
            UPDATE public.villages
            SET district_id = @DistrictId,
                name = @Name
            WHERE id = @Id;";
        var rows = await _db.ExecuteAsync(sql, village, transaction: transaction);
        return rows > 0;
    }

    public async Task<bool> DeleteAsync(long id, IDbTransaction? transaction = null)
    {
        const string sql = "DELETE FROM public.villages WHERE id = @Id;";
        var rows = await _db.ExecuteAsync(sql, new { Id = id }, transaction: transaction);
        return rows > 0;
    }
}
