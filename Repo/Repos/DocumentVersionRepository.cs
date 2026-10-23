using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using GovtLandService.Data.Entities;
using GovtLandService.Data.Interfaces;

namespace GovtLandService.Data.Repositories;

public class DocumentVersionRepository : IDocumentVersionRepository
{
    private readonly IDbConnection _db;

    public DocumentVersionRepository(IDbConnection db)
    {
        _db = db;
    }

    private const string SelectColumns = @"
        id AS Id,
        document_id AS DocumentId,
        version_number AS VersionNumber,
        storage_key AS StorageKey,
        uploaded_by AS UploadedBy,
        created_at AS CreatedAt";

    public async Task<DocumentVersion?> GetByIdAsync(long id, IDbTransaction? transaction = null)
    {
        const string sql = $"SELECT {SelectColumns} FROM public.document_versions WHERE id = @Id;";
        return await _db.QuerySingleOrDefaultAsync<DocumentVersion>(sql, new { Id = id }, transaction: transaction);
    }

    public async Task<IEnumerable<DocumentVersion>> GetByDocumentIdAsync(long documentId, IDbTransaction? transaction = null)
    {
        const string sql = $"SELECT {SelectColumns} FROM public.document_versions WHERE document_id = @DocumentId ORDER BY version_number DESC;";
        return await _db.QueryAsync<DocumentVersion>(sql, new { DocumentId = documentId }, transaction: transaction);
    }

    public async Task<DocumentVersion?> GetByDocumentAndVersionAsync(long documentId, int versionNumber, IDbTransaction? transaction = null)
    {
        const string sql = $"SELECT {SelectColumns} FROM public.document_versions WHERE document_id = @DocumentId AND version_number = @VersionNumber;";
        return await _db.QuerySingleOrDefaultAsync<DocumentVersion>(sql, new { DocumentId = documentId, VersionNumber = versionNumber }, transaction: transaction);
    }

    public async Task<long> CreateAsync(DocumentVersion version, IDbTransaction? transaction = null)
    {
        const string sql = @"
            INSERT INTO public.document_versions (document_id, version_number, storage_key, uploaded_by, created_at)
            VALUES (@DocumentId, @VersionNumber, @StorageKey, @UploadedBy, COALESCE(@CreatedAt, now()))
            RETURNING id;";
        return await _db.ExecuteScalarAsync<long>(sql, new
        {
            version.DocumentId,
            version.VersionNumber,
            version.StorageKey,
            version.UploadedBy,
            CreatedAt = version.CreatedAt == default ? (System.DateTimeOffset?)null : version.CreatedAt
        }, transaction: transaction);
    }

    public async Task<bool> UpdateAsync(DocumentVersion version, IDbTransaction? transaction = null)
    {
        const string sql = @"
            UPDATE public.document_versions
            SET document_id = @DocumentId,
                version_number = @VersionNumber,
                storage_key = @StorageKey,
                uploaded_by = @UploadedBy
            WHERE id = @Id;";
        var rows = await _db.ExecuteAsync(sql, version, transaction: transaction);
        return rows > 0;
    }

    public async Task<bool> DeleteAsync(long id, IDbTransaction? transaction = null)
    {
        const string sql = "DELETE FROM public.document_versions WHERE id = @Id;";
        var rows = await _db.ExecuteAsync(sql, new { Id = id }, transaction: transaction);
        return rows > 0;
    }
}
