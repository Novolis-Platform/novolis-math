using System.Numerics;

namespace Novolis.Math.Geometry;

/// <summary>Axis-aligned bounding box from inclusive min/max corners.</summary>
public readonly struct AxisAlignedBox3(Vector3 min, Vector3 max)
{
    public Vector3 Min { get; } = min;
    public Vector3 Max { get; } = max;

    public static AxisAlignedBox3 FromMinMax(Vector3 min, Vector3 max) =>
        new(
            new Vector3(MathF.Min(min.X, max.X), MathF.Min(min.Y, max.Y), MathF.Min(min.Z, max.Z)),
            new Vector3(MathF.Max(min.X, max.X), MathF.Max(min.Y, max.Y), MathF.Max(min.Z, max.Z)));

    public static AxisAlignedBox3 Expand(AxisAlignedBox3 box, Vector3 point) =>
        FromMinMax(
            new Vector3(MathF.Min(box.Min.X, point.X), MathF.Min(box.Min.Y, point.Y), MathF.Min(box.Min.Z, point.Z)),
            new Vector3(MathF.Max(box.Max.X, point.X), MathF.Max(box.Max.Y, point.Y), MathF.Max(box.Max.Z, point.Z)));

    public bool Intersects(AxisAlignedBox3 other) =>
        Min.X <= other.Max.X && Max.X >= other.Min.X
        && Min.Y <= other.Max.Y && Max.Y >= other.Min.Y
        && Min.Z <= other.Max.Z && Max.Z >= other.Min.Z;

    public bool Contains(Vector3 p) =>
        p.X >= Min.X && p.X <= Max.X && p.Y >= Min.Y && p.Y <= Max.Y && p.Z >= Min.Z && p.Z <= Max.Z;
}
