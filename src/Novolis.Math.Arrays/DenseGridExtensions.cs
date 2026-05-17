using System.Numerics;

namespace Novolis.Math.Arrays;

public static class DenseGridExtensions
{
    public static T? GetValue<T>(this T?[,,] array, GridIndex position) =>
        array[position.Y, position.X, position.Z];

    public static T? GetValue<T>(this T?[,,] array, uint x, uint y, uint z = 0) =>
        array[y, x, z];

    public static T? GetValue<T>(this T?[,,] array, string x, string y, uint z = 0) =>
        array.GetValue(new GridIndex(x, y, z));

    public static T? GetValue<T>(this T?[,,] array, uint x, string y, uint z = 0) =>
        array.GetValue(new GridIndex(x, y, z));

    public static T? GetValue<T>(this T?[,,] array, string x, uint y, uint z = 0) =>
        array.GetValue(new GridIndex(x, y, z));

    public static IEnumerable<T?> GetValues<T>(this T?[,,] array)
    {
        for (var y = 0u; y < array.GetLength(0); y++)
        for (var x = 0u; x < array.GetLength(1); x++)
        for (var z = 0u; z < array.GetLength(2); z++)
            yield return array[y, x, z];
    }

    public static bool TryGetValue<T>(this T?[,,] array, GridIndex position, out T? value)
    {
        value = array[position.Y, position.X, position.Z];
        return value != null;
    }

    public static bool TryGetValue<T>(this T?[,,] array, uint x, uint y, out T? value, uint z = 0)
    {
        value = array[y, x, z];
        return value != null;
    }

    public static bool TryGetValue<T>(this T?[,,] array, string x, string y, out T? value, uint z = 0) =>
        array.TryGetValue(new GridIndex(x, y, z), out value);

    public static IEnumerable<T?> GetRow<T>(this T?[,,] array, uint row, uint z = 0)
    {
        for (var x = 0u; x < array.GetLength(1); x++)
            yield return array[row, x, z];
    }

    public static IEnumerable<T?> GetColumn<T>(this T?[,,] array, uint column, uint z = 0)
    {
        for (var y = 0u; y < array.GetLength(0); y++)
            yield return array[y, column, z];
    }

    public static IEnumerable<T?> GetRow<T>(this T?[,,] array, string row, uint z = 0)
    {
        var position = new GridIndex(row, 0, z);
        for (var x = 0u; x < array.GetLength(1); x++)
            yield return array[position.Y, x, z];
    }

    public static IEnumerable<T?> GetColumn<T>(this T?[,,] array, string column, uint z = 0)
    {
        var position = new GridIndex(0, column, z);
        for (var y = 0u; y < array.GetLength(0); y++)
            yield return array[y, position.X, z];
    }

    public static IEnumerable<T?> Find<T>(this T?[,,] array, Func<T, bool> predicate)
    {
        for (var y = 0u; y < array.GetLength(0); y++)
        for (var x = 0u; x < array.GetLength(1); x++)
        for (var z = 0u; z < array.GetLength(2); z++)
            if (array[y, x, z] != null && predicate(array[y, x, z]!))
                yield return array[y, x, z]!;
    }

    public static IEnumerable<T?> FindInRow<T>(this T?[,,] array, uint row, Func<T?, bool> predicate, uint z = 0)
    {
        for (var x = 0u; x < array.GetLength(1); x++)
            if (array[row, x, z] != null && predicate(array[row, x, z]))
                yield return array[row, x, z];
    }

    public static IEnumerable<T?> FindInColumn<T>(this T?[,,] array, uint column, Func<T?, bool> predicate, uint z = 0)
    {
        for (var y = 0u; y < array.GetLength(0); y++)
            if (array[y, column, z] != null && predicate(array[y, column, z]))
                yield return array[y, column, z];
    }

    public static IEnumerable<T?> FindIn<T>(this T?[,,] array, Func<T, bool> predicate, params GridIndex[] positions)
    {
        foreach (var position in positions)
            if (array[position.Y, position.X, position.Z] != null &&
                predicate(array[position.Y, position.X, position.Z]!))
                yield return array[position.Y, position.X, position.Z]!;
    }

    public static IEnumerable<T?> FindIn<T>(this T?[,,] array, Func<T, bool> predicate, params Vector3[] positions)
    {
        foreach (var position in positions)
        {
            var index = new GridIndex((uint)position.X, (uint)position.Y, (uint)position.Z);
            if (array[index.Y, index.X, index.Z] != null && predicate(array[index.Y, index.X, index.Z]!))
                yield return array[index.Y, index.X, index.Z]!;
        }
    }
}
