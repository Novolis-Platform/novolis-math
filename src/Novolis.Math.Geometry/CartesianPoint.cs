using System.Numerics;

namespace Novolis.Math.Geometry;

/// <summary>
///     A point in the Cartesian plane: <c>(x, y)</c>.
/// </summary>
/// <remarks>
///     Interoperates with <see cref="Vector2"/> and converts to <see cref="PolarPoint"/> via
///     <c>r = √(x² + y²)</c> and <c>θ = atan2(y, x)</c>.
/// </remarks>
public readonly struct CartesianPoint(float x, float y) : IEquatable<CartesianPoint>
{
    /// <summary>Horizontal coordinate.</summary>
    public float X { get; } = x;

    /// <summary>Vertical coordinate.</summary>
    public float Y { get; } = y;

    public static CartesianPoint Zero => new(0f, 0f);

    public static CartesianPoint FromVector2(Vector2 vector) => new(vector.X, vector.Y);

    public Vector2 ToVector2() => new(X, Y);

    public PolarPoint ToPolar() => PolarPoint.FromCartesian(this);

    public bool Equals(CartesianPoint other) => X.Equals(other.X) && Y.Equals(other.Y);

    public override bool Equals(object? obj) => obj is CartesianPoint other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(X, Y);

    public static bool operator ==(CartesianPoint left, CartesianPoint right) => left.Equals(right);

    public static bool operator !=(CartesianPoint left, CartesianPoint right) => !left.Equals(right);

    public static implicit operator Vector2(CartesianPoint point) => point.ToVector2();

    public static implicit operator CartesianPoint(Vector2 vector) => FromVector2(vector);

    public override string ToString() => $"({X}, {Y})";
}
