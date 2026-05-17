using System.Numerics;

namespace Novolis.Math.Geometry;

public static class Vector3Extensions
{
    public static Vector3 Normalized(this Vector3 v) => Vector3.Normalize(v);

    public static Vector3 Multiply(this Vector3 v, double scalar) => v * (float)scalar;

    public static Vector3 Divide(this Vector3 v, double scalar) => v / (float)scalar;
}
