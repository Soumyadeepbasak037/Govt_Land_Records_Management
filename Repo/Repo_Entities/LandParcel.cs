using System;
using NetTopologySuite.Geometries;

namespace GovtLandService.Data.Entities;

public class LandParcel
{
    public long Id { get; set; }
    public long ProjectId { get; set; }
    public string ParcelNumber { get; set; } = string.Empty;
    public string? SurveyNumber { get; set; }
    public long? VillageId { get; set; }
    public decimal Area { get; set; }
    public string? OwnershipStatus { get; set; }
    public string AcquisitionStatus { get; set; } = "PROPOSED";
    public MultiPolygon? Geometry { get; set; }
    public string? GeometryGeoJson { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}
