using System.Numerics;

namespace Novolis.Math.Geometry;

/// <summary>
/// Cylindrical coordinates: horizontal radius, azimuth in radians (counter-clockwise from +X), and height along +Z.
/// </summary>
public readonly struct CylindricalPoint(float radius, float azimuthRadians, float height) : IEquatable<CylindricalPoint>
{
    /// <summary>Horizontal distance from the axis.</summary>
    public float Radius { get; } = radius;

    /// <summary>Azimuth in radians (counter-clockwise from +X).</summary>
    public float AzimuthRadians { get; } = azimuthRadians;

    /// <summary>Height along +Z.</summary>
    public float Height { get; } = height;

    /// <summary>Azimuth in degrees.</summary>
    public float AzimuthDegrees => AzimuthRadians * Constants.MathConstants.Rad2Deg;

    /// <summary>Origin <c>(0, 0, 0)</c>.</summary>
    public static CylindricalPoint Origin => new(0f, 0f, 0f);

    /// <summary>Creates a point from degree-based azimuth.</summary>
    /// <param name="radius">Horizontal radius.</param>
    /// <param name="azimuthDegrees">Azimuth in degrees.</param>
    /// <param name="height">Height along +Z.</param>
    /// <returns>Cylindrical point.</returns>
    public static CylindricalPoint FromDegrees(float radius, float azimuthDegrees, float height) =>
        new(radius, azimuthDegrees * Constants.MathConstants.Deg2Rad, height);

    /// <summary>Converts to Cartesian <see cref="Vector3"/>.</summary>
    /// <returns>Cartesian position.</returns>
    public Vector3 ToCartesian() =>
        new(
            Radius * MathF.Cos(AzimuthRadians),
            Radius * MathF.Sin(AzimuthRadians),
            Height);

    /// <summary>Converts a Cartesian vector to cylindrical coordinates.</summary>
    /// <param name="vector">Cartesian position.</param>
    /// <returns>Cylindrical point.</returns>
    public static CylindricalPoint FromCartesian(Vector3 vector) =>
        new(
            MathF.Sqrt(vector.X * vector.X + vector.Y * vector.Y),
            MathF.Atan2(vector.Y, vector.X),
            vector.Z);

    /// <summary>Converts Cartesian components to cylindrical coordinates.</summary>
    /// <param name="x">X component.</param>
    /// <param name="y">Y component.</param>
    /// <param name="z">Z component.</param>
    /// <returns>Cylindrical point.</returns>
    public static CylindricalPoint FromCartesian(float x, float y, float z) =>
        FromCartesian(new Vector3(x, y, z));

    /// <inheritdoc />
    public bool Equals(CylindricalPoint other) =>
        Radius.Equals(other.Radius) &&
        AzimuthRadians.Equals(other.AzimuthRadians) &&
        Height.Equals(other.Height);

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is CylindricalPoint other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(Radius, AzimuthRadians, Height);

    /// <summary>Equality operator.</summary>
    public static bool operator ==(CylindricalPoint left, CylindricalPoint right) => left.Equals(right);

    /// <summary>Inequality operator.</summary>
    public static bool operator !=(CylindricalPoint left, CylindricalPoint right) => !left.Equals(right);

    /// <inheritdoc />
    public override string ToString() =>
        $"r={Radius}, θ={AzimuthRadians:F4} rad ({AzimuthDegrees:F2}°), h={Height} → {ToCartesian()}";
}
