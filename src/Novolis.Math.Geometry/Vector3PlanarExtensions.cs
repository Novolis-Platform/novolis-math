using System.Numerics;

namespace Novolis.Math.Geometry;

/// <summary>XZ planar helpers on BCL <see cref="Vector3"/> (Y = 0).</summary>
public static class Vector3PlanarExtensions
{
    public static Vector3 Xz(float x, float z) => new(x, 0f, z);

    public static Vector3 WithY(this Vector3 v, float y) => new(v.X, y, v.Z);

    public static Vector3 ToPlanar(this Vector3 v) => new(v.X, 0f, v.Z);
}
