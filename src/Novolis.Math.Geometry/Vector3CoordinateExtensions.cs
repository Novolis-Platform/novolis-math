using System.Numerics;

namespace Novolis.Math.Geometry;

/// <summary>Cylindrical ↔ Cartesian conversions for <see cref="Vector3"/>.</summary>
public static class Vector3CoordinateExtensions
{
    /// <summary>Converts a Cartesian vector to cylindrical coordinates.</summary>
    /// <param name="vector">Cartesian position.</param>
    /// <returns>Cylindrical representation.</returns>
    public static CylindricalPoint ToCylindrical(this Vector3 vector) => CylindricalPoint.FromCartesian(vector);
}
