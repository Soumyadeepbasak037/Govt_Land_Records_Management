using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using GovtLandService.Data.Entities;

namespace GovtLandService.Data.Interfaces;

public interface IDocumentVersionRepository
{
    Task<DocumentVersion?> GetByIdAsync(long id, IDbTransaction? transaction = null);
    Task<IEnumerable<DocumentVersion>> GetByDocumentIdAsync(long documentId, IDbTransaction? transaction = null);
    Task<DocumentVersion?> GetByDocumentAndVersionAsync(long documentId, int versionNumber, IDbTransaction? transaction = null);
    Task<long> CreateAsync(DocumentVersion version, IDbTransaction? transaction = null);
    Task<bool> UpdateAsync(DocumentVersion version, IDbTransaction? transaction = null);
    Task<bool> DeleteAsync(long id, IDbTransaction? transaction = null);
}
