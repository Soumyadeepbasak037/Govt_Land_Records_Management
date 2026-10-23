using System;

namespace GovtLandService.Data.Entities;

public class Approval
{
    public long Id { get; set; }
    public long ProjectId { get; set; }
    public long? StageId { get; set; }
    public long ApprovedBy { get; set; }
    public string Decision { get; set; } = string.Empty;
    public string? Remarks { get; set; }
    public DateTimeOffset DecidedAt { get; set; }
}
