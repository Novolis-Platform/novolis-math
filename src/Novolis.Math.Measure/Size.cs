namespace Novolis.Math.Measure;

/// <summary>Width and height extents as scalar lengths (not a vector type).</summary>
/// <param name="Width">Horizontal extent.</param>
/// <param name="Height">Vertical extent.</param>
public readonly record struct Size(Length Width, Length Height)
{
    /// <inheritdoc />
    public override string ToString() => $"{Width.Points}×{Height.Points}pt";
}
