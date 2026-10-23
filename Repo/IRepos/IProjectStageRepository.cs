using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using GovtLandService.Data.Entities;

namespace GovtLandService.Data.Interfaces;

public interface IProjectStageRepository
{
    Task<ProjectStage?> GetByIdAsync(long id, IDbTransaction? transaction = null);
    Task<IEnumerable<ProjectStage>> GetByProjectIdAsync(long projectId, IDbTransaction? transaction = null);
    Task<IEnumerable<ProjectStage>> GetByAssigneeIdAsync(long assignedTo, IDbTransaction? transaction = null);
    Task<long> CreateAsync(ProjectStage stage, IDbTransaction? transaction = null);
    Task<bool> UpdateAsync(ProjectStage stage, IDbTransaction? transaction = null);
    Task<bool> UpdateStatusAsync(long id, string status, string? remarks = null, IDbTransaction? transaction = null);
    Task<bool> DeleteAsync(long id, IDbTransaction? transaction = null);
}
