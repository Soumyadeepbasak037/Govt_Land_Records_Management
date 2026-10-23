using govt_land_service.DTO.LocationDTO;

namespace govt_land_service.Services
{
    public interface IApprovalService
    {
        Task<bool> Approve_Project(int projectId);

    }
}
