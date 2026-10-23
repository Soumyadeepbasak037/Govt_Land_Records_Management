using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using GovtLandService.Data.Entities;
using GovtLandService.Data.Interfaces;

namespace GovtLandService.Data.Repositories;

public class LandParcelRepository : ILandParcelRepository
{
    private readonly IDbConnection _db;

    public LandParcelRepository(IDbConnection db)
    {
        _db = db;
    }

    private const string SelectColumns = @"
        id AS Id,
        project_id AS ProjectId,
        parcel_number AS ParcelNumber,
        survey_number AS SurveyNumber,
        village_id AS VillageId,
        area AS Area,
        ownership_status AS OwnershipStatus,
        acquisition_status AS AcquisitionStatus,
        geometry AS Geometry,
        ST_AsGeoJSON(geometry) AS GeometryGeoJson,
        created_at AS CreatedAt,
        updated_at AS UpdatedAt";

    public async Task<LandParcel?> GetByIdAsync(long id, IDbTransaction? transaction = null)
    {
        const string sql = $"SELECT {SelectColumns} FROM public.land_parcels WHERE id = @Id;";
        return await _db.QuerySingleOrDefaultAsync<LandParcel>(sql, new { Id = id }, transaction: transaction);
    }

    public async Task<IEnumerable<LandParcel>> GetAllAsync(IDbTransaction? transaction = null)
    {
        const string sql = $"SELECT {SelectColumns} FROM public.land_parcels ORDER BY id DESC;";
        return await _db.QueryAsync<LandParcel>(sql, transaction: transaction);
    }

    public async Task<IEnumerable<LandParcel>> GetByProjectIdAsync(long projectId, IDbTransaction? transaction = null)
    {
        const string sql = $"SELECT {SelectColumns} FROM public.land_parcels WHERE project_id = @ProjectId ORDER BY parcel_number;";
        return await _db.QueryAsync<LandParcel>(sql, new { ProjectId = projectId }, transaction: transaction);
    }

    public async Task<IEnumerable<LandParcel>> GetByVillageIdAsync(long villageId, IDbTransaction? transaction = null)
    {
        const string sql = $"SELECT {SelectColumns} FROM public.land_parcels WHERE village_id = @VillageId ORDER BY parcel_number;";
        return await _db.QueryAsync<LandParcel>(sql, new { VillageId = villageId }, transaction: transaction);
    }

    public async Task<LandParcel?> GetByProjectAndParcelNumberAsync(long projectId, string parcelNumber, IDbTransaction? transaction = null)
    {
        const string sql = $"SELECT {SelectColumns} FROM public.land_parcels WHERE project_id = @ProjectId AND parcel_number = @ParcelNumber;";
        return await _db.QuerySingleOrDefaultAsync<LandParcel>(sql, new { ProjectId = projectId, ParcelNumber = parcelNumber }, transaction: transaction);
    }

    public async Task<long> CreateAsync(LandParcel parcel, IDbTransaction? transaction = null)
    {
        const string sql = @"
            INSERT INTO public.land_parcels (
                project_id, parcel_number, survey_number, village_id, area,
                ownership_status, acquisition_status, geometry, created_at, updated_at
            )
            VALUES (
                @ProjectId, @ParcelNumber, @SurveyNumber, @VillageId, @Area,
                @OwnershipStatus, COALESCE(@AcquisitionStatus, 'PROPOSED'),
                COALESCE(@Geometry, CASE WHEN @GeometryGeoJson IS NOT NULL THEN ST_SetSRID(ST_Multi(ST_GeomFromGeoJSON(@GeometryGeoJson)), 4326) ELSE NULL END),
                now(), now()
            )
            RETURNING id;";
        return await _db.ExecuteScalarAsync<long>(sql, parcel, transaction: transaction);
    }

    public async Task<bool> UpdateAsync(LandParcel parcel, IDbTransaction? transaction = null)
    {
        const string sql = @"
            UPDATE public.land_parcels
            SET project_id = @ProjectId,
                parcel_number = @ParcelNumber,
                survey_number = @SurveyNumber,
                village_id = @VillageId,
                area = @Area,
                ownership_status = @OwnershipStatus,
                acquisition_status = @AcquisitionStatus,
                geometry = COALESCE(@Geometry, CASE WHEN @GeometryGeoJson IS NOT NULL THEN ST_SetSRID(ST_Multi(ST_GeomFromGeoJSON(@GeometryGeoJson)), 4326) ELSE geometry END),
                updated_at = now()
            WHERE id = @Id;";
        var rows = await _db.ExecuteAsync(sql, parcel, transaction: transaction);
        return rows > 0;
    }

    public async Task<bool> UpdateAcquisitionStatusAsync(long id, string acquisitionStatus, IDbTransaction? transaction = null)
    {
        const string sql = @"
            UPDATE public.land_parcels
            SET acquisition_status = @AcquisitionStatus,
                updated_at = now()
            WHERE id = @Id;";
        var rows = await _db.ExecuteAsync(sql, new { Id = id, AcquisitionStatus = acquisitionStatus }, transaction: transaction);
        return rows > 0;
    }

    public async Task<bool> DeleteAsync(long id, IDbTransaction? transaction = null)
    {
        const string sql = "DELETE FROM public.land_parcels WHERE id = @Id;";
        var rows = await _db.ExecuteAsync(sql, new { Id = id }, transaction: transaction);
        return rows > 0;
    }
}
