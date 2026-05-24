namespace Novolis.Math.Arrays;

public partial class DenseGrid<T>
{
    /// <summary>Stores <paramref name="value"/> at <paramref name="position"/>.</summary>
    /// <param name="position">Cell coordinates.</param>
    /// <param name="value">Value to store.</param>
    public void Set(GridIndex position, T value) => _array[position.Y, position.X, position.Z] = value;

    /// <summary>Stores <paramref name="value"/> at numeric coordinates.</summary>
    /// <param name="x">Column index.</param>
    /// <param name="y">Row index.</param>
    /// <param name="value">Value to store.</param>
    /// <param name="z">Depth slice; defaults to <c>0</c>.</param>
    public void Set(uint x, uint y, T value, uint z = 0) => _array[y, x, z] = value;

    /// <summary>Stores <paramref name="value"/> at letter column and row labels.</summary>
    /// <param name="x">Column label.</param>
    /// <param name="y">Row label.</param>
    /// <param name="value">Value to store.</param>
    /// <param name="z">Depth slice; defaults to <c>0</c>.</param>
    public void Set(string x, string y, T value, uint z = 0) => Set(new GridIndex(x, y, z), value);

    /// <summary>Stores <paramref name="value"/> at a numeric column and letter row.</summary>
    /// <param name="x">Column index.</param>
    /// <param name="y">Row label.</param>
    /// <param name="value">Value to store.</param>
    /// <param name="z">Depth slice; defaults to <c>0</c>.</param>
    public void Set(uint x, string y, T value, uint z = 0) => Set(new GridIndex(x, y, z), value);

    /// <summary>Stores <paramref name="value"/> at a letter column and numeric row.</summary>
    /// <param name="x">Column label.</param>
    /// <param name="y">Row index.</param>
    /// <param name="value">Value to store.</param>
    /// <param name="z">Depth slice; defaults to <c>0</c>.</param>
    public void Set(string x, uint y, T value, uint z = 0) => Set(new GridIndex(x, y, z), value);
}
