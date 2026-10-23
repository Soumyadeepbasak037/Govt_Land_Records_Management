using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using GovtLandService.Data.Entities;

namespace GovtLandService.Data.Interfaces;

public interface IProjectRepository
{
    Task<Project?> GetByIdAsync(long id, IDbTransaction? transaction = null);
    Task<IEnumerable<Project>> GetAllAsync(IDbTransaction? transaction = null);
    Task<IEnumerable<Project>> GetByStateIdAsync(int stateId, IDbTransaction? transaction = null);
    Task<IEnumerable<Project>> GetByDistrictIdAsync(int districtId, IDbTransaction? transaction = null);
    Task<IEnumerable<Project>> GetByCreatorIdAsync(long createdBy, IDbTransaction? transaction = null);
    Task<long> CreateAsync(Project project, IDbTransaction? transaction = null);
    Task<bool> UpdateAsync(Project project, IDbTransaction? transaction = null);
    Task<bool> UpdateStatusAsync(long id, string status, IDbTransaction? transaction = null);
    Task<bool> DeleteAsync(long id, IDbTransaction? transaction = null);
}
