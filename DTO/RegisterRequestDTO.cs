namespace govt_land_service.DTO
{
    public class RegisterRequestDTO
    {
        public string name { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string password { get; set; } = string.Empty;
        public string? phone { get; set; }
    }
}
