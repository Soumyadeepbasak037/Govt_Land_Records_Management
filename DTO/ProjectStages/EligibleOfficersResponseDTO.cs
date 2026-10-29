namespace govt_land_service.DTO.ProjectStages
{
    public class EligibleOfficersResponseDTO
    {
        //        [
        //  {
        //    "id": 104,
        //    "name": "Ramesh Kumar",
        //    "email": "ramesh.surveyor@gov.in",
        //    "roles": ["SURVEYOR"]
        //    },
        //  {
        //    "id": 109,
        //    "name": "Sarah Jenkins",
        //    "email": "sarah.lao@gov.in",
        //    "roles": ["LAND_ACQUISITION_OFFICER"]
        //}
        //]
        public int id { get; set; }
        public required string email { get; set; }
        public required string name { get; set; }
        public required string[] roles { get; set; }
    }
}
