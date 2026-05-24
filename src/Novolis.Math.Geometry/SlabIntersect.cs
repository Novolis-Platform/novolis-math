using System.Numerics;

namespace Novolis.Math.Geometry;

/// <summary>Shared axis-aligned slab ray intersection (CPU and ILGPU callers).</summary>
public static class SlabIntersect
{
    /// <summary>Tests ray intersection with an axis-aligned box in [tMin, tMax].</summary>
    public static bool RayBox(
        Vector3 boxMin,
        Vector3 boxMax,
        Vector3 origin,
        Vector3 direction,
        float tMin,
        float tMax,
        float epsilon = 1e-15f)
    {
        var tEnter = tMin;
        var tExit = tMax;
        for (var axis = 0; axis < 3; axis++)
        {
            var o = axis == 0 ? origin.X : axis == 1 ? origin.Y : origin.Z;
            var d = axis == 0 ? direction.X : axis == 1 ? direction.Y : direction.Z;
            var min = axis == 0 ? boxMin.X : axis == 1 ? boxMin.Y : boxMin.Z;
            var max = axis == 0 ? boxMax.X : axis == 1 ? boxMax.Y : boxMax.Z;
            if (MathF.Abs(d) < epsilon)
            {
                if (o < min || o > max)
                    return false;
                continue;
            }

            var invD = 1f / d;
            var t0 = (min - o) * invD;
            var t1 = (max - o) * invD;
            if (t0 > t1)
                (t0, t1) = (t1, t0);

            tEnter = MathF.Max(tEnter, t0);
            tExit = MathF.Min(tExit, t1);
            if (tEnter > tExit)
                return false;
        }

        return tEnter <= tMax && tExit >= tMin;
    }
}
