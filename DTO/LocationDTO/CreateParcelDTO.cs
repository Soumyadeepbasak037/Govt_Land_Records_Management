namespace govt_land_service.DTO.LocationDTO
{
    public class CreateParcelDTO
    {
        public long ProjectId { get; set; }
        public long VillageId { get; set; }
        public string ParcelNumber { get; set; }
        public string SurveyNumber { get; set; }
        public decimal Area { get; set; }
        public string OwnershipStatus { get; set; }
        public string AcquisitionStatus { get; set; }

        public string Geometry { get; set; }

        public List<ParcelOwnerDTO> Owners { get; set; } = new List<ParcelOwnerDTO>();
    }

    public class ParcelOwnerDTO
    {
        
        public long? OwnerId { get; set; }
        public decimal OwnershipShare { get; set; }

        // Fields for creating a NEW owner (leave null if OwnerId is provided)
        public string Name { get; set; }
        public string IdentifierType { get; set; }
        public string IdentifierHash { get; set; }
        public string Phone { get; set; }
    }
}