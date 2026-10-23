using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using GovtLandService.Data.Entities;
using GovtLandService.Data.Interfaces;

namespace GovtLandService.Data.Repositories;

public class UserRoleRepository : IUserRoleRepository
{
    private readonly IDbConnection _db;

    public UserRoleRepository(IDbConnection db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Role>> GetRolesByUserIdAsync(long userId, IDbTransaction? transaction = null)
    {
        const string sql = @"
            SELECT r.id AS Id, r.name AS Name
            FROM public.roles r
            INNER JOIN public.user_roles ur ON ur.role_id = r.id
            WHERE ur.user_id = @UserId
            ORDER BY r.name;";
        return await _db.QueryAsync<Role>(sql, new { UserId = userId }, transaction: transaction);
    }

    public async Task<IEnumerable<User>> GetUsersByRoleIdAsync(int roleId, IDbTransaction? transaction = null)
    {
        const string sql = @"
            SELECT
                u.id AS Id,
                u.name AS Name,
                u.email AS Email,
                u.password_hash AS PasswordHash,
                u.phone AS Phone,
                u.is_active AS IsActive,
                u.created_at AS CreatedAt,
                u.updated_at AS UpdatedAt
            FROM public.users u
            INNER JOIN public.user_roles ur ON ur.user_id = u.id
            WHERE ur.role_id = @RoleId
            ORDER BY u.name;";
        return await _db.QueryAsync<User>(sql, new { RoleId = roleId }, transaction: transaction);
    }

    public async Task<bool> ExistsAsync(long userId, int roleId, IDbTransaction? transaction = null)
    {
        const string sql = "SELECT EXISTS(SELECT 1 FROM public.user_roles WHERE user_id = @UserId AND role_id = @RoleId);";
        return await _db.ExecuteScalarAsync<bool>(sql, new { UserId = userId, RoleId = roleId }, transaction: transaction);
    }

    public async Task<bool> AssignRoleAsync(long userId, int roleId, IDbTransaction? transaction = null)
    {
        const string sql = @"
            INSERT INTO public.user_roles (user_id, role_id)
            VALUES (@UserId, @RoleId)
            ON CONFLICT (user_id, role_id) DO NOTHING;";
        var rows = await _db.ExecuteAsync(sql, new { UserId = userId, RoleId = roleId }, transaction: transaction);
        return rows > 0;
    }

    public async Task<bool> RemoveRoleAsync(long userId, int roleId, IDbTransaction? transaction = null)
    {
        const string sql = "DELETE FROM public.user_roles WHERE user_id = @UserId AND role_id = @RoleId;";
        var rows = await _db.ExecuteAsync(sql, new { UserId = userId, RoleId = roleId }, transaction: transaction);
        return rows > 0;
    }
}
