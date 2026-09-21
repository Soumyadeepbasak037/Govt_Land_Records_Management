using govt_land_service.DTO.LocationDTO;

namespace govt_land_service.Services
{
    public interface IParcelService
    {
       Task<ParcelResponseDTO> CreateParcel(CreateParcelDTO dto);

    }
}
