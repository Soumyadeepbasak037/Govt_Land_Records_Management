namespace govt_land_service.DTO.LocationDTO
{
    public class CreateParcelDTO
    {
        public string SurveyNumber { get; set; } = string.Empty;

        public decimal Area { get; set; }

        public string Geometry { get; set; } = string.Empty;
    }
}
