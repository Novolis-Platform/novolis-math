namespace Novolis.Math.Geometry;

/// <summary>Measurements for geographic paths and polygons.</summary>
public static class GeoPathMetrics
{
    /// <summary>Calculates the shortest surface distance along a path.</summary>
    public static double PolylineLength(
        IReadOnlyList<GeoCoordinate> points,
        bool close = false)
    {
        ArgumentNullException.ThrowIfNull(points);
        if (points.Count < 2)
            return 0;

        var length = 0d;
        for (var index = 1; index < points.Count; index++)
            length += GeoDistance.Between(points[index - 1], points[index]);

        if (close && points[0] != points[^1])
            length += GeoDistance.Between(points[^1], points[0]);

        return length;
    }

    /// <summary>
    /// Calculates an equirectangular approximation of polygon area in square meters.
    /// This is intended for local regions, not continental-scale surveying.
    /// </summary>
    public static double PolygonAreaSquareMeters(IReadOnlyList<GeoCoordinate> points)
    {
        ArgumentNullException.ThrowIfNull(points);
        if (points.Count < 3)
            return 0;

        var count = points[0] == points[^1] ? points.Count - 1 : points.Count;
        if (count < 3)
            return 0;

        var referenceLatitude = points
            .Take(count)
            .Average(point => point.Latitude);
        var referenceLatitudeRadians = GeoDistance.ToRadians(referenceLatitude);
        var earthRadius = GeoDistance.MeanEarthRadiusMeters;
        var projected = new (double X, double Y)[count];
        var originLongitude = points[0].Longitude;

        for (var index = 0; index < count; index++)
        {
            var point = points[index];
            var longitudeDelta = NormalizeLongitudeDelta(
                point.Longitude - originLongitude);
            projected[index] = (
                earthRadius
                    * GeoDistance.ToRadians(longitudeDelta)
                    * global::System.Math.Cos(referenceLatitudeRadians),
                earthRadius
                    * GeoDistance.ToRadians(point.Latitude - referenceLatitude));
        }

        var twiceArea = 0d;
        for (var index = 0; index < count; index++)
        {
            var next = (index + 1) % count;
            twiceArea += projected[index].X * projected[next].Y
                - projected[next].X * projected[index].Y;
        }

        return global::System.Math.Abs(twiceArea) / 2;
    }

    static double NormalizeLongitudeDelta(double degrees)
    {
        var normalized = degrees % 360d;
        if (normalized > 180)
            normalized -= 360;
        if (normalized < -180)
            normalized += 360;
        return normalized;
    }
}
