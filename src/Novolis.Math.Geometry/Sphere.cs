using System.Numerics;

namespace Novolis.Math.Geometry;

/// <summary>Sphere with center and radius in world space.</summary>
public readonly struct Sphere(Vector3 center, float radius)
{
    /// <summary>Center in world space.</summary>
    public Vector3 Center { get; } = center;

    /// <summary>Radius (non-negative).</summary>
    public float Radius { get; } = radius;
}
