using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using GovtLandService.Data.Entities;

namespace GovtLandService.Data.Interfaces;

public interface ICompensationRepository
{
    Task<Compensation?> GetByIdAsync(long id, IDbTransaction? transaction = null);
    Task<IEnumerable<Compensation>> GetAllAsync(IDbTransaction? transaction = null);
    Task<IEnumerable<Compensation>> GetByParcelIdAsync(long parcelId, IDbTransaction? transaction = null);
    Task<IEnumerable<Compensation>> GetByOwnerIdAsync(long ownerId, IDbTransaction? transaction = null);
    Task<long> CreateAsync(Compensation compensation, IDbTransaction? transaction = null);
    Task<bool> UpdateAsync(Compensation compensation, IDbTransaction? transaction = null);
    Task<bool> DeleteAsync(long id, IDbTransaction? transaction = null);
}
