namespace Novolis.Math.Arrays;

public partial class DenseGrid<T>
{
    /// <summary>
    /// Copies a rectangular sub-region into a new single-depth grid.
    /// </summary>
    /// <param name="x">Origin column.</param>
    /// <param name="y">Origin row.</param>
    /// <param name="width">Sub-region width in columns.</param>
    /// <param name="height">Sub-region height in rows.</param>
    /// <param name="z">Source depth slice; defaults to <c>0</c>.</param>
    /// <returns>A new grid containing the copied cells.</returns>
    public DenseGrid<T> Slice(uint x, uint y, uint width, uint height, uint z = 0)
    {
        var slice = new DenseGrid<T>(width, height, 1);
        for (var xi = x; xi < x + width; xi++)
        for (var yi = y; yi < y + height; yi++)
            slice[xi - x, yi - y, 0] = _array[yi, xi, z];
        return slice;
    }
}
