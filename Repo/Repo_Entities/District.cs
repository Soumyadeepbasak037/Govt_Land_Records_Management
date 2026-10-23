namespace GovtLandService.Data.Entities;

public class District
{
    public int Id { get; set; }
    public int StateId { get; set; }
    public string Name { get; set; } = string.Empty;
}
