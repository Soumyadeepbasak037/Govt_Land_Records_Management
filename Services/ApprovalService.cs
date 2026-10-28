using Dapper;
using System.Security.Claims;
using  GovtLandService.Data.Repositories;

namespace govt_land_service.Services
{
    public class ApprovalService : IApprovalService
    {
        public readonly IConfiguration _configuration;
        public readonly IHttpContextAccessor _httpContextAccessor;
        public ApprovalService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> Approve_Project(int projectId)
        {
            await using var connection =
              new Npgsql.NpgsqlConnection(
                  _configuration.GetConnectionString("DefaultConnection"));

            await connection.OpenAsync();

            await using var transaction = await connection.BeginTransactionAsync();

            var user = _httpContextAccessor.HttpContext?.User;
            var roles = user?
               .FindAll(ClaimTypes.Role)
               .Select(c => c.Value)
               .ToList() ?? new List<string>();
            
            Console.WriteLine(roles);



            throw new NotImplementedException();
        }
    };
    
}
