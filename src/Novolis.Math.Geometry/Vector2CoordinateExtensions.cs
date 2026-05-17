using System.Numerics;

namespace Novolis.Math.Geometry;

/// <summary>
///     Polar ↔ Cartesian conversions for <see cref="Vector2"/>.
/// </summary>
public static class Vector2CoordinateExtensions
{
    /// <summary>Converts a Cartesian <see cref="Vector2"/> to polar form.</summary>
    public static PolarPoint ToPolar(this Vector2 vector) => PolarPoint.FromCartesian(vector);
}
