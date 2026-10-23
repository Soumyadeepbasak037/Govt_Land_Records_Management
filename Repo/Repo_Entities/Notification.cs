using System;

namespace GovtLandService.Data.Entities;

public class Notification
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public long? ProjectId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}
