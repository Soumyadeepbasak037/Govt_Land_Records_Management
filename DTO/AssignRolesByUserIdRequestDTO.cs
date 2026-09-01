namespace govt_land_service.DTO
{
    public class AssignOrRemoveRolesByUserIdRequestDTO
    {
        public int userId { get; set; }
        public int[] roleIds { get; set; }
    }
}
