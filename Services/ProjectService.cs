using Dapper;
using govt_land_service.DTO.ProjectDTO;

namespace govt_land_service.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IConfiguration _configuration;
        public ProjectService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<ProjectResponseDTO> CreateProjectAsync(
            CreateProjectDTO dto
            )
        {
            await using var connection =
                new Npgsql.NpgsqlConnection(
                    _configuration.GetConnectionString("DefaultConnection"));

            await connection.OpenAsync();

            await using var transaction = await connection.BeginTransactionAsync();

            const string sql = """
                                                INSERT INTO projects
                                                (
                                                    name,
                                                    description,
                                                    project_type,
                                                    state_id,
                                                    district_id,
                                                    created_by,
                                                    implementing_agency
                                                )
                                                VALUES
                                                (
                                                    @Name,
                                                    @Description,
                                                    @ProjectType,
                                                    @StateId,
                                                    @DistrictId,
                                                    @UserId,
                                                    @ImplementingAgency
                                                )
                                                RETURNING id;
        """;

            const string get_details_sql = """select id,name,description,project_type,state_id,district_id,created_by,implementing_agency,status, ST_AsGeoJSON(geometry) AS geometry from projects where id = @ID""";

            try
            {
                var parameters = new
                {
                    Name = dto.Name,
                    Description = dto.Description,
                    ProjectType = dto.ProjectType,
                    StateId = dto.StateId,
                    DistrictId = dto.DistrictId,
                    UserId = dto.Userid,
                    ImplementingAgency = dto.ImplementingAgency
                };

                long projectId;


                    projectId = await connection.QuerySingleAsync<long>(
                        sql,
                        parameters,transaction);
               
              
                ProjectResponseDTO response = await connection.QueryFirstAsync<ProjectResponseDTO>(get_details_sql, new { ID = projectId },transaction);
                await transaction.CommitAsync();
                return response;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine(ex);
                return null;

            }
        }

        public async Task<ProjectDetailsDTO?> GetProjectByIdAsync(long projectId)
        {
            throw new NotImplementedException();
        }


        public async Task<ProjectResponseDTO> ApproveProjectAsync(long projectId, ApproveProjectDTO dto, long userId)
        {
            throw new NotImplementedException();
        }


        

        public async Task<IEnumerable<ProjectListDTO>> GetProjectsAsync(ProjectFilterDTO filter)
        {
            throw new NotImplementedException();
        }

        public async Task<ProjectResponseDTO> SubmitProjectAsync(long projectId, SubmitProjectDTO dto, long userId)
        {
            throw new NotImplementedException();
        }

        public async Task<ProjectResponseDTO> UpdateProjectAsync(long projectId, UpdateProjectDTO dto, long userId)
        {
            throw new NotImplementedException();
        }

        public async Task<ProjectResponseDTO> VerifyProjectAsync(long projectId, VerifyProjectDTO dto, long userId)
        {
            throw new NotImplementedException();
        }
    }
}
