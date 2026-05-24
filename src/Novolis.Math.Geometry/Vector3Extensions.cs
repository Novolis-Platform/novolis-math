using System.Numerics;

namespace Novolis.Math.Geometry;

/// <summary>Convenience extensions on BCL <see cref="Vector3"/>.</summary>
public static class Vector3Extensions
{
    /// <summary>Returns a unit vector in the same direction as <paramref name="v"/>.</summary>
    /// <param name="v">Source vector.</param>
    /// <returns>Normalized vector.</returns>
    public static Vector3 Normalized(this Vector3 v) => Vector3.Normalize(v);

    /// <summary>Scales <paramref name="v"/> by a <see cref="double"/> factor (cast to <c>float</c>).</summary>
    /// <param name="v">Source vector.</param>
    /// <param name="scalar">Scale factor.</param>
    /// <returns>Scaled vector.</returns>
    public static Vector3 Multiply(this Vector3 v, double scalar) => v * (float)scalar;

    /// <summary>Divides <paramref name="v"/> by a <see cref="double"/> divisor (cast to <c>float</c>).</summary>
    /// <param name="v">Source vector.</param>
    /// <param name="scalar">Divisor.</param>
    /// <returns>Scaled vector.</returns>
    public static Vector3 Divide(this Vector3 v, double scalar) => v / (float)scalar;
}
