namespace Novolis.Math.Arrays;

public partial class DenseGrid<T>
{
    public void Set(GridIndex position, T value) => _array[position.Y, position.X, position.Z] = value;

    public void Set(uint x, uint y, T value, uint z = 0) => _array[y, x, z] = value;

    public void Set(string x, string y, T value, uint z = 0) => Set(new GridIndex(x, y, z), value);

    public void Set(uint x, string y, T value, uint z = 0) => Set(new GridIndex(x, y, z), value);

    public void Set(string x, uint y, T value, uint z = 0) => Set(new GridIndex(x, y, z), value);
}
