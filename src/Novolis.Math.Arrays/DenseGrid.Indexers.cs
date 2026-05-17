namespace Novolis.Math.Arrays;

public partial class DenseGrid<T>
{
    public T? this[GridIndex position]
    {
        get => _array[position.Y, position.X, position.Z];
        set => _array[position.Y, position.X, position.Z] = value;
    }

    public T? this[uint x, uint y, uint z]
    {
        get => _array[y, x, z];
        set => _array[y, x, z] = value;
    }

    public T? this[string x, string y, uint z]
    {
        get => this[new GridIndex(x, y, z)];
        set => this[new GridIndex(x, y, z)] = value;
    }

    public T? this[uint x, string y, uint z]
    {
        get => this[new GridIndex(x, y, z)];
        set => this[new GridIndex(x, y, z)] = value;
    }

    public T? this[string x, uint y, uint z]
    {
        get => this[new GridIndex(x, y, z)];
        set => this[new GridIndex(x, y, z)] = value;
    }
}
