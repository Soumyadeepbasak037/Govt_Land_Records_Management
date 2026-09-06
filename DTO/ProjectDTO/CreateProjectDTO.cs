namespace govt_land_service.DTO.ProjectDTO
{
    public class CreateProjectDTO
    {
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string ImplementingAgency { get; set; } = string.Empty;

        public string ProjectType { get; set; } = string.Empty;

        public long StateId { get; set; }

        public long DistrictId { get; set; }

        public long  Userid { get; set; }
   
    }
}
