namespace govt_land_service.DTO.ProjectDTO
{
    public class ProjectFilterDTO
    {
        public string? Search { get; set; }

        public string? Status { get; set; }

        public string? ProjectType { get; set; }

        public long? StateId { get; set; }

        public long? DistrictId { get; set; }

        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 20;
    }
}
