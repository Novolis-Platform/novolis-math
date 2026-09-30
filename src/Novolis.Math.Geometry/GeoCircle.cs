namespace Novolis.Math.Geometry;

/// <summary>A circle on the Earth's surface with a radius in meters.</summary>
public readonly record struct GeoCircle
{
    /// <summary>Creates a terrestrial circle.</summary>
    [global::System.Text.Json.Serialization.JsonConstructor]
    public GeoCircle(GeoCoordinate center, double radiusMeters)
    {
        if (!double.IsFinite(radiusMeters) || radiusMeters < 0)
            throw new ArgumentOutOfRangeException(
                nameof(radiusMeters),
                radiusMeters,
                "Radius must be finite and non-negative.");

        Center = center;
        RadiusMeters = radiusMeters;
    }

    /// <summary>Circle center.</summary>
    public GeoCoordinate Center { get; }

    /// <summary>Circle radius in meters.</summary>
    public double RadiusMeters { get; }

    /// <summary>Returns the great-circle distance from the center to a point.</summary>
    public double DistanceTo(GeoCoordinate point) => GeoDistance.Between(Center, point);

    /// <summary>Tests exact geometric containment without an accuracy allowance.</summary>
    public bool Contains(GeoCoordinate point) => DistanceTo(point) <= RadiusMeters;

    /// <summary>Classifies a measured point using its reported horizontal accuracy in meters.</summary>
    public GeoContainmentClassification Classify(GeoCoordinate point, double accuracyMeters)
    {
        if (!double.IsFinite(accuracyMeters) || accuracyMeters < 0)
            throw new ArgumentOutOfRangeException(
                nameof(accuracyMeters),
                accuracyMeters,
                "Accuracy must be finite and non-negative.");

        var distance = DistanceTo(point);

        if (distance + accuracyMeters <= RadiusMeters)
            return GeoContainmentClassification.DefinitelyInside;

        if (distance - accuracyMeters > RadiusMeters)
            return GeoContainmentClassification.DefinitelyOutside;

        if (distance <= RadiusMeters)
            return GeoContainmentClassification.ProbablyInside;

        return GeoContainmentClassification.Uncertain;
    }
}
