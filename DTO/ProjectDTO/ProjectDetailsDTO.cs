namespace govt_land_service.DTO.ProjectDTO
{
    public class ProjectDetailsDTO
    {
        public long Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string ProjectType { get; set; } = string.Empty;

        public long StateId { get; set; }

        public string? StateName { get; set; }

        public long DistrictId { get; set; }

        public string? DistrictName { get; set; }

        public long? VillageId { get; set; }

        public string? VillageName { get; set; }

        public string Status { get; set; } = string.Empty;

        public long CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public int ParcelCount { get; set; }

        public decimal TotalLandArea { get; set; }
    }
}
