using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using GovtLandService.Data.Entities;

namespace GovtLandService.Data.Interfaces;

public interface IUserRoleRepository
{
    Task<IEnumerable<Role>> GetRolesByUserIdAsync(long userId, IDbTransaction? transaction = null);
    Task<IEnumerable<User>> GetUsersByRoleIdAsync(int roleId, IDbTransaction? transaction = null);
    Task<bool> ExistsAsync(long userId, int roleId, IDbTransaction? transaction = null);
    Task<bool> AssignRoleAsync(long userId, int roleId, IDbTransaction? transaction = null);
    Task<bool> RemoveRoleAsync(long userId, int roleId, IDbTransaction? transaction = null);
}
