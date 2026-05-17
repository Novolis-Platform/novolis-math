using System.Numerics;

namespace Novolis.Math.Geometry;

/// <summary>Cylindrical ↔ Cartesian conversions for <see cref="Vector3"/>.</summary>
public static class Vector3CoordinateExtensions
{
    public static CylindricalPoint ToCylindrical(this Vector3 vector) => CylindricalPoint.FromCartesian(vector);
}
