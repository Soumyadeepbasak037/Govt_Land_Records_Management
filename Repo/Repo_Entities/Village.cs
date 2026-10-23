namespace GovtLandService.Data.Entities;

public class Village
{
    public long Id { get; set; }
    public int DistrictId { get; set; }
    public string Name { get; set; } = string.Empty;
}
