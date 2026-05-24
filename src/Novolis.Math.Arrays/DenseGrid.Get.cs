namespace Novolis.Math.Arrays;

public partial class DenseGrid<T>
{
    /// <summary>Gets the cell at <paramref name="position"/>.</summary>
    /// <param name="position">Cell coordinates.</param>
    /// <returns>The stored value, or <see langword="default"/> if unset.</returns>
    public T? Get(GridIndex position) => _array[position.Y, position.X, position.Z];

    /// <summary>Gets the cell at numeric coordinates.</summary>
    /// <param name="x">Column index.</param>
    /// <param name="y">Row index.</param>
    /// <param name="z">Depth slice; defaults to <c>0</c>.</param>
    /// <returns>The stored value, or <see langword="default"/> if unset.</returns>
    public T? Get(uint x, uint y, uint z = 0) => _array[y, x, z];

    /// <summary>Gets the cell at letter column and row labels.</summary>
    /// <param name="x">Column label.</param>
    /// <param name="y">Row label.</param>
    /// <param name="z">Depth slice; defaults to <c>0</c>.</param>
    /// <returns>The stored value, or <see langword="default"/> if unset.</returns>
    public T? Get(string x, string y, uint z = 0) => Get(new GridIndex(x, y, z));

    /// <summary>Gets the cell at a numeric column and letter row.</summary>
    /// <param name="x">Column index.</param>
    /// <param name="y">Row label.</param>
    /// <param name="z">Depth slice; defaults to <c>0</c>.</param>
    /// <returns>The stored value, or <see langword="default"/> if unset.</returns>
    public T? Get(uint x, string y, uint z = 0) => Get(new GridIndex(x, y, z));

    /// <summary>Gets the cell at a letter column and numeric row.</summary>
    /// <param name="x">Column label.</param>
    /// <param name="y">Row index.</param>
    /// <param name="z">Depth slice; defaults to <c>0</c>.</param>
    /// <returns>The stored value, or <see langword="default"/> if unset.</returns>
    public T? Get(string x, uint y, uint z = 0) => Get(new GridIndex(x, y, z));

    /// <summary>Copies one row into a new array.</summary>
    /// <param name="row">Row index.</param>
    /// <param name="z">Depth slice; defaults to <c>0</c>.</param>
    /// <returns>Row values in column order.</returns>
    public T?[] GetRow(uint row, uint z = 0)
    {
        var rowArray = new T?[Width];
        for (var i = 0u; i < Width; i++)
            rowArray[i] = _array[row, i, z];
        return rowArray;
    }

    /// <summary>Copies one column into a new array.</summary>
    /// <param name="column">Column index.</param>
    /// <param name="z">Depth slice; defaults to <c>0</c>.</param>
    /// <returns>Column values in row order.</returns>
    public T?[] GetColumn(uint column, uint z = 0)
    {
        var columnArray = new T?[Height];
        for (var i = 0u; i < Height; i++)
            columnArray[i] = _array[i, column, z];
        return columnArray;
    }

    /// <summary>Copies one row (letter label) into a new array.</summary>
    /// <param name="row">Row label.</param>
    /// <param name="z">Depth slice; defaults to <c>0</c>.</param>
    /// <returns>Row values in column order.</returns>
    public T?[] GetRow(string row, uint z = 0)
    {
        var rowArray = new T?[Width];
        var position = new GridIndex(row, 0, z);
        for (var i = 0u; i < Width; i++)
            rowArray[i] = _array[position.Y, i, z];
        return rowArray;
    }

    /// <summary>Copies one column (letter label) into a new array.</summary>
    /// <param name="column">Column label.</param>
    /// <param name="z">Depth slice; defaults to <c>0</c>.</param>
    /// <returns>Column values in row order.</returns>
    public T?[] GetColumn(string column, uint z = 0)
    {
        var columnArray = new T?[Height];
        var position = new GridIndex(0, column, z);
        for (var i = 0u; i < Height; i++)
            columnArray[i] = _array[i, position.X, z];
        return columnArray;
    }
}
