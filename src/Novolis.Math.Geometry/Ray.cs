using System.Numerics;

namespace Novolis.Math.Geometry;

/// <summary>Ray from <see cref="Origin"/> along <see cref="Direction"/> (prefer unit direction for distance semantics).</summary>
public readonly struct Ray(Vector3 origin, Vector3 direction)
{
    /// <summary>Ray origin in world space.</summary>
    public Vector3 Origin { get; } = origin;

    /// <summary>Should be unit direction for distance semantics in raycasts.</summary>
    public Vector3 Direction { get; } = direction;

    /// <summary>Returns <see cref="Origin"/> + <see cref="Direction"/> × <paramref name="distance"/>.</summary>
    /// <param name="distance">Distance along the ray.</param>
    /// <returns>World-space point on the ray.</returns>
    public Vector3 PointAt(float distance) => Origin + Direction * distance;
}
