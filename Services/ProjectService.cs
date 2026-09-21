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

            const string check_state_district_exists_sql = """

                select 1 from states s 
                inner join districts d 
                on s.id = d.state_id 
                where state_id = @STATEID and d.id = @DISTRICTID
     
                """;
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

            const string get_details_sql = """
    SELECT 
        id,
        name,
        description,
        project_type AS ProjectType,
        state_id AS StateId,
        district_id AS DistrictId,
        created_by AS CreatedBy,
        implementing_agency AS ImplementingAgency,
        status, 
        created_at AS CreatedAt,
        ST_AsGeoJSON(geometry) AS geometry 
    FROM projects 
    WHERE id = @ID
""";

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


                var check = await connection.QueryFirstOrDefaultAsync<int>(check_state_district_exists_sql, new { STATEID = parameters.StateId, DISTRICTID = parameters.DistrictId } , transaction);

                //Console.WriteLine( check );

                if(check == 0)
                {
                    throw new Exception("The specified State or District does not exist.");
                }
                long projectId;



                    projectId = await connection.QuerySingleAsync<long>(
                        sql,
                        parameters,transaction);
               
              
                ProjectResponseDTO response = await connection.QueryFirstAsync<ProjectResponseDTO>(get_details_sql, new { ID = projectId },transaction);



                const string insertStatusSql = """
            INSERT INTO project_stages
            (
                project_id,
                stage,
                status
            )
            VALUES
            (
                @ProjectId,
                '0',
                'DRAFT'
                
            );
            """;

                await connection.ExecuteAsync(
                    insertStatusSql,
                    new
                    {
                        ProjectId = projectId,
                        UserId = dto.Userid
                    },
                    transaction);


                const string insertAuditSql = """
            INSERT INTO audit_logs
            (
                user_id,
                action,
                entity_type,
                entity_id
            )
            VALUES
            (
                @UserId,
                'PROJECT_CREATED',
                'PROJECT',
                @ProjectId
            );
            """;

                await connection.ExecuteAsync(
                    insertAuditSql,
                    new
                    {
                        UserId = dto.Userid,
                        ProjectId = projectId
                    },
                    transaction);

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
