using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using GovtLandService.Data.Entities;

namespace GovtLandService.Data.Interfaces;

public interface ILandParcelRepository
{
    Task<LandParcel?> GetByIdAsync(long id, IDbTransaction? transaction = null);
    Task<IEnumerable<LandParcel>> GetAllAsync(IDbTransaction? transaction = null);
    Task<IEnumerable<LandParcel>> GetByProjectIdAsync(long projectId, IDbTransaction? transaction = null);
    Task<IEnumerable<LandParcel>> GetByVillageIdAsync(long villageId, IDbTransaction? transaction = null);
    Task<LandParcel?> GetByProjectAndParcelNumberAsync(long projectId, string parcelNumber, IDbTransaction? transaction = null);
    Task<long> CreateAsync(LandParcel parcel, IDbTransaction? transaction = null);
    Task<bool> UpdateAsync(LandParcel parcel, IDbTransaction? transaction = null);
    Task<bool> UpdateAcquisitionStatusAsync(long id, string acquisitionStatus, IDbTransaction? transaction = null);
    Task<bool> DeleteAsync(long id, IDbTransaction? transaction = null);
}
