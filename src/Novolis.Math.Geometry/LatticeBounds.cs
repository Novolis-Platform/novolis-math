namespace Novolis.Math.Geometry;

/// <summary>Axis-aligned integer bounds (origin plus size).</summary>
public readonly struct LatticeBounds(int x, int y, int z, int width, int height, int depth) : IEquatable<LatticeBounds>
{
    /// <summary>Origin X (inclusive).</summary>
    public int X { get; } = x;

    /// <summary>Origin Y (inclusive).</summary>
    public int Y { get; } = y;

    /// <summary>Origin Z (inclusive).</summary>
    public int Z { get; } = z;

    /// <summary>Extent along X.</summary>
    public int Width { get; } = width;

    /// <summary>Extent along Y.</summary>
    public int Height { get; } = height;

    /// <summary>Extent along Z.</summary>
    public int Depth { get; } = depth;

    /// <inheritdoc />
    public bool Equals(LatticeBounds other) =>
        X == other.X && Y == other.Y && Z == other.Z &&
        Width == other.Width && Height == other.Height && Depth == other.Depth;

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is LatticeBounds other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(X, Y, Z, Width, Height, Depth);

    /// <summary>Equality operator.</summary>
    public static bool operator ==(LatticeBounds left, LatticeBounds right) => left.Equals(right);

    /// <summary>Inequality operator.</summary>
    public static bool operator !=(LatticeBounds left, LatticeBounds right) => !left.Equals(right);
}
