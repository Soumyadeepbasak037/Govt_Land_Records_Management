using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using GovtLandService.Data.Entities;
using GovtLandService.Data.Interfaces;

namespace GovtLandService.Data.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly IDbConnection _db;

    public RoleRepository(IDbConnection db)
    {
        _db = db;
    }

    private const string SelectColumns = "id AS Id, name AS Name";

    public async Task<Role?> GetByIdAsync(int id, IDbTransaction? transaction = null)
    {
        const string sql = $"SELECT {SelectColumns} FROM public.roles WHERE id = @Id;";
        return await _db.QuerySingleOrDefaultAsync<Role>(sql, new { Id = id }, transaction: transaction);
    }

    public async Task<Role?> GetByNameAsync(string name, IDbTransaction? transaction = null)
    {
        const string sql = $"SELECT {SelectColumns} FROM public.roles WHERE name = @Name;";
        return await _db.QuerySingleOrDefaultAsync<Role>(sql, new { Name = name }, transaction: transaction);
    }

    public async Task<IEnumerable<Role>> GetAllAsync(IDbTransaction? transaction = null)
    {
        const string sql = $"SELECT {SelectColumns} FROM public.roles ORDER BY id;";
        return await _db.QueryAsync<Role>(sql, transaction: transaction);
    }

    public async Task<int> CreateAsync(Role role, IDbTransaction? transaction = null)
    {
        const string sql = @"
            INSERT INTO public.roles (name)
            VALUES (@Name)
            RETURNING id;";
        return await _db.ExecuteScalarAsync<int>(sql, role, transaction: transaction);
    }

    public async Task<bool> UpdateAsync(Role role, IDbTransaction? transaction = null)
    {
        const string sql = "UPDATE public.roles SET name = @Name WHERE id = @Id;";
        var rows = await _db.ExecuteAsync(sql, role, transaction: transaction);
        return rows > 0;
    }

    public async Task<bool> DeleteAsync(int id, IDbTransaction? transaction = null)
    {
        const string sql = "DELETE FROM public.roles WHERE id = @Id;";
        var rows = await _db.ExecuteAsync(sql, new { Id = id }, transaction: transaction);
        return rows > 0;
    }
}
