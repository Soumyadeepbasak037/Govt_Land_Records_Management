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
                EXISTS (SELECT 1 FROM projects WHERE id = 1) AS ProjectExists,
                EXISTS (SELECT 1 FROM villages WHERE id = 2) AS VillageExists;
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

                                            ST_Multi(ST_SetSRID(ST_GeomFromGeoJSON(@Geometry), 4326))
                                        )
                                        RETURNING id;
                                    """;
            var linkOwnerSql = "INSERT INTO parcel_owners (parcel_id, owner_id, ownership_share) VALUES (@ParcelId, @OwnerId, @OwnershipShare);";

            var insertNewOwnerSql = """
                                INSERT INTO land_owners (name, identifier_type, identifier_hash, phone) 
                                VALUES (@Name, @IdentifierType, @IdentifierHash, @Phone) 
                                RETURNING id;
                            """;
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
                var parcelId = await connection.QuerySingleAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception(ex.ToString());
            }            
        }
    }
}
