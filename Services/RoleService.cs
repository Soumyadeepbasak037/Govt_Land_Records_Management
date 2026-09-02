using Dapper;
using govt_land_service.DTO;
using govt_land_service.Models;
using Npgsql.Replication.PgOutput.Messages;

namespace govt_land_service.Services
{
    public class RoleService : IRoleService
    {
        private readonly IConfiguration _configuration;
        public RoleService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<RolesResponseDTO> AssignRolesToUser(int userId, int[] roleId)
        {
            const string getRoleNamesSql = """
                SELECT name
                FROM roles
                WHERE id = ANY(@RolesIds);
                """;
            const string insertQuery = """insert into user_roles (user_id,role_id) select @userid,unnest(@RolesIds)""";
                await using var connection = new Npgsql.NpgsqlConnection(
                _configuration.GetConnectionString("DefaultConnection")
            );
            await connection.OpenAsync();

            await using var transaction = await connection.BeginTransactionAsync();
            try
            {
                var parameters = new
                {
                    UserId = userId,
                    RolesIds = roleId
                };

                var roleNames = (
                    await connection.QueryAsync<string>(
                        getRoleNamesSql,
                        parameters,
                        transaction
                    )
                ).ToArray();

                var affectedCount = (
                    await connection.ExecuteAsync(
                        insertQuery,
                        parameters,
                        transaction
                    )
                );

                if (affectedCount != roleId.Length)
                {
                    await transaction.RollbackAsync();
                    return null;
                }

                await transaction.CommitAsync();

                return new RolesResponseDTO(
                    userId: userId,
                    roleId: roleId,
                    roleName: roleNames
                );
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                Console.WriteLine(ex);
                return null;
            }
        }


        public async Task<RolesResponseDTO> RemoveRolesFromUser(int userId, int[] roleIds)
        {
            //verify whether the roles belong to the user and then remove them
            //using a transaction 
            const string getRoleNamesSql = """
                SELECT name
                FROM roles
                WHERE id = ANY(@ROLEIDS);
                """;
            const string checkRolesBelongToUserSql = """select role_id from user_roles where user_id = @USERID""";
            const string removeRolesSql = """delete from user_roles where user_id = @USERID and role_id in @ROLEIDS""";

            await using var connection = new Npgsql.NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            await connection.OpenAsync();

            await using var transaction = await connection.BeginTransactionAsync();
            try {
                var roleNames = (
                   await connection.QueryAsync<string>(
                       getRoleNamesSql,
                       new { ROLEIDS = roleIds},
                       transaction
                   )
               ).ToArray();
                var verifiedIds = (await connection.QueryAsync<int>(checkRolesBelongToUserSql, new {USERID = userId},transaction)).ToArray();
                bool areAllValid = roleIds.All(id => verifiedIds.Contains(id));

                if (!areAllValid)
                {
                    throw new Exception("User doesn't possess the roles that are trying to be modified!");
                }

                var affectedCount = await connection.ExecuteAsync(removeRolesSql, new {USERID =  userId,ROLEIDS = roleIds},transaction);

                if(affectedCount != roleIds.Count()) { 
                    await transaction.RollbackAsync();
                }
                else
                {
                    await transaction.CommitAsync();
                }

                RolesResponseDTO response = new RolesResponseDTO(
                        userId : userId,
                        roleId : roleIds,
                       roleName : roleNames

                    );
                return response;
            }
            catch (Exception ex) { 
                Console.WriteLine(ex.Message);
                return null;
            }

        }
        public async Task<int[]?> CreateRole(string[] roleNames)
        {
            const string sql = """
        INSERT INTO roles (name)
        SELECT unnest(@Names)
        RETURNING id;
        """;

            await using var connection = new Npgsql.NpgsqlConnection(
                _configuration.GetConnectionString("DefaultConnection")
            );

            try
            {
                var roleIds = await connection.QueryAsync<int>(
                    sql,
                    new { Names = roleNames }
                );

                return roleIds.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while creating roles: {ex}");
                return null;
            }
        }
        public async Task<RolesModel> GetRolesByUserIdAsync(int userID)
        {
            await using var connection = new Npgsql.NpgsqlConnection(
                _configuration.GetConnectionString("DefaultConnection")
            );
            const string sql = """select r.id as roleId,r.name as roleName from users u inner join user_roles ur on u.id = ur.user_id inner join roles r on ur.role_id = r.id where u.id = @userId""";

            try
            {
                var roles = await connection.QueryAsync<RoleInfotDto>(sql, new { userid = userID });

                RolesModel res = new RolesModel
                {
                    userId = userID,
                    roleNames = roles.Select(r => r.roleName).ToArray(),
                    roleIds = roles.Select(r => r.roleid).ToArray()
                };
                return res;
            }
            catch (Exception ex) { 
                Console.WriteLine($"{ex.Message}");
                return null;
            }
        }


    }
}
