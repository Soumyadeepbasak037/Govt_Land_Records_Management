using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using GovtLandService.Data.Entities;

namespace GovtLandService.Data.Interfaces;

public interface IRehabilitationRepository
{
    Task<Rehabilitation?> GetByIdAsync(long id, IDbTransaction? transaction = null);
    Task<IEnumerable<Rehabilitation>> GetAllAsync(IDbTransaction? transaction = null);
    Task<IEnumerable<Rehabilitation>> GetByProjectIdAsync(long projectId, IDbTransaction? transaction = null);
    Task<IEnumerable<Rehabilitation>> GetByOwnerIdAsync(long ownerId, IDbTransaction? transaction = null);
    Task<long> CreateAsync(Rehabilitation rehabilitation, IDbTransaction? transaction = null);
    Task<bool> UpdateAsync(Rehabilitation rehabilitation, IDbTransaction? transaction = null);
    Task<bool> DeleteAsync(long id, IDbTransaction? transaction = null);
}
