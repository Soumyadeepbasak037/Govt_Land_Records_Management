using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using GovtLandService.Data.Entities;

namespace GovtLandService.Data.Interfaces;

public interface IApprovalRepository
{
    Task<Approval?> GetByIdAsync(long id, IDbTransaction? transaction = null);
    Task<IEnumerable<Approval>> GetAllAsync(IDbTransaction? transaction = null);
    Task<IEnumerable<Approval>> GetByProjectIdAsync(long projectId, IDbTransaction? transaction = null);
    Task<IEnumerable<Approval>> GetByStageIdAsync(long stageId, IDbTransaction? transaction = null);
    Task<IEnumerable<Approval>> GetByApproverIdAsync(long approvedBy, IDbTransaction? transaction = null);
    Task<long> CreateAsync(Approval approval, IDbTransaction? transaction = null);
    Task<bool> UpdateAsync(Approval approval, IDbTransaction? transaction = null);
    Task<bool> DeleteAsync(long id, IDbTransaction? transaction = null);
}
