using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using GovtLandService.Data.Entities;

namespace GovtLandService.Data.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(long id, IDbTransaction? transaction = null);
    Task<User?> GetByEmailAsync(string email, IDbTransaction? transaction = null);
    Task<IEnumerable<User>> GetAllAsync(IDbTransaction? transaction = null);
    Task<long> CreateAsync(User user, IDbTransaction? transaction = null);
    Task<bool> UpdateAsync(User user, IDbTransaction? transaction = null);
    Task<bool> DeleteAsync(long id, IDbTransaction? transaction = null);
}
