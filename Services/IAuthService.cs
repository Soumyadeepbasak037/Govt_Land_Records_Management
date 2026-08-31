using govt_land_service.Models;
using govt_land_service.DTO;
namespace govt_land_service.Services
{
    public interface IAuthService
    {
        Task<string?> LoginAsync(string username, string password);
        //Task<int?> RegisterAsync(UserModel user);
        Task<int?> RegisterAsync(RegisterRequestDTO request);
    }
}
