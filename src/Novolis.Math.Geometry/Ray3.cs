using System.Numerics;

namespace Novolis.Math.Geometry;

/// <summary>Ray from <see cref="Origin"/> along <see cref="Direction"/> (prefer unit direction for distance semantics).</summary>
public readonly struct Ray3(Vector3 origin, Vector3 direction)
{
    public Vector3 Origin { get; } = origin;

    /// <summary>Should be unit direction for distance semantics in raycasts.</summary>
    public Vector3 Direction { get; } = direction;

    public Vector3 PointAt(float distance) => Origin + Direction * distance;
}
