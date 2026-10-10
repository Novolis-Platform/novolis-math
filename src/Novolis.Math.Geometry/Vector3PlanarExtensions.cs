using System.Numerics;

namespace Novolis.Math.Geometry;

/// <summary>XZ planar helpers on BCL <see cref="Vector3"/> (Y = 0).</summary>
public static class Vector3PlanarExtensions
{
    /// <summary>Creates a planar vector with <c>Y = 0</c>.</summary>
    /// <param name="x">X component.</param>
    /// <param name="z">Z component.</param>
    /// <returns><c>(x, 0, z)</c>.</returns>
    public static Vector3 Xz(float x, float z) => new(x, 0f, z);

    /// <summary>Replaces the Y component, preserving X and Z.</summary>
    /// <param name="v">Source vector.</param>
    /// <param name="y">New Y value.</param>
    /// <returns>Vector with updated Y.</returns>
    public static Vector3 WithY(this Vector3 v, float y) => new(v.X, y, v.Z);

    /// <summary>Projects to the XZ plane (<c>Y = 0</c>).</summary>
    /// <param name="v">Source vector.</param>
    /// <returns>Planar vector.</returns>
    public static Vector3 ToPlanar(this Vector3 v) => new(v.X, 0f, v.Z);

    /// <summary>Squared length on the XZ plane.</summary>
    public static float LengthSquaredXz(this Vector3 v) => v.X * v.X + v.Z * v.Z;

    /// <summary>Length on the XZ plane.</summary>
    public static float LengthXz(this Vector3 v) => MathF.Sqrt(v.LengthSquaredXz());

    /// <summary>XZ distance between two points.</summary>
    public static float DistanceXz(Vector3 a, Vector3 b) => (a - b).LengthXz();

    /// <summary>Unit vector on XZ, or <paramref name="fallback"/> when the length is ~0.</summary>
    public static Vector3 NormalizeXz(this Vector3 v, Vector3 fallback)
    {
        var len = v.LengthXz();
        return len < 1e-6f ? fallback.ToPlanar() : new Vector3(v.X / len, 0f, v.Z / len);
    }

    /// <summary>Unit vector on XZ, or <c>(0, 0, 1)</c> when the length is ~0.</summary>
    public static Vector3 NormalizeXz(this Vector3 v) => v.NormalizeXz(new Vector3(0f, 0f, 1f));

    /// <summary>Heading on XZ, same convention as <c>Atan2(z, x)</c>.</summary>
    public static float Atan2Xz(this Vector3 v) => MathF.Atan2(v.Z, v.X);

    /// <summary>Unit XZ vector from a heading in radians.</summary>
    public static Vector3 FromHeadingXz(float radians) =>
        new(MathF.Cos(radians), 0f, MathF.Sin(radians));
}
