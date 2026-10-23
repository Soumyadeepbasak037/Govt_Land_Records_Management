using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using GovtLandService.Data.Entities;
using GovtLandService.Data.Interfaces;

namespace GovtLandService.Data.Repositories;

public class LandOwnerRepository : ILandOwnerRepository
{
    private readonly IDbConnection _db;

    public LandOwnerRepository(IDbConnection db)
    {
        _db = db;
    }

    private const string SelectColumns = @"
        lo.id AS Id,
        lo.name AS Name,
        lo.identifier_type AS IdentifierType,
        lo.identifier_hash AS IdentifierHash,
        lo.phone AS Phone,
        lo.created_at AS CreatedAt";

    public async Task<LandOwner?> GetByIdAsync(long id, IDbTransaction? transaction = null)
    {
        const string sql = $"SELECT {SelectColumns} FROM public.land_owners lo WHERE lo.id = @Id;";
        return await _db.QuerySingleOrDefaultAsync<LandOwner>(sql, new { Id = id }, transaction: transaction);
    }

    public async Task<IEnumerable<LandOwner>> GetAllAsync(IDbTransaction? transaction = null)
    {
        const string sql = $"SELECT {SelectColumns} FROM public.land_owners lo ORDER BY lo.name;";
        return await _db.QueryAsync<LandOwner>(sql, transaction: transaction);
    }

    public async Task<IEnumerable<LandOwner>> GetByParcelIdAsync(long parcelId, IDbTransaction? transaction = null)
    {
        const string sql = $@"
            SELECT {SelectColumns}
            FROM public.land_owners lo
            INNER JOIN public.parcel_owners po ON po.owner_id = lo.id
            WHERE po.parcel_id = @ParcelId
            ORDER BY lo.name;";
        return await _db.QueryAsync<LandOwner>(sql, new { ParcelId = parcelId }, transaction: transaction);
    }

    public async Task<long> CreateAsync(LandOwner owner, IDbTransaction? transaction = null)
    {
        const string sql = @"
            INSERT INTO public.land_owners (name, identifier_type, identifier_hash, phone, created_at)
            VALUES (@Name, @IdentifierType, @IdentifierHash, @Phone, COALESCE(@CreatedAt, now()))
            RETURNING id;";
        return await _db.ExecuteScalarAsync<long>(sql, new
        {
            owner.Name,
            owner.IdentifierType,
            owner.IdentifierHash,
            owner.Phone,
            CreatedAt = owner.CreatedAt == default ? (System.DateTimeOffset?)null : owner.CreatedAt
        }, transaction: transaction);
    }

    public async Task<bool> UpdateAsync(LandOwner owner, IDbTransaction? transaction = null)
    {
        const string sql = @"
            UPDATE public.land_owners
            SET name = @Name,
                identifier_type = @IdentifierType,
                identifier_hash = @IdentifierHash,
                phone = @Phone
            WHERE id = @Id;";
        var rows = await _db.ExecuteAsync(sql, owner, transaction: transaction);
        return rows > 0;
    }

    public async Task<bool> DeleteAsync(long id, IDbTransaction? transaction = null)
    {
        const string sql = "DELETE FROM public.land_owners WHERE id = @Id;";
        var rows = await _db.ExecuteAsync(sql, new { Id = id }, transaction: transaction);
        return rows > 0;
    }
}
