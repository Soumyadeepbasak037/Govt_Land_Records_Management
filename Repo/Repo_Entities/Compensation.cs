using System;

namespace GovtLandService.Data.Entities;

public class Compensation
{
    public long Id { get; set; }
    public long ParcelId { get; set; }
    public long OwnerId { get; set; }
    public decimal AssessedAmount { get; set; }
    public decimal? DisbursedAmount { get; set; } = 0m;
    public string Status { get; set; } = "ASSESSED";
    public DateTimeOffset? AssessedAt { get; set; }
    public DateTimeOffset? DisbursedAt { get; set; }
}
