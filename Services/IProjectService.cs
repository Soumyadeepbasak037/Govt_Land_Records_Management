using govt_land_service.DTO.ProjectDTO;

namespace govt_land_service.Services
{
    public interface IProjectService
    {

        Task<ProjectResponseDTO> CreateProjectAsync(
            CreateProjectDTO dto
            
        );

        Task<ProjectDetailsDTO?> GetProjectByIdAsync(
            long projectId
        );

        Task<ProjectResponseDTO> UpdateProjectAsync(
            long projectId,
            UpdateProjectDTO dto,
            long userId
        );

        Task<ProjectResponseDTO> SubmitProjectAsync(
            long projectId,
            SubmitProjectDTO dto,
            long userId
        );

        Task<ProjectResponseDTO> VerifyProjectAsync(
            long projectId,
            VerifyProjectDTO dto,
            long userId
        );

        Task<ProjectResponseDTO> ApproveProjectAsync(
            long projectId,
            ApproveProjectDTO dto,
            long userId
        );
        Task<IEnumerable<ProjectListDTO>> GetProjectsAsync(
            ProjectFilterDTO filter
        );

        //Task<ProjectResponseDTO> RejectProjectAsync(
        //    long projectId,
        //    RejectProjectDTO dto,
        //    long userId
        //);

        //Task<ProjectResponseDTO> CancelProjectAsync(
        //    long projectId,
        //    CancelProjectDTO dto,
        //    long userId
        //);

        //Task<IEnumerable<ProjectStatusHistoryDTO>> GetProjectHistoryAsync(
        //    long projectId
        //);
    }
}
