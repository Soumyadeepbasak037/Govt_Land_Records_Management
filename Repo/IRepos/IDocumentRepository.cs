using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using GovtLandService.Data.Entities;

namespace GovtLandService.Data.Interfaces;

public interface IDocumentRepository
{
    Task<Document?> GetByIdAsync(long id, IDbTransaction? transaction = null);
    Task<IEnumerable<Document>> GetAllAsync(IDbTransaction? transaction = null);
    Task<IEnumerable<Document>> GetByProjectIdAsync(long projectId, IDbTransaction? transaction = null);
    Task<IEnumerable<Document>> GetByParcelIdAsync(long parcelId, IDbTransaction? transaction = null);
    Task<long> CreateAsync(Document document, IDbTransaction? transaction = null);
    Task<bool> UpdateAsync(Document document, IDbTransaction? transaction = null);
    Task<bool> DeleteAsync(long id, IDbTransaction? transaction = null);
}
