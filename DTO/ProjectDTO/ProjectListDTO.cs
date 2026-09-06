namespace govt_land_service.DTO.ProjectDTO
{
    public class ProjectListDTO
    {
        public long Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string ProjectType { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public string? DistrictName { get; set; }

        public string? StateName { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
