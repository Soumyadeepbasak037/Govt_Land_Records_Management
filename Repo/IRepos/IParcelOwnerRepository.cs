using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using GovtLandService.Data.Entities;

namespace GovtLandService.Data.Interfaces;

public interface IParcelOwnerRepository
{
    Task<ParcelOwner?> GetAsync(long parcelId, long ownerId, IDbTransaction? transaction = null);
    Task<IEnumerable<ParcelOwner>> GetByParcelIdAsync(long parcelId, IDbTransaction? transaction = null);
    Task<IEnumerable<ParcelOwner>> GetByOwnerIdAsync(long ownerId, IDbTransaction? transaction = null);
    Task<bool> AddAsync(ParcelOwner parcelOwner, IDbTransaction? transaction = null);
    Task<bool> UpdateShareAsync(ParcelOwner parcelOwner, IDbTransaction? transaction = null);
    Task<bool> RemoveAsync(long parcelId, long ownerId, IDbTransaction? transaction = null);
}
