using System;

namespace GovtLandService.Data.Entities;

public class ProjectStage
{
    public long Id { get; set; }
    public long ProjectId { get; set; }
    public int StageId { get; set; }
    public string Status { get; set; } = string.Empty;
    public long? AssignedTo { get; set; }
    public DateTimeOffset? StartedAt { get; set; }
    public DateTimeOffset? CompletedAt { get; set; }
    public string? Remarks { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}
