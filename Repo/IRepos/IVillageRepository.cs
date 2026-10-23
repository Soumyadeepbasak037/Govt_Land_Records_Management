using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using GovtLandService.Data.Entities;

namespace GovtLandService.Data.Interfaces;

public interface IVillageRepository
{
    Task<Village?> GetByIdAsync(long id, IDbTransaction? transaction = null);
    Task<IEnumerable<Village>> GetAllAsync(IDbTransaction? transaction = null);
    Task<IEnumerable<Village>> GetByDistrictIdAsync(int districtId, IDbTransaction? transaction = null);
    Task<Village?> GetByDistrictAndNameAsync(int districtId, string name, IDbTransaction? transaction = null);
    Task<long> CreateAsync(Village village, IDbTransaction? transaction = null);
    Task<bool> UpdateAsync(Village village, IDbTransaction? transaction = null);
    Task<bool> DeleteAsync(long id, IDbTransaction? transaction = null);
}
