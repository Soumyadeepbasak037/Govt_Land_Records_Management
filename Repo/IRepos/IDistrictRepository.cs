using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using GovtLandService.Data.Entities;

namespace GovtLandService.Data.Interfaces;

public interface IDistrictRepository
{
    Task<District?> GetByIdAsync(int id, IDbTransaction? transaction = null);
    Task<IEnumerable<District>> GetAllAsync(IDbTransaction? transaction = null);
    Task<IEnumerable<District>> GetByStateIdAsync(int stateId, IDbTransaction? transaction = null);
    Task<District?> GetByStateAndNameAsync(int stateId, string name, IDbTransaction? transaction = null);
    Task<int> CreateAsync(District district, IDbTransaction? transaction = null);
    Task<bool> UpdateAsync(District district, IDbTransaction? transaction = null);
    Task<bool> DeleteAsync(int id, IDbTransaction? transaction = null);
}
