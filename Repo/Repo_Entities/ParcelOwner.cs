namespace GovtLandService.Data.Entities;

public class ParcelOwner
{
    public long ParcelId { get; set; }
    public long OwnerId { get; set; }
    public decimal? OwnershipShare { get; set; }
}
