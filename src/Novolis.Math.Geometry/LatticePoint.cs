namespace Novolis.Math.Geometry;

/// <summary>Integer lattice coordinates for volumetric grids and bounds.</summary>
public readonly struct LatticePoint(int x, int y, int z) : IEquatable<LatticePoint>
{
    /// <summary>X coordinate.</summary>
    public int X { get; } = x;

    /// <summary>Y coordinate.</summary>
    public int Y { get; } = y;

    /// <summary>Z coordinate.</summary>
    public int Z { get; } = z;

    /// <inheritdoc />
    public bool Equals(LatticePoint other) => X == other.X && Y == other.Y && Z == other.Z;

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is LatticePoint other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(X, Y, Z);

    /// <summary>Equality operator.</summary>
    public static bool operator ==(LatticePoint left, LatticePoint right) => left.Equals(right);

    /// <summary>Inequality operator.</summary>
    public static bool operator !=(LatticePoint left, LatticePoint right) => !left.Equals(right);
}
