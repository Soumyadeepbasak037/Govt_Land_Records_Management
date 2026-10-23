using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using GovtLandService.Data.Entities;
using GovtLandService.Data.Interfaces;

namespace GovtLandService.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IDbConnection _db;

    public UserRepository(IDbConnection db)
    {
        _db = db;
    }

    private const string SelectColumns = @"
        id AS Id,
        name AS Name,
        email AS Email,
        password_hash AS PasswordHash,
        phone AS Phone,
        is_active AS IsActive,
        created_at AS CreatedAt,
        updated_at AS UpdatedAt";

    public async Task<User?> GetByIdAsync(long id, IDbTransaction? transaction = null)
    {
        const string sql = $"SELECT {SelectColumns} FROM public.users WHERE id = @Id;";
        return await _db.QuerySingleOrDefaultAsync<User>(sql, new { Id = id }, transaction: transaction);
    }

    public async Task<User?> GetByEmailAsync(string email, IDbTransaction? transaction = null)
    {
        const string sql = $"SELECT {SelectColumns} FROM public.users WHERE email = @Email;";
        return await _db.QuerySingleOrDefaultAsync<User>(sql, new { Email = email }, transaction: transaction);
    }

    public async Task<IEnumerable<User>> GetAllAsync(IDbTransaction? transaction = null)
    {
        const string sql = $"SELECT {SelectColumns} FROM public.users ORDER BY created_at DESC;";
        return await _db.QueryAsync<User>(sql, transaction: transaction);
    }

    public async Task<long> CreateAsync(User user, IDbTransaction? transaction = null)
    {
        const string sql = @"
            INSERT INTO public.users (name, email, password_hash, phone, is_active, created_at, updated_at)
            VALUES (@Name, @Email, @PasswordHash, @Phone, @IsActive, now(), now())
            RETURNING id;";
        return await _db.ExecuteScalarAsync<long>(sql, user, transaction: transaction);
    }

    public async Task<bool> UpdateAsync(User user, IDbTransaction? transaction = null)
    {
        const string sql = @"
            UPDATE public.users
            SET name = @Name,
                email = @Email,
                password_hash = @PasswordHash,
                phone = @Phone,
                is_active = @IsActive,
                updated_at = now()
            WHERE id = @Id;";
        var rows = await _db.ExecuteAsync(sql, user, transaction: transaction);
        return rows > 0;
    }

    public async Task<bool> DeleteAsync(long id, IDbTransaction? transaction = null)
    {
        const string sql = "DELETE FROM public.users WHERE id = @Id;";
        var rows = await _db.ExecuteAsync(sql, new { Id = id }, transaction: transaction);
        return rows > 0;
    }
}
