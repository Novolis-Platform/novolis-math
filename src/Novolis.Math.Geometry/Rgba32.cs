namespace Novolis.Math.Geometry;

/// <summary>
/// 8-bit RGBA color for meshes and scenes without referencing <c>System.Drawing</c>.
/// </summary>
public readonly struct Rgba32 : IEquatable<Rgba32>
{
    /// <summary>Red channel (0–255).</summary>
    public byte R { get; init; }

    /// <summary>Green channel (0–255).</summary>
    public byte G { get; init; }

    /// <summary>Blue channel (0–255).</summary>
    public byte B { get; init; }

    /// <summary>Alpha channel (0–255).</summary>
    public byte A { get; init; }

    /// <summary>Creates a color with the given RGBA components.</summary>
    /// <param name="r">Red.</param>
    /// <param name="g">Green.</param>
    /// <param name="b">Blue.</param>
    /// <param name="a">Alpha; defaults to 255.</param>
    public Rgba32(byte r, byte g, byte b, byte a = 255)
    {
        R = r;
        G = g;
        B = b;
        A = a;
    }

    /// <summary>Creates a color from ARGB component order.</summary>
    /// <param name="a">Alpha.</param>
    /// <param name="r">Red.</param>
    /// <param name="g">Green.</param>
    /// <param name="b">Blue.</param>
    /// <returns>RGBA color.</returns>
    public static Rgba32 FromArgb(byte a, byte r, byte g, byte b) => new(r, g, b, a);

    /// <summary>Opaque white.</summary>
    public static Rgba32 White => new(255, 255, 255);

    /// <summary>Opaque black.</summary>
    public static Rgba32 Black => new(0, 0, 0);

    /// <summary>Opaque red.</summary>
    public static Rgba32 Red => new(255, 0, 0);

    /// <summary>Opaque chartreuse.</summary>
    public static Rgba32 Chartreuse => new(127, 255, 0);

    /// <summary>Opaque crimson.</summary>
    public static Rgba32 Crimson => new(220, 20, 60);

    /// <inheritdoc />
    public bool Equals(Rgba32 other) => R == other.R && G == other.G && B == other.B && A == other.A;

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is Rgba32 other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(R, G, B, A);

    /// <summary>Equality operator.</summary>
    public static bool operator ==(Rgba32 left, Rgba32 right) => left.Equals(right);

    /// <summary>Inequality operator.</summary>
    public static bool operator !=(Rgba32 left, Rgba32 right) => !left.Equals(right);

    /// <inheritdoc />
    public override string ToString() => $"RGBA({R},{G},{B},{A})";
}
