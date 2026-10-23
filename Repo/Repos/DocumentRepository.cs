using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using GovtLandService.Data.Entities;
using GovtLandService.Data.Interfaces;

namespace GovtLandService.Data.Repositories;

public class DocumentRepository : IDocumentRepository
{
    private readonly IDbConnection _db;

    public DocumentRepository(IDbConnection db)
    {
        _db = db;
    }

    private const string SelectColumns = @"
        id AS Id,
        project_id AS ProjectId,
        parcel_id AS ParcelId,
        document_type AS DocumentType,
        title AS Title,
        storage_key AS StorageKey,
        uploaded_by AS UploadedBy,
        created_at AS CreatedAt";

    public async Task<Document?> GetByIdAsync(long id, IDbTransaction? transaction = null)
    {
        const string sql = $"SELECT {SelectColumns} FROM public.documents WHERE id = @Id;";
        return await _db.QuerySingleOrDefaultAsync<Document>(sql, new { Id = id }, transaction: transaction);
    }

    public async Task<IEnumerable<Document>> GetAllAsync(IDbTransaction? transaction = null)
    {
        const string sql = $"SELECT {SelectColumns} FROM public.documents ORDER BY created_at DESC;";
        return await _db.QueryAsync<Document>(sql, transaction: transaction);
    }

    public async Task<IEnumerable<Document>> GetByProjectIdAsync(long projectId, IDbTransaction? transaction = null)
    {
        const string sql = $"SELECT {SelectColumns} FROM public.documents WHERE project_id = @ProjectId ORDER BY created_at DESC;";
        return await _db.QueryAsync<Document>(sql, new { ProjectId = projectId }, transaction: transaction);
    }

    public async Task<IEnumerable<Document>> GetByParcelIdAsync(long parcelId, IDbTransaction? transaction = null)
    {
        const string sql = $"SELECT {SelectColumns} FROM public.documents WHERE parcel_id = @ParcelId ORDER BY created_at DESC;";
        return await _db.QueryAsync<Document>(sql, new { ParcelId = parcelId }, transaction: transaction);
    }

    public async Task<long> CreateAsync(Document document, IDbTransaction? transaction = null)
    {
        const string sql = @"
            INSERT INTO public.documents (project_id, parcel_id, document_type, title, storage_key, uploaded_by, created_at)
            VALUES (@ProjectId, @ParcelId, @DocumentType, @Title, @StorageKey, @UploadedBy, COALESCE(@CreatedAt, now()))
            RETURNING id;";
        return await _db.ExecuteScalarAsync<long>(sql, new
        {
            document.ProjectId,
            document.ParcelId,
            document.DocumentType,
            document.Title,
            document.StorageKey,
            document.UploadedBy,
            CreatedAt = document.CreatedAt == default ? (System.DateTimeOffset?)null : document.CreatedAt
        }, transaction: transaction);
    }

    public async Task<bool> UpdateAsync(Document document, IDbTransaction? transaction = null)
    {
        const string sql = @"
            UPDATE public.documents
            SET project_id = @ProjectId,
                parcel_id = @ParcelId,
                document_type = @DocumentType,
                title = @Title,
                storage_key = @StorageKey,
                uploaded_by = @UploadedBy
            WHERE id = @Id;";
        var rows = await _db.ExecuteAsync(sql, document, transaction: transaction);
        return rows > 0;
    }

    public async Task<bool> DeleteAsync(long id, IDbTransaction? transaction = null)
    {
        const string sql = "DELETE FROM public.documents WHERE id = @Id;";
        var rows = await _db.ExecuteAsync(sql, new { Id = id }, transaction: transaction);
        return rows > 0;
    }
}
