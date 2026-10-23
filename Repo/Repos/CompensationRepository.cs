using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using GovtLandService.Data.Entities;
using GovtLandService.Data.Interfaces;

namespace GovtLandService.Data.Repositories;

public class CompensationRepository : ICompensationRepository
{
    private readonly IDbConnection _db;

    public CompensationRepository(IDbConnection db)
    {
        _db = db;
    }

    private const string SelectColumns = @"
        id AS Id,
        parcel_id AS ParcelId,
        owner_id AS OwnerId,
        assessed_amount AS AssessedAmount,
        disbursed_amount AS DisbursedAmount,
        status AS Status,
        assessed_at AS AssessedAt,
        disbursed_at AS DisbursedAt";

    public async Task<Compensation?> GetByIdAsync(long id, IDbTransaction? transaction = null)
    {
        const string sql = $"SELECT {SelectColumns} FROM public.compensation WHERE id = @Id;";
        return await _db.QuerySingleOrDefaultAsync<Compensation>(sql, new { Id = id }, transaction: transaction);
    }

    public async Task<IEnumerable<Compensation>> GetAllAsync(IDbTransaction? transaction = null)
    {
        const string sql = $"SELECT {SelectColumns} FROM public.compensation ORDER BY id DESC;";
        return await _db.QueryAsync<Compensation>(sql, transaction: transaction);
    }

    public async Task<IEnumerable<Compensation>> GetByParcelIdAsync(long parcelId, IDbTransaction? transaction = null)
    {
        const string sql = $"SELECT {SelectColumns} FROM public.compensation WHERE parcel_id = @ParcelId ORDER BY id;";
        return await _db.QueryAsync<Compensation>(sql, new { ParcelId = parcelId }, transaction: transaction);
    }

    public async Task<IEnumerable<Compensation>> GetByOwnerIdAsync(long ownerId, IDbTransaction? transaction = null)
    {
        const string sql = $"SELECT {SelectColumns} FROM public.compensation WHERE owner_id = @OwnerId ORDER BY id;";
        return await _db.QueryAsync<Compensation>(sql, new { OwnerId = ownerId }, transaction: transaction);
    }

    public async Task<long> CreateAsync(Compensation compensation, IDbTransaction? transaction = null)
    {
        const string sql = @"
            INSERT INTO public.compensation (parcel_id, owner_id, assessed_amount, disbursed_amount, status, assessed_at, disbursed_at)
            VALUES (@ParcelId, @OwnerId, @AssessedAmount, COALESCE(@DisbursedAmount, 0), COALESCE(@Status, 'ASSESSED'), @AssessedAt, @DisbursedAt)
            RETURNING id;";
        return await _db.ExecuteScalarAsync<long>(sql, compensation, transaction: transaction);
    }

    public async Task<bool> UpdateAsync(Compensation compensation, IDbTransaction? transaction = null)
    {
        const string sql = @"
            UPDATE public.compensation
            SET parcel_id = @ParcelId,
                owner_id = @OwnerId,
                assessed_amount = @AssessedAmount,
                disbursed_amount = @DisbursedAmount,
                status = @Status,
                assessed_at = @AssessedAt,
                disbursed_at = @DisbursedAt
            WHERE id = @Id;";
        var rows = await _db.ExecuteAsync(sql, compensation, transaction: transaction);
        return rows > 0;
    }

    public async Task<bool> DeleteAsync(long id, IDbTransaction? transaction = null)
    {
        const string sql = "DELETE FROM public.compensation WHERE id = @Id;";
        var rows = await _db.ExecuteAsync(sql, new { Id = id }, transaction: transaction);
        return rows > 0;
    }
}
