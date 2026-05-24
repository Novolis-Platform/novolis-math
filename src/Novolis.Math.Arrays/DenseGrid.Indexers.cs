namespace Novolis.Math.Arrays;

public partial class DenseGrid<T>
{
    /// <summary>Gets or sets the cell at <paramref name="position"/>.</summary>
    /// <param name="position">Cell coordinates.</param>
    public T? this[GridIndex position]
    {
        get => _array[position.Y, position.X, position.Z];
        set => _array[position.Y, position.X, position.Z] = value;
    }

    /// <summary>Gets or sets the cell at numeric coordinates.</summary>
    /// <param name="x">Column index.</param>
    /// <param name="y">Row index.</param>
    /// <param name="z">Depth slice.</param>
    public T? this[uint x, uint y, uint z]
    {
        get => _array[y, x, z];
        set => _array[y, x, z] = value;
    }

    /// <summary>Gets or sets the cell at letter column and row labels.</summary>
    /// <param name="x">Column label.</param>
    /// <param name="y">Row label.</param>
    /// <param name="z">Depth slice.</param>
    public T? this[string x, string y, uint z]
    {
        get => this[new GridIndex(x, y, z)];
        set => this[new GridIndex(x, y, z)] = value;
    }

    /// <summary>Gets or sets the cell at a numeric column and letter row.</summary>
    /// <param name="x">Column index.</param>
    /// <param name="y">Row label.</param>
    /// <param name="z">Depth slice.</param>
    public T? this[uint x, string y, uint z]
    {
        get => this[new GridIndex(x, y, z)];
        set => this[new GridIndex(x, y, z)] = value;
    }

    /// <summary>Gets or sets the cell at a letter column and numeric row.</summary>
    /// <param name="x">Column label.</param>
    /// <param name="y">Row index.</param>
    /// <param name="z">Depth slice.</param>
    public T? this[string x, uint y, uint z]
    {
        get => this[new GridIndex(x, y, z)];
        set => this[new GridIndex(x, y, z)] = value;
    }
}
