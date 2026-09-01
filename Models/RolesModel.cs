namespace govt_land_service.Models
{
    public class RolesModel
    {
        public int userId { get; set; }
        public string[] roleNames { get; set; }
        public int[] roleIds { get; set; }
    }
}

// we will return a response containing the userIds and the roleNames that were removed or inserted as multiple roles can be assigned and removed from the user by the admin user.
