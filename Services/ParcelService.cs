using Dapper;
using govt_land_service.DTO.LocationDTO;
using govt_land_service.Models;

namespace govt_land_service.Services
{
    public class ParcelService : IParcelService
    {
        private readonly IConfiguration _configuration;
        public ParcelService(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public async Task<ParcelResponseDTO> CreateParcel(CreateParcelDTO dto)
        {
            await using var connection =
                new Npgsql.NpgsqlConnection(
                    _configuration.GetConnectionString("DefaultConnection"));



            await connection.OpenAsync();

            await using var transaction = await connection.BeginTransactionAsync();

            const string check_exists = """

                        SELECT 
                EXISTS (SELECT 1 FROM projects WHERE id = @ProjectId) AS "ProjectExists",
                EXISTS (SELECT 1 FROM villages WHERE id = @VillageId) AS "VillageExists";
                """;

            const string parcelSql = """
                                        INSERT INTO land_parcels 
                
                                        (
                                            project_id,
                                            village_id,
                                            parcel_number,
                                            survey_number,
                                            area,
                                            ownership_status,
                                            acquisition_status,
                                            geometry
                                        )
                                        VALUES
                                        (
                                            @ProjectId,
                                            @VillageId,
                                            @ParcelNumber,
                                            @SurveyNumber,
                                            @Area,
                                            @OwnershipStatus,
                                            @AcquisitionStatus,

                                            ST_Multi(ST_SetSRID(ST_GeomFromGeoJSON(@Geometry::text), 4326))
                                        )
                                        RETURNING id;
                                    """;
            var linkOwnerSql = "INSERT INTO parcel_owners (parcel_id, owner_id, ownership_share) VALUES (@ParcelId, @OwnerId, @OwnershipShare);";

            var insertNewOwnerSql = """
                                INSERT INTO land_owners (name, identifier_type, identifier_hash, phone) 
                                VALUES (@Name, @IdentifierType, @IdentifierHash, @Phone) 
                                RETURNING id;
                            """;
            //var updateStageSql = """INSERt INTO project_stages """;
            try
            {
                var validationResult = await connection.QuerySingleAsync(check_exists, new { ProjectId = dto.ProjectId, VillageId = dto.VillageId }, transaction);
                if (!validationResult.ProjectExists)
                {
                    throw new Exception($"Project with ID {dto.ProjectId} does not exist.");
                }

                if (!validationResult.VillageExists)
                {
                    throw new Exception($"Village with ID {dto.VillageId} does not exist.");
                }
                var parcelId = await connection.ExecuteScalarAsync<long>(parcelSql, new { ProjectId = dto.ProjectId, VillageId = dto.VillageId, ParcelNumber = dto.ParcelNumber, SurveyNumber = dto.SurveyNumber, Area =dto.Area, OwnershipStatus=dto.OwnershipStatus , AcquisitionStatus=dto.AcquisitionStatus , Geometry =dto.Geometry},transaction);

                foreach (var owner in dto.Owners) {
                    long resolvedOwnerId;

                    if (owner.OwnerId.HasValue && owner.OwnerId > 0)
                    {
                        resolvedOwnerId = owner.OwnerId.Value;
                    }
                    else
                    {
                        resolvedOwnerId = await connection.ExecuteScalarAsync<long>(
                            insertNewOwnerSql,
                            owner,
                            transaction
                        );
                    }
                    await connection.ExecuteAsync(linkOwnerSql, new
                    {
                        ParcelId = parcelId,
                        OwnerId = resolvedOwnerId,
                        OwnershipShare = owner.OwnershipShare
                    }, transaction);
                }

                await transaction.CommitAsync();

                ParcelResponseDTO response = new ParcelResponseDTO();
                response.Id = parcelId;
                response.ProjectId = dto.ProjectId;
                response.SurveyNumber = dto.SurveyNumber;
                response.Area = dto.Area;
                response.Geometry = dto.Geometry;
                response.AcquisitionStatus = dto.AcquisitionStatus;
                return response;
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex);
                await transaction.RollbackAsync();
                throw;
               
            }            
        }
    }
}
