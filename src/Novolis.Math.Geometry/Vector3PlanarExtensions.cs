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
}
