using System.Numerics;

namespace Novolis.Math.Geometry;

/// <summary>
///     Cylindrical coordinates: horizontal radius, azimuth in radians (counter-clockwise from +X), and height along +Z.
/// </summary>
public readonly struct CylindricalPoint(float radius, float azimuthRadians, float height) : IEquatable<CylindricalPoint>
{
    public float Radius { get; } = radius;

    public float AzimuthRadians { get; } = azimuthRadians;

    public float Height { get; } = height;

    public float AzimuthDegrees => AzimuthRadians * Constants.MathConstants.Rad2Deg;

    public static CylindricalPoint Origin => new(0f, 0f, 0f);

    public static CylindricalPoint FromDegrees(float radius, float azimuthDegrees, float height) =>
        new(radius, azimuthDegrees * Constants.MathConstants.Deg2Rad, height);

    public Vector3 ToCartesian() =>
        new(
            Radius * MathF.Cos(AzimuthRadians),
            Radius * MathF.Sin(AzimuthRadians),
            Height);

    public static CylindricalPoint FromCartesian(Vector3 vector) =>
        new(
            MathF.Sqrt(vector.X * vector.X + vector.Y * vector.Y),
            MathF.Atan2(vector.Y, vector.X),
            vector.Z);

    public static CylindricalPoint FromCartesian(float x, float y, float z) =>
        FromCartesian(new Vector3(x, y, z));

    public bool Equals(CylindricalPoint other) =>
        Radius.Equals(other.Radius) &&
        AzimuthRadians.Equals(other.AzimuthRadians) &&
        Height.Equals(other.Height);

    public override bool Equals(object? obj) => obj is CylindricalPoint other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(Radius, AzimuthRadians, Height);

    public static bool operator ==(CylindricalPoint left, CylindricalPoint right) => left.Equals(right);

    public static bool operator !=(CylindricalPoint left, CylindricalPoint right) => !left.Equals(right);

    public override string ToString() =>
        $"r={Radius}, θ={AzimuthRadians:F4} rad ({AzimuthDegrees:F2}°), h={Height} → {ToCartesian()}";
}
