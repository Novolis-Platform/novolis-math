namespace Novolis.Math.Geometry;

/// <summary>Great-circle distance calculations for terrestrial coordinates.</summary>
public static class GeoDistance
{
    /// <summary>
    /// Mean Earth radius used by the spherical great-circle approximation, in meters.
    /// </summary>
    public const double MeanEarthRadiusMeters = 6_371_000d;

    /// <summary>Calculates the shortest surface distance between two coordinates.</summary>
    public static double Between(
        GeoCoordinate from,
        GeoCoordinate to,
        double earthRadiusMeters = MeanEarthRadiusMeters)
    {
        if (!double.IsFinite(earthRadiusMeters) || earthRadiusMeters <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(earthRadiusMeters),
                earthRadiusMeters,
                "Earth radius must be finite and greater than zero.");

        var latitudeDelta = ToRadians(to.Latitude - from.Latitude);
        var longitudeDelta = ToRadians(to.Longitude - from.Longitude);
        var fromLatitude = ToRadians(from.Latitude);
        var toLatitude = ToRadians(to.Latitude);

        var halfChord = global::System.Math.Pow(global::System.Math.Sin(latitudeDelta / 2), 2)
            + global::System.Math.Cos(fromLatitude)
            * global::System.Math.Cos(toLatitude)
            * global::System.Math.Pow(global::System.Math.Sin(longitudeDelta / 2), 2);

        var clampedHalfChord = global::System.Math.Clamp(halfChord, 0d, 1d);
        var centralAngle = 2 * global::System.Math.Atan2(
            global::System.Math.Sqrt(clampedHalfChord),
            global::System.Math.Sqrt(1 - clampedHalfChord));

        return earthRadiusMeters * centralAngle;
    }

    internal static double ToRadians(double degrees) => degrees * global::System.Math.PI / 180d;

    internal static double ToDegrees(double radians) => radians * 180d / global::System.Math.PI;
}
