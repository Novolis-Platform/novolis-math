namespace Novolis.Math.Geometry;

/// <summary>Axis-aligned integer bounds (origin plus size).</summary>
public readonly struct LatticeBounds(int x, int y, int z, int width, int height, int depth) : IEquatable<LatticeBounds>
{
    public int X { get; } = x;

    public int Y { get; } = y;

    public int Z { get; } = z;

    public int Width { get; } = width;

    public int Height { get; } = height;

    public int Depth { get; } = depth;

    public bool Equals(LatticeBounds other) =>
        X == other.X && Y == other.Y && Z == other.Z &&
        Width == other.Width && Height == other.Height && Depth == other.Depth;

    public override bool Equals(object? obj) => obj is LatticeBounds other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(X, Y, Z, Width, Height, Depth);

    public static bool operator ==(LatticeBounds left, LatticeBounds right) => left.Equals(right);

    public static bool operator !=(LatticeBounds left, LatticeBounds right) => !left.Equals(right);
}
