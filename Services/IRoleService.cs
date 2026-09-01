using govt_land_service.DTO;
using govt_land_service.Models;

namespace govt_land_service.Services
{
    public interface IRoleService
    {
        Task<int[]?> CreateRole(string[] roleName);//returns an array of the ids of the created roles
        Task<RolesModel> GetRolesByUserIdAsync(int userID);
        Task<RolesResponseDTO> AssignRolesToUser(int userId, int[] roleId);
        Task<RolesResponseDTO> RemoveRolesFromUser(int userId, int[] roleIds);

    }
}
