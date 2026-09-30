namespace Novolis.Math.Geometry;

/// <summary>Classifies a measured point against a circle while accounting for reported accuracy.</summary>
public enum GeoContainmentClassification
{
    /// <summary>The complete reported uncertainty interval is inside the circle.</summary>
    DefinitelyInside,

    /// <summary>The measured point is inside, but the uncertainty reaches the boundary.</summary>
    ProbablyInside,

    /// <summary>The uncertainty interval overlaps the circle boundary.</summary>
    Uncertain,

    /// <summary>The complete reported uncertainty interval is outside the circle.</summary>
    DefinitelyOutside,
}
