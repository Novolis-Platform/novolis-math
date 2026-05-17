using System.Numerics;

namespace Novolis.Math.Geometry;

/// <summary>Line segment from <see cref="A"/> to <see cref="B"/> with radius around the segment.</summary>
public readonly struct Capsule(Vector3 a, Vector3 b, float radius)
{
    public Vector3 A { get; } = a;
    public Vector3 B { get; } = b;
    public float Radius { get; } = radius;
}
