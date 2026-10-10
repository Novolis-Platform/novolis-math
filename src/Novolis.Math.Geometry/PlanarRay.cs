using System.Numerics;

namespace Novolis.Math.Geometry;

/// <summary>Ray vs segment and circle queries on the XZ plane (Y ignored).</summary>
public static class PlanarRay
{
    /// <summary>Hits a finite segment. Normal faces the incoming ray.</summary>
    public static bool TryHitSegment(
        Vector3 origin,
        Vector3 direction,
        float length,
        Vector3 a,
        Vector3 b,
        out PlanarHit hit)
    {
        hit = default;
        var rdx = direction.X;
        var rdz = direction.Z;
        var sdx = b.X - a.X;
        var sdz = b.Z - a.Z;
        var denom = rdx * sdz - rdz * sdx;
        if (MathF.Abs(denom) < 1e-6f)
        {
            return false;
        }

        var ax = a.X - origin.X;
        var az = a.Z - origin.Z;
        var t = (ax * sdz - az * sdx) / denom;
        var u = (ax * rdz - az * rdx) / denom;
        if (t < 0f || t > length || u is < 0f or > 1f)
        {
            return false;
        }

        var nx = -sdz;
        var nz = sdx;
        var nlen = MathF.Sqrt(nx * nx + nz * nz);
        if (nlen < 1e-6f)
        {
            return false;
        }

        nx /= nlen;
        nz /= nlen;
        if (nx * rdx + nz * rdz > 0f)
        {
            nx = -nx;
            nz = -nz;
        }

        hit = new PlanarHit(origin + direction * t, new Vector3(nx, 0f, nz), t);
        return true;
    }

    /// <summary>Hits a circle on XZ. Returns the nearer entry distance.</summary>
    public static bool TryHitCircle(
        Vector3 origin,
        Vector3 direction,
        float length,
        Vector3 center,
        float radius,
        out float distance)
    {
        var fx = origin.X - center.X;
        var fz = origin.Z - center.Z;
        var a = direction.X * direction.X + direction.Z * direction.Z;
        var b = 2f * (fx * direction.X + fz * direction.Z);
        var c = fx * fx + fz * fz - radius * radius;
        var disc = b * b - 4f * a * c;
        distance = 0f;
        if (disc < 0f || a < 1e-8f)
        {
            return false;
        }

        var t = (-b - MathF.Sqrt(disc)) / (2f * a);
        if (t < 0f || t > length)
        {
            return false;
        }

        distance = t;
        return true;
    }
}
