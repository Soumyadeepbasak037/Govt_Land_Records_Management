using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using GovtLandService.Data.Entities;
using GovtLandService.Data.Interfaces;

namespace GovtLandService.Data.Repositories;

public class ParcelOwnerRepository : IParcelOwnerRepository
{
    private readonly IDbConnection _db;

    public ParcelOwnerRepository(IDbConnection db)
    {
        _db = db;
    }

    private const string SelectColumns = @"
        parcel_id AS ParcelId,
        owner_id AS OwnerId,
        ownership_share AS OwnershipShare";

    public async Task<ParcelOwner?> GetAsync(long parcelId, long ownerId, IDbTransaction? transaction = null)
    {
        const string sql = $"SELECT {SelectColumns} FROM public.parcel_owners WHERE parcel_id = @ParcelId AND owner_id = @OwnerId;";
        return await _db.QuerySingleOrDefaultAsync<ParcelOwner>(sql, new { ParcelId = parcelId, OwnerId = ownerId }, transaction: transaction);
    }

    public async Task<IEnumerable<ParcelOwner>> GetByParcelIdAsync(long parcelId, IDbTransaction? transaction = null)
    {
        const string sql = $"SELECT {SelectColumns} FROM public.parcel_owners WHERE parcel_id = @ParcelId;";
        return await _db.QueryAsync<ParcelOwner>(sql, new { ParcelId = parcelId }, transaction: transaction);
    }

    public async Task<IEnumerable<ParcelOwner>> GetByOwnerIdAsync(long ownerId, IDbTransaction? transaction = null)
    {
        const string sql = $"SELECT {SelectColumns} FROM public.parcel_owners WHERE owner_id = @OwnerId;";
        return await _db.QueryAsync<ParcelOwner>(sql, new { OwnerId = ownerId }, transaction: transaction);
    }

    public async Task<bool> AddAsync(ParcelOwner parcelOwner, IDbTransaction? transaction = null)
    {
        const string sql = @"
            INSERT INTO public.parcel_owners (parcel_id, owner_id, ownership_share)
            VALUES (@ParcelId, @OwnerId, @OwnershipShare)
            ON CONFLICT (parcel_id, owner_id) DO UPDATE
            SET ownership_share = EXCLUDED.ownership_share;";
        var rows = await _db.ExecuteAsync(sql, parcelOwner, transaction: transaction);
        return rows > 0;
    }

    public async Task<bool> UpdateShareAsync(ParcelOwner parcelOwner, IDbTransaction? transaction = null)
    {
        const string sql = @"
            UPDATE public.parcel_owners
            SET ownership_share = @OwnershipShare
            WHERE parcel_id = @ParcelId AND owner_id = @OwnerId;";
        var rows = await _db.ExecuteAsync(sql, parcelOwner, transaction: transaction);
        return rows > 0;
    }

    public async Task<bool> RemoveAsync(long parcelId, long ownerId, IDbTransaction? transaction = null)
    {
        const string sql = "DELETE FROM public.parcel_owners WHERE parcel_id = @ParcelId AND owner_id = @OwnerId;";
        var rows = await _db.ExecuteAsync(sql, new { ParcelId = parcelId, OwnerId = ownerId }, transaction: transaction);
        return rows > 0;
    }
}
