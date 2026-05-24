namespace Novolis.Math.Arrays;

/// <summary>
/// Zero-based cell coordinates for a <see cref="DenseGrid{T}"/>, with optional spreadsheet-style letter indices.
/// </summary>
public readonly record struct GridIndex
{
    /// <summary>Column index (X axis).</summary>
    public uint X { get; }

    /// <summary>Row index (Y axis).</summary>
    public uint Y { get; }

    /// <summary>Depth slice index (Z axis).</summary>
    public uint Z { get; }

    /// <summary>Creates a zero-based index from numeric coordinates.</summary>
    /// <param name="x">Column index.</param>
    /// <param name="y">Row index.</param>
    /// <param name="z">Depth slice; defaults to <c>0</c>.</param>
    public GridIndex(uint x, uint y, uint z = 0)
    {
        X = x;
        Y = y;
        Z = z;
    }

    /// <summary>Creates an index from spreadsheet-style column and row labels (lowercase letters, zero-based).</summary>
    /// <param name="x">Column label (e.g. <c>"a"</c>, <c>"ab"</c>).</param>
    /// <param name="y">Row label.</param>
    /// <param name="z">Depth slice; defaults to <c>0</c>.</param>
    public GridIndex(string x, string y, uint z = 0)
    {
        X = ConvertToZeroBasedIndex(x);
        Y = ConvertToZeroBasedIndex(y);
        Z = z;
    }

    /// <summary>Creates an index with a numeric column and a letter row label.</summary>
    /// <param name="x">Column index.</param>
    /// <param name="y">Row label.</param>
    /// <param name="z">Depth slice; defaults to <c>0</c>.</param>
    public GridIndex(uint x, string y, uint z = 0)
    {
        X = x;
        Y = ConvertToZeroBasedIndex(y);
        Z = z;
    }

    /// <summary>Creates an index with a letter column label and a numeric row.</summary>
    /// <param name="x">Column label.</param>
    /// <param name="y">Row index.</param>
    /// <param name="z">Depth slice; defaults to <c>0</c>.</param>
    public GridIndex(string x, uint y, uint z = 0)
    {
        X = ConvertToZeroBasedIndex(x);
        Y = y;
        Z = z;
    }

    private static uint ConvertToZeroBasedIndex(string index)
    {
        var result = 0u;
        foreach (var c in index)
            if (char.IsLetter(c))
                result = result * 26u + (uint)(c - 'a');
            else
                throw new ArgumentException("Invalid character in index");
        return result;
    }
}
