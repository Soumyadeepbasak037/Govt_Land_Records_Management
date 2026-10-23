using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using GovtLandService.Data.Entities;

namespace GovtLandService.Data.Interfaces;

public interface IRoleRepository
{
    Task<Role?> GetByIdAsync(int id, IDbTransaction? transaction = null);
    Task<Role?> GetByNameAsync(string name, IDbTransaction? transaction = null);
    Task<IEnumerable<Role>> GetAllAsync(IDbTransaction? transaction = null);
    Task<int> CreateAsync(Role role, IDbTransaction? transaction = null);
    Task<bool> UpdateAsync(Role role, IDbTransaction? transaction = null);
    Task<bool> DeleteAsync(int id, IDbTransaction? transaction = null);
}
