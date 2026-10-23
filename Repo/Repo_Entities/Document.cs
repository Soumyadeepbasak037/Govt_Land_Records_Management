using System;

namespace GovtLandService.Data.Entities;

public class Document
{
    public long Id { get; set; }
    public long? ProjectId { get; set; }
    public long? ParcelId { get; set; }
    public string DocumentType { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string StorageKey { get; set; } = string.Empty;
    public long UploadedBy { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}
