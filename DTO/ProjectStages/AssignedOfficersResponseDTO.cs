namespace govt_land_service.DTO.ProjectStages
{
    public class AssignedOfficersResponseDTO
    {
  //      "success": true,
  //"message": "Stage successfully assigned to Ramesh Kumar.",
  //"assignedTo": 104,
  //"updatedAt": "2026-10-24T10:15:30Z"
  //

        public bool ? success { get; set; }
        public required string message { get; set; }
        public DateTime updatedAt { get; set; }
        public int assignedTo { get; set; }

    }
}
