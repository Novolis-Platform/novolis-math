using System.Numerics;

namespace Novolis.Math.Geometry;

/// <summary>
///     A point in polar form: radius <c>r</c> and angle <c>θ</c> in radians, measured counter-clockwise from +X.
/// </summary>
/// <remarks>
///     Cartesian conversion (standard math convention):
///     <list type="bullet">
///         <item><c>x = r · cos(θ)</c></item>
///         <item><c>y = r · sin(θ)</c></item>
///     </list>
///     Use <see cref="FromDegrees"/> when the angle is given in degrees (game sprite / camera conventions).
/// </remarks>
public readonly struct PolarPoint(float radius, float angleRadians) : IEquatable<PolarPoint>
{
    /// <summary>Distance from the origin.</summary>
    public float Radius { get; } = radius;

    /// <summary>Angle in radians, counter-clockwise from +X.</summary>
    public float AngleRadians { get; } = angleRadians;

    /// <summary>Angle in degrees (<c>θ° = θ · 180 / π</c>).</summary>
    public float AngleDegrees => AngleRadians * Constants.MathConstants.Rad2Deg;

    public static PolarPoint Origin => new(0f, 0f);

    public static PolarPoint FromDegrees(float radius, float angleDegrees) =>
        new(radius, angleDegrees * Constants.MathConstants.Deg2Rad);

    /// <summary>
    ///     Converts to Cartesian coordinates: <c>(r cos θ, r sin θ)</c>.
    /// </summary>
    public CartesianPoint ToCartesian() =>
        new(Radius * MathF.Cos(AngleRadians), Radius * MathF.Sin(AngleRadians));

    public Vector2 ToVector2() => ToCartesian().ToVector2();

    public static PolarPoint FromCartesian(CartesianPoint point) => FromCartesian(point.X, point.Y);

    public static PolarPoint FromCartesian(Vector2 vector) => FromCartesian(vector.X, vector.Y);

    /// <summary>
    ///     Converts from Cartesian coordinates: <c>r = √(x² + y²)</c>, <c>θ = atan2(y, x)</c>.
    /// </summary>
    public static PolarPoint FromCartesian(float x, float y) =>
        new(MathF.Sqrt(x * x + y * y), MathF.Atan2(y, x));

    public bool Equals(PolarPoint other) =>
        Radius.Equals(other.Radius) && AngleRadians.Equals(other.AngleRadians);

    public override bool Equals(object? obj) => obj is PolarPoint other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(Radius, AngleRadians);

    public static bool operator ==(PolarPoint left, PolarPoint right) => left.Equals(right);

    public static bool operator !=(PolarPoint left, PolarPoint right) => !left.Equals(right);

    public override string ToString() =>
        $"r={Radius}, θ={AngleRadians:F4} rad ({AngleDegrees:F2}°) → {ToCartesian()}";
}
