namespace Novolis.Math.Measure;

/// <summary>
/// Axis-aligned box in a planar measure space using scalar lengths
/// (page boxes, content frames). Not a vector type and not <c>Vector2</c>.
/// </summary>
/// <param name="X">Left edge.</param>
/// <param name="Y">Top edge.</param>
/// <param name="Width">Horizontal extent.</param>
/// <param name="Height">Vertical extent.</param>
public readonly record struct Rect(Length X, Length Y, Length Width, Length Height)
{
    /// <summary>Right edge (<see cref="X"/> + <see cref="Width"/>).</summary>
    public Length Right => X + Width;

    /// <summary>Bottom edge (<see cref="Y"/> + <see cref="Height"/>).</summary>
    public Length Bottom => Y + Height;

    /// <summary>Size of this rect.</summary>
    public Size Size => new(Width, Height);

    /// <summary>Creates a rect from origin and size.</summary>
    public static Rect FromSize(Length x, Length y, Size size) =>
        new(x, y, size.Width, size.Height);

    /// <summary>Insets this rect by a thickness (content box inside margins).</summary>
    public Rect Inset(Thickness thickness) =>
        new(
            X + thickness.Left,
            Y + thickness.Top,
            Width - thickness.Horizontal,
            Height - thickness.Vertical);
}
