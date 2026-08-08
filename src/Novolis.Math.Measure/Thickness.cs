namespace Novolis.Math.Measure;

/// <summary>Four-sided inset or margin thicknesses.</summary>
/// <param name="Left">Left edge.</param>
/// <param name="Top">Top edge.</param>
/// <param name="Right">Right edge.</param>
/// <param name="Bottom">Bottom edge.</param>
public readonly record struct Thickness(Length Left, Length Top, Length Right, Length Bottom)
{
    /// <summary>Creates a uniform thickness on all sides.</summary>
    public static Thickness Uniform(Length all) => new(all, all, all, all);

    /// <summary>Creates a thickness with equal horizontal and equal vertical sides.</summary>
    public static Thickness Symmetric(Length horizontal, Length vertical) =>
        new(horizontal, vertical, horizontal, vertical);

    /// <summary>Sum of left and right.</summary>
    public Length Horizontal => Left + Right;

    /// <summary>Sum of top and bottom.</summary>
    public Length Vertical => Top + Bottom;
}
