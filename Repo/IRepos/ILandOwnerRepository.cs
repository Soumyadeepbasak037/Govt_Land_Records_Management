using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using GovtLandService.Data.Entities;

namespace GovtLandService.Data.Interfaces;

public interface ILandOwnerRepository
{
    Task<LandOwner?> GetByIdAsync(long id, IDbTransaction? transaction = null);
    Task<IEnumerable<LandOwner>> GetAllAsync(IDbTransaction? transaction = null);
    Task<IEnumerable<LandOwner>> GetByParcelIdAsync(long parcelId, IDbTransaction? transaction = null);
    Task<long> CreateAsync(LandOwner owner, IDbTransaction? transaction = null);
    Task<bool> UpdateAsync(LandOwner owner, IDbTransaction? transaction = null);
    Task<bool> DeleteAsync(long id, IDbTransaction? transaction = null);
}
