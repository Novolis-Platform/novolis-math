namespace Novolis.Math.Geometry;

/// <summary>
/// A position on Earth expressed as latitude and longitude in decimal degrees.
/// </summary>
public readonly record struct GeoCoordinate
{
    /// <summary>Creates a coordinate in decimal degrees.</summary>
    /// <param name="latitude">Latitude in the inclusive range -90 through 90.</param>
    /// <param name="longitude">Longitude in the inclusive range -180 through 180.</param>
    [global::System.Text.Json.Serialization.JsonConstructor]
    public GeoCoordinate(double latitude, double longitude)
    {
        if (!double.IsFinite(latitude))
            throw new ArgumentOutOfRangeException(nameof(latitude), latitude, "Latitude must be finite.");

        if (latitude is < -90 or > 90)
            throw new ArgumentOutOfRangeException(nameof(latitude), latitude, "Latitude must be between -90 and 90 degrees.");

        if (!double.IsFinite(longitude))
            throw new ArgumentOutOfRangeException(nameof(longitude), longitude, "Longitude must be finite.");

        if (longitude is < -180 or > 180)
            throw new ArgumentOutOfRangeException(nameof(longitude), longitude, "Longitude must be between -180 and 180 degrees.");

        Latitude = latitude;
        Longitude = longitude;
    }

    /// <summary>Latitude in decimal degrees.</summary>
    public double Latitude { get; }

    /// <summary>Longitude in decimal degrees.</summary>
    public double Longitude { get; }
}
