using govt_land_service.DTO.LocationDTO;
using govt_land_service.DTO.ProjectStages;

namespace govt_land_service.Services
{
    public interface IProjectStages
    {
        Task GetEligibleOfficers(EligibleOfficersDTO dto);
        Task AssignOfficer(AssignOfficerDTO dto);
    }
}
