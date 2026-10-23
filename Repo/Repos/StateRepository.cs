using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using GovtLandService.Data.Entities;
using GovtLandService.Data.Interfaces;

namespace GovtLandService.Data.Repositories;

public class StateRepository : IStateRepository
{
    private readonly IDbConnection _db;

    public StateRepository(IDbConnection db)
    {
        _db = db;
    }

    private const string SelectColumns = "id AS Id, name AS Name, code AS Code";

    public async Task<State?> GetByIdAsync(int id, IDbTransaction? transaction = null)
    {
        const string sql = $"SELECT {SelectColumns} FROM public.states WHERE id = @Id;";
        return await _db.QuerySingleOrDefaultAsync<State>(sql, new { Id = id }, transaction: transaction);
    }

    public async Task<State?> GetByCodeAsync(string code, IDbTransaction? transaction = null)
    {
        const string sql = $"SELECT {SelectColumns} FROM public.states WHERE code = @Code;";
        return await _db.QuerySingleOrDefaultAsync<State>(sql, new { Code = code }, transaction: transaction);
    }

    public async Task<State?> GetByNameAsync(string name, IDbTransaction? transaction = null)
    {
        const string sql = $"SELECT {SelectColumns} FROM public.states WHERE name = @Name;";
        return await _db.QuerySingleOrDefaultAsync<State>(sql, new { Name = name }, transaction: transaction);
    }

    public async Task<IEnumerable<State>> GetAllAsync(IDbTransaction? transaction = null)
    {
        const string sql = $"SELECT {SelectColumns} FROM public.states ORDER BY name;";
        return await _db.QueryAsync<State>(sql, transaction: transaction);
    }

    public async Task<int> CreateAsync(State state, IDbTransaction? transaction = null)
    {
        const string sql = @"
            INSERT INTO public.states (name, code)
            VALUES (@Name, @Code)
            RETURNING id;";
        return await _db.ExecuteScalarAsync<int>(sql, state, transaction: transaction);
    }

    public async Task<bool> UpdateAsync(State state, IDbTransaction? transaction = null)
    {
        const string sql = @"
            UPDATE public.states
            SET name = @Name,
                code = @Code
            WHERE id = @Id;";
        var rows = await _db.ExecuteAsync(sql, state, transaction: transaction);
        return rows > 0;
    }

    public async Task<bool> DeleteAsync(int id, IDbTransaction? transaction = null)
    {
        const string sql = "DELETE FROM public.states WHERE id = @Id;";
        var rows = await _db.ExecuteAsync(sql, new { Id = id }, transaction: transaction);
        return rows > 0;
    }
}
