using govt_land_service.Models;

namespace govt_land_service.Services
{
    public interface IRoleService
    {
        Task<RolesModel?> CreateRole(string roleName);
        Task<RolesModel[]> GetRolesAsync();
        Task<int> AssignRoleToUser(int user_id, int roleId, string? roleName);
        Task<int> RemoveRolesFromuser(int user_id, int roleId);

    }
}
