using System;

namespace GovtLandService.Data.Entities;

public class DocumentVersion
{
    public long Id { get; set; }
    public long DocumentId { get; set; }
    public int VersionNumber { get; set; }
    public string StorageKey { get; set; } = string.Empty;
    public long UploadedBy { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}
