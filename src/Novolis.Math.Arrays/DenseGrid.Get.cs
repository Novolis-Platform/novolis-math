namespace Novolis.Math.Arrays;

public partial class DenseGrid<T>
{
    public T? Get(GridIndex position) => _array[position.Y, position.X, position.Z];

    public T? Get(uint x, uint y, uint z = 0) => _array[y, x, z];

    public T? Get(string x, string y, uint z = 0) => Get(new GridIndex(x, y, z));

    public T? Get(uint x, string y, uint z = 0) => Get(new GridIndex(x, y, z));

    public T? Get(string x, uint y, uint z = 0) => Get(new GridIndex(x, y, z));

    public T?[] GetRow(uint row, uint z = 0)
    {
        var rowArray = new T?[Width];
        for (var i = 0u; i < Width; i++)
            rowArray[i] = _array[row, i, z];
        return rowArray;
    }

    public T?[] GetColumn(uint column, uint z = 0)
    {
        var columnArray = new T?[Height];
        for (var i = 0u; i < Height; i++)
            columnArray[i] = _array[i, column, z];
        return columnArray;
    }

    public T?[] GetRow(string row, uint z = 0)
    {
        var rowArray = new T?[Width];
        var position = new GridIndex(row, 0, z);
        for (var i = 0u; i < Width; i++)
            rowArray[i] = _array[position.Y, i, z];
        return rowArray;
    }

    public T?[] GetColumn(string column, uint z = 0)
    {
        var columnArray = new T?[Height];
        var position = new GridIndex(0, column, z);
        for (var i = 0u; i < Height; i++)
            columnArray[i] = _array[i, position.X, z];
        return columnArray;
    }
}
