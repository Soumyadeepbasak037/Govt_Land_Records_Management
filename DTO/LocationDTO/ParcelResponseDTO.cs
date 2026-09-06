namespace govt_land_service.DTO.LocationDTO
{
    public class ParcelResponseDTO
    {
        public long Id { get; set; }

        public long ProjectId { get; set; }

        public string SurveyNumber { get; set; } = string.Empty;

        public decimal Area { get; set; }

        public string AcquisitionStatus { get; set; } = string.Empty;

        public string Geometry { get; set; } = string.Empty;
    }
}
