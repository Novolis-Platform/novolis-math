using System.Numerics;

namespace Novolis.Math.Geometry;

/// <summary>Axis-aligned bounding box from inclusive min/max corners.</summary>
public readonly struct AxisAlignedBox(Vector3 min, Vector3 max)
{
    /// <summary>Minimum corner (inclusive).</summary>
    public Vector3 Min { get; } = min;

    /// <summary>Maximum corner (inclusive).</summary>
    public Vector3 Max { get; } = max;

    /// <summary>Creates a box with components ordered so <see cref="Min"/> ≤ <see cref="Max"/> per axis.</summary>
    /// <param name="min">First corner (not required to be component-wise minimum).</param>
    /// <param name="max">Second corner.</param>
    /// <returns>Normalized axis-aligned box.</returns>
    public static AxisAlignedBox FromMinMax(Vector3 min, Vector3 max) =>
        new(
            new Vector3(MathF.Min(min.X, max.X), MathF.Min(min.Y, max.Y), MathF.Min(min.Z, max.Z)),
            new Vector3(MathF.Max(min.X, max.X), MathF.Max(min.Y, max.Y), MathF.Max(min.Z, max.Z)));

    /// <summary>Grows <paramref name="box"/> to include <paramref name="point"/>.</summary>
    /// <param name="box">Starting bounds.</param>
    /// <param name="point">Point to enclose.</param>
    /// <returns>Expanded box.</returns>
    public static AxisAlignedBox Expand(AxisAlignedBox box, Vector3 point) =>
        FromMinMax(
            new Vector3(MathF.Min(box.Min.X, point.X), MathF.Min(box.Min.Y, point.Y), MathF.Min(box.Min.Z, point.Z)),
            new Vector3(MathF.Max(box.Max.X, point.X), MathF.Max(box.Max.Y, point.Y), MathF.Max(box.Max.Z, point.Z)));

    /// <summary>Tests overlap with another axis-aligned box.</summary>
    /// <param name="other">Other box.</param>
    /// <returns><see langword="true"/> if the volumes intersect.</returns>
    public bool Intersects(AxisAlignedBox other) =>
        Min.X <= other.Max.X && Max.X >= other.Min.X
        && Min.Y <= other.Max.Y && Max.Y >= other.Min.Y
        && Min.Z <= other.Max.Z && Max.Z >= other.Min.Z;

    /// <summary>Tests whether <paramref name="p"/> lies inside the closed box.</summary>
    /// <param name="p">Point in world space.</param>
    /// <returns><see langword="true"/> if contained.</returns>
    public bool Contains(Vector3 p) =>
        p.X >= Min.X && p.X <= Max.X && p.Y >= Min.Y && p.Y <= Max.Y && p.Z >= Min.Z && p.Z <= Max.Z;

    /// <summary>
    /// Computes the ray interval where <paramref name="ray"/> intersects this box (slab method).
    /// </summary>
    /// <param name="ray">World-space ray.</param>
    /// <param name="minT">Minimum parametric distance along the ray.</param>
    /// <param name="maxT">Maximum parametric distance along the ray.</param>
    /// <param name="tEnter">Entry distance along the ray.</param>
    /// <param name="tExit">Exit distance along the ray.</param>
    /// <returns><see langword="true"/> if the ray intersects the box within the interval.</returns>
    public bool RayInterval(in Ray ray, float minT, float maxT, out float tEnter, out float tExit) =>
        RayInterval(this, ray.Origin, ray.Direction, minT, maxT, out tEnter, out tExit);

    /// <summary>
    /// Computes the ray interval where a ray intersects an axis-aligned box (slab method).
    /// </summary>
    /// <param name="box">Axis-aligned bounds.</param>
    /// <param name="origin">Ray origin.</param>
    /// <param name="direction">Ray direction.</param>
    /// <param name="minT">Minimum parametric distance.</param>
    /// <param name="maxT">Maximum parametric distance.</param>
    /// <param name="tEnter">Entry distance along the ray.</param>
    /// <param name="tExit">Exit distance along the ray.</param>
    /// <returns><see langword="true"/> if the ray intersects the box within the interval.</returns>
    public static bool RayInterval(
        AxisAlignedBox box,
        Vector3 origin,
        Vector3 direction,
        float minT,
        float maxT,
        out float tEnter,
        out float tExit)
    {
        tEnter = minT;
        tExit = maxT;
        for (var axis = 0; axis < 3; axis++)
        {
            var o = axis == 0 ? origin.X : axis == 1 ? origin.Y : origin.Z;
            var d = axis == 0 ? direction.X : axis == 1 ? direction.Y : direction.Z;
            var min = axis == 0 ? box.Min.X : axis == 1 ? box.Min.Y : box.Min.Z;
            var max = axis == 0 ? box.Max.X : axis == 1 ? box.Max.Y : box.Max.Z;
            if (MathF.Abs(d) < GeometryConstants.RayParallelEpsilon)
            {
                if (o < min || o > max)
                {
                    return false;
                }

                continue;
            }

            var invD = 1f / d;
            var t0 = (min - o) * invD;
            var t1 = (max - o) * invD;
            if (t0 > t1)
            {
                (t0, t1) = (t1, t0);
            }

            tEnter = MathF.Max(tEnter, t0);
            tExit = MathF.Min(tExit, t1);
            if (tEnter > tExit)
            {
                return false;
            }
        }

        return true;
    }
}
