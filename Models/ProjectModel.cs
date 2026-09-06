namespace govt_land_service.Models
{
    public class Project
    {
        public long Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string ProjectType { get; set; } = string.Empty;

        public long StateId { get; set; }

        public long DistrictId { get; set; }

        public long? VillageId { get; set; }

        public string Status { get; set; } = string.Empty;

        public long CreatedBy { get; set; }

        public string Geometry { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
