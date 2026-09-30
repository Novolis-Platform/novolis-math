namespace Novolis.Math.Geometry;

/// <summary>
/// Converts terrestrial coordinates to normalized Web Mercator coordinates and back.
/// </summary>
public static class WebMercatorProjection
{
    /// <summary>The latitude limit of a square Web Mercator world.</summary>
    public const double MaximumLatitudeDegrees = 85.0511287798066d;

    /// <summary>Projects a terrestrial coordinate into the normalized range [0, 1].</summary>
    public static GeoProjectedPoint Project(GeoCoordinate coordinate)
    {
        var latitude = global::System.Math.Clamp(
            coordinate.Latitude,
            -MaximumLatitudeDegrees,
            MaximumLatitudeDegrees);
        var latitudeRadians = GeoDistance.ToRadians(latitude);
        var sine = global::System.Math.Sin(latitudeRadians);

        var x = (coordinate.Longitude + 180d) / 360d;
        var y = 0.5d - global::System.Math.Log((1d + sine) / (1d - sine))
            / (4d * global::System.Math.PI);

        return new GeoProjectedPoint(x, y);
    }

    /// <summary>Unprojects a normalized Web Mercator coordinate into latitude and longitude.</summary>
    public static GeoCoordinate Unproject(GeoProjectedPoint point)
    {
        if (!double.IsFinite(point.X) || point.X is < 0 or > 1)
            throw new ArgumentOutOfRangeException(nameof(point), point, "Projected X must be in the range [0, 1].");

        if (!double.IsFinite(point.Y) || point.Y is < 0 or > 1)
            throw new ArgumentOutOfRangeException(nameof(point), point, "Projected Y must be in the range [0, 1].");

        var longitude = point.X * 360d - 180d;
        var mercatorRadians = global::System.Math.PI * (1d - 2d * point.Y);
        var latitude = GeoDistance.ToDegrees(
            global::System.Math.Atan(global::System.Math.Sinh(mercatorRadians)));

        return new GeoCoordinate(latitude, longitude);
    }
}
