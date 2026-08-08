namespace Novolis.Math.Measure;

/// <summary>A one-dimensional length stored in typographic points (1/72 inch).</summary>
/// <param name="Points">Length in points.</param>
public readonly record struct Length(float Points)
{
    /// <summary>Length in inches.</summary>
    public float Inches => Points / 72f;

    /// <summary>Length in millimeters.</summary>
    public float Millimeters => Points * 25.4f / 72f;

    /// <summary>Adds two lengths.</summary>
    public static Length operator +(Length left, Length right) => new(left.Points + right.Points);

    /// <summary>Subtracts <paramref name="right"/> from <paramref name="left"/>.</summary>
    public static Length operator -(Length left, Length right) => new(left.Points - right.Points);

    /// <summary>Scales a length by a scalar.</summary>
    public static Length operator *(Length length, float factor) => new(length.Points * factor);

    /// <inheritdoc />
    public override string ToString() => $"{Points}pt";
}
