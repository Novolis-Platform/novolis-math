namespace Novolis.Math.Geometry;

/// <summary>Derived geometry measurements for a completed drawing.</summary>
public readonly record struct GeoDrawingStatistics(
    int VertexCount,
    double LengthMeters,
    double AreaSquareMeters,
    double? RadiusMeters);
