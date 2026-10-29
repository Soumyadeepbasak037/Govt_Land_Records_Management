using govt_land_service.DTO.ProjectStages;
using GovtLandService.Data.Entities;
using GovtLandService.Data.Interfaces;
using GovtLandService.Data.Repositories;
using System.Data;

namespace govt_land_service.Services
{
    public class ProjectStageService : IProjectStages
    {
        private readonly IConfiguration _configuration;
        private readonly IProjectRepository _projRepo;
        private readonly IProjectStageRepository _stagesRepo;
        private readonly IDbConnection _db;

        public ProjectStageService(IConfiguration configuration, IProjectRepository proj_repo, IProjectStageRepository stage_repo,IDbConnection db)
        {
            _db = db;
            _configuration = configuration;
            _projRepo = proj_repo;
            _stagesRepo = stage_repo;
        }
        public async Task<AssignedOfficersResponseDTO> AssignOfficer(AssignOfficerDTO dto)
        {
           

            throw new NotImplementedException();
        }

        public async Task<EligibleOfficersResponseDTO[]> GetEligibleOfficers(EligibleOfficersDTO dto)
        {
            if (_db.State != ConnectionState.Open)
                _db.Open();

            using var transaction = _db.BeginTransaction();
            try
            {
                var projects = await _projRepo.GetByIdAsync(dto.projectId, transaction);
                if(projects == null)
                {
                    throw new Exception("Project Id is invalid");
                }
                var project_stage = await _stagesRepo.GetByProjectIdAsync(dto.projectId, transaction);
                foreach(ProjectStage stage in project_stage)
                {
                    if(stage.Id == dto.stageId)
                    {
                        if(stage.AssignedTo != null)
                        {
                            throw new Exception("Project is already assigned to an indivudual");
                        }
                    }
                }
                var eligible_individuals = await _stagesRepo.GetEligibleOfficersByStageAsync(dto.stageId, transaction);
                transaction.Commit();
                return eligible_individuals;

            }
            catch 
            {
                transaction.Rollback();
                throw;
            }


            //check project_id exists -> check stage_id exists and belongs to the project_id -> stage doesnt already have a asscoiated assigned-to user_id 





     
        }
    }
}
