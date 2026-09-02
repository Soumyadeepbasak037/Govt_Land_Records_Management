using govt_land_service.Models;
using Microsoft.AspNetCore.Mvc.TagHelpers;
namespace govt_land_service.Services
{
    public interface ILocationService
    {
        public Task<LocationData> GetStates();
        public Task<LocationData> GetDistricts();
        public Task<LocationData> GetVillages();
        public Task<int> InsertDistricts(int state_id,string[] districtName);
        public Task<int> InsertVillages(int district_id,string[] villageName);
       
    }
}
