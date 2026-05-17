namespace Novolis.Math.Arrays;

public partial class DenseGrid<T>
{
    public DenseGrid<T> Slice(uint x, uint y, uint width, uint height, uint z = 0)
    {
        var slice = new DenseGrid<T>(width, height, 1);
        for (var xi = x; xi < x + width; xi++)
        for (var yi = y; yi < y + height; yi++)
            slice[xi - x, yi - y, 0] = _array[yi, xi, z];
        return slice;
    }
}
