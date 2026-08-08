namespace Novolis.Math.Measure;

/// <summary>Converters into <see cref="Length"/> (canonical storage is points).</summary>
public static class LengthUnits
{
    /// <summary>Creates a length from typographic points.</summary>
    public static Length FromPoints(float points) => new(points);

    /// <summary>Creates a length from inches (1 inch = 72 points).</summary>
    public static Length FromInches(float inches) => new(inches * 72f);

    /// <summary>Creates a length from millimeters.</summary>
    public static Length FromMillimeters(float millimeters) => new(millimeters * 72f / 25.4f);
}
