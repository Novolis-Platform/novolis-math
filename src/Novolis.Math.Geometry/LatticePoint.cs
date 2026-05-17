namespace Novolis.Math.Geometry;

/// <summary>Integer lattice coordinates for volumetric grids and bounds.</summary>
public readonly struct LatticePoint(int x, int y, int z) : IEquatable<LatticePoint>
{
    public int X { get; } = x;

    public int Y { get; } = y;

    public int Z { get; } = z;

    public bool Equals(LatticePoint other) => X == other.X && Y == other.Y && Z == other.Z;

    public override bool Equals(object? obj) => obj is LatticePoint other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(X, Y, Z);

    public static bool operator ==(LatticePoint left, LatticePoint right) => left.Equals(right);

    public static bool operator !=(LatticePoint left, LatticePoint right) => !left.Equals(right);
}
