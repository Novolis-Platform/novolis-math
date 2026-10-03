namespace Novolis.Math.Geometry;

/// <summary>A geographic rectangle defined by two opposite corners.</summary>
public readonly record struct GeoRectangle
{
    /// <summary>Creates a rectangle from any two opposite corners.</summary>
    public GeoRectangle(GeoCoordinate first, GeoCoordinate opposite)
    {
        First = first;
        Opposite = opposite;
    }

    /// <summary>First corner selected by the user.</summary>
    public GeoCoordinate First { get; }

    /// <summary>Corner opposite <see cref="First" /> selected by the user.</summary>
    public GeoCoordinate Opposite { get; }

    /// <summary>Four corners in a connected winding order.</summary>
    public IReadOnlyList<GeoCoordinate> Corners =>
    [
        new GeoCoordinate(global::System.Math.Max(First.Latitude, Opposite.Latitude), First.Longitude),
        new GeoCoordinate(global::System.Math.Max(First.Latitude, Opposite.Latitude), Opposite.Longitude),
        new GeoCoordinate(global::System.Math.Min(First.Latitude, Opposite.Latitude), Opposite.Longitude),
        new GeoCoordinate(global::System.Math.Min(First.Latitude, Opposite.Latitude), First.Longitude),
    ];

    /// <summary>Four corners followed by the first corner for closed-path consumers.</summary>
    public IReadOnlyList<GeoCoordinate> ClosedCorners
    {
        get
        {
            var corners = Corners;
            return [.. corners, corners[0]];
        }
    }

    /// <summary>Geodesic perimeter in meters.</summary>
    public double PerimeterMeters => GeoPathMetrics.PolylineLength(Corners, close: true);

    /// <summary>Locally accurate surface-area approximation in square meters.</summary>
    public double AreaSquareMeters => GeoPathMetrics.PolygonAreaSquareMeters(Corners);
}
