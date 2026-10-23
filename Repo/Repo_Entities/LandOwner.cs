using System;

namespace GovtLandService.Data.Entities;

public class LandOwner
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? IdentifierType { get; set; }
    public string? IdentifierHash { get; set; }
    public string? Phone { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}
