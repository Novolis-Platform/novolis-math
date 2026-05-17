using System.Numerics;

namespace Novolis.Math.Geometry;

/// <summary>Sphere with center and radius in world space.</summary>
public readonly struct Sphere3(Vector3 center, float radius)
{
    public Vector3 Center { get; } = center;
    public float Radius { get; } = radius;
}
