using System;
using NetTopologySuite.Geometries;

namespace GovtLandService.Data.Entities;

public class Project
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string ProjectType { get; set; } = string.Empty;
    public string? ImplementingAgency { get; set; }
    public int StateId { get; set; }
    public int? DistrictId { get; set; }
    public decimal? TotalRequiredArea { get; set; }
    public string Status { get; set; } = "DRAFT";
    public long CreatedBy { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public MultiPolygon? Geometry { get; set; }
    public string? GeometryGeoJson { get; set; }
}
