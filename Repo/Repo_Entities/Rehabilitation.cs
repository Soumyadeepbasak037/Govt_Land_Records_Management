using System;

namespace GovtLandService.Data.Entities;

public class Rehabilitation
{
    public long Id { get; set; }
    public long ProjectId { get; set; }
    public long? OwnerId { get; set; }
    public bool Affected { get; set; } = true;
    public bool Displaced { get; set; } = false;
    public string? AssistanceType { get; set; }
    public string? AssistanceStatus { get; set; }
    public DateTimeOffset? CompletedAt { get; set; }
    public string? Remarks { get; set; }
}
