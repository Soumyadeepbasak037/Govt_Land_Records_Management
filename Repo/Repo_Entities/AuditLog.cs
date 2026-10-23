using System;

namespace GovtLandService.Data.Entities;

public class AuditLog
{
    public long Id { get; set; }
    public long? UserId { get; set; }
    public string Action { get; set; } = string.Empty;
    public string EntityType { get; set; } = string.Empty;
    public long? EntityId { get; set; }
    public string? OldValue { get; set; } // JSONB string
    public string? NewValue { get; set; } // JSONB string
    public DateTimeOffset CreatedAt { get; set; }
}
