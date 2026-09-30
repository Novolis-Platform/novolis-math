using System.Numerics;

namespace Novolis.Math.Geometry;

/// <summary>Control sphere for an <see cref="AdaptiveMesh"/> (position + influence radius).</summary>
public readonly struct AdaptiveMeshHandle
{
    /// <summary>Creates a handle at <paramref name="position"/> with <paramref name="radius"/>.</summary>
    public AdaptiveMeshHandle(Vector3 position, float radius)
    {
        if (radius <= 0f)
            throw new ArgumentOutOfRangeException(nameof(radius), radius, "Radius must be positive.");
        Position = position;
        Radius = radius;
    }

    /// <summary>World-space center.</summary>
    public Vector3 Position { get; }

    /// <summary>Influence / surface radius (meters).</summary>
    public float Radius { get; }

    /// <summary>Returns a copy with a new position.</summary>
    public AdaptiveMeshHandle WithPosition(Vector3 position) => new(position, Radius);

    /// <summary>Returns a copy with a new radius.</summary>
    public AdaptiveMeshHandle WithRadius(float radius) => new(Position, radius);
}
