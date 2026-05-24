using System.Numerics;

namespace Novolis.Math.Geometry;

/// <summary>Möller–Trumbore ray–triangle intersection.</summary>
public static class TriangleRay
{
    /// <summary>
    /// Tests whether <paramref name="ray"/> hits the triangle <paramref name="v0"/>–<paramref name="v2"/> within
    /// <paramref name="maxDistance"/>.
    /// </summary>
    /// <param name="ray">World-space ray (unit direction recommended).</param>
    /// <param name="v0">First triangle vertex.</param>
    /// <param name="v1">Second triangle vertex.</param>
    /// <param name="v2">Third triangle vertex.</param>
    /// <param name="maxDistance">Maximum hit distance along the ray.</param>
    /// <param name="distance">Hit distance when returning <see langword="true"/>.</param>
    /// <param name="normal">Face normal oriented against the ray direction.</param>
    /// <returns><see langword="true"/> if the ray intersects the triangle.</returns>
    public static bool TryHit(in Ray ray, Vector3 v0, Vector3 v1, Vector3 v2, float maxDistance, out float distance, out Vector3 normal)
    {
        distance = 0f;
        var e1 = v1 - v0;
        var e2 = v2 - v0;
        normal = Vector3.Normalize(Vector3.Cross(e1, e2));

        var epsilon = GeometryConstants.RayTriangleEpsilon;
        var h = Vector3.Cross(ray.Direction, e2);
        var a = Vector3.Dot(e1, h);
        if (MathF.Abs(a) < epsilon)
        {
            return false;
        }

        var f = 1f / a;
        var s = ray.Origin - v0;
        var u = f * Vector3.Dot(s, h);
        if (u is < 0f or > 1f)
        {
            return false;
        }

        var q = Vector3.Cross(s, e1);
        var v = f * Vector3.Dot(ray.Direction, q);
        if (v < 0f || u + v > 1f)
        {
            return false;
        }

        var t = f * Vector3.Dot(e2, q);
        if (t <= epsilon || t > maxDistance)
        {
            return false;
        }

        distance = t;
        if (Vector3.Dot(normal, ray.Direction) > 0f)
        {
            normal = -normal;
        }

        return true;
    }
}
