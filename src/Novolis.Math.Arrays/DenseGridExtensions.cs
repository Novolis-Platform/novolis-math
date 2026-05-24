using System.Numerics;

namespace Novolis.Math.Arrays;

/// <summary>Extension methods for rank-3 nullable arrays used by <see cref="DenseGrid{T}"/>.</summary>
public static class DenseGridExtensions
{
    /// <summary>Gets the cell at <paramref name="position"/>.</summary>
    /// <typeparam name="T">Cell value type.</typeparam>
    /// <param name="array">Source array (row, column, depth layout).</param>
    /// <param name="position">Cell coordinates.</param>
    /// <returns>The stored value, or <see langword="default"/> if unset.</returns>
    public static T? GetValue<T>(this T?[,,] array, GridIndex position) =>
        array[position.Y, position.X, position.Z];

    /// <summary>Gets the cell at numeric coordinates.</summary>
    /// <typeparam name="T">Cell value type.</typeparam>
    /// <param name="array">Source array.</param>
    /// <param name="x">Column index.</param>
    /// <param name="y">Row index.</param>
    /// <param name="z">Depth slice; defaults to <c>0</c>.</param>
    /// <returns>The stored value, or <see langword="default"/> if unset.</returns>
    public static T? GetValue<T>(this T?[,,] array, uint x, uint y, uint z = 0) =>
        array[y, x, z];

    /// <summary>Gets the cell at letter column and row labels.</summary>
    /// <typeparam name="T">Cell value type.</typeparam>
    /// <param name="array">Source array.</param>
    /// <param name="x">Column label.</param>
    /// <param name="y">Row label.</param>
    /// <param name="z">Depth slice; defaults to <c>0</c>.</param>
    /// <returns>The stored value, or <see langword="default"/> if unset.</returns>
    public static T? GetValue<T>(this T?[,,] array, string x, string y, uint z = 0) =>
        array.GetValue(new GridIndex(x, y, z));

    /// <summary>Gets the cell at a numeric column and letter row.</summary>
    /// <typeparam name="T">Cell value type.</typeparam>
    /// <param name="array">Source array.</param>
    /// <param name="x">Column index.</param>
    /// <param name="y">Row label.</param>
    /// <param name="z">Depth slice; defaults to <c>0</c>.</param>
    /// <returns>The stored value, or <see langword="default"/> if unset.</returns>
    public static T? GetValue<T>(this T?[,,] array, uint x, string y, uint z = 0) =>
        array.GetValue(new GridIndex(x, y, z));

    /// <summary>Gets the cell at a letter column and numeric row.</summary>
    /// <typeparam name="T">Cell value type.</typeparam>
    /// <param name="array">Source array.</param>
    /// <param name="x">Column label.</param>
    /// <param name="y">Row index.</param>
    /// <param name="z">Depth slice; defaults to <c>0</c>.</param>
    /// <returns>The stored value, or <see langword="default"/> if unset.</returns>
    public static T? GetValue<T>(this T?[,,] array, string x, uint y, uint z = 0) =>
        array.GetValue(new GridIndex(x, y, z));

    /// <summary>Enumerates every cell in row-major order.</summary>
    /// <typeparam name="T">Cell value type.</typeparam>
    /// <param name="array">Source array.</param>
    /// <returns>All cell values, including nulls.</returns>
    public static IEnumerable<T?> GetValues<T>(this T?[,,] array)
    {
        for (var y = 0u; y < array.GetLength(0); y++)
        for (var x = 0u; x < array.GetLength(1); x++)
        for (var z = 0u; z < array.GetLength(2); z++)
            yield return array[y, x, z];
    }

    /// <summary>Gets a cell and reports whether it is non-null.</summary>
    /// <typeparam name="T">Cell value type.</typeparam>
    /// <param name="array">Source array.</param>
    /// <param name="position">Cell coordinates.</param>
    /// <param name="value">When this method returns, the cell value if non-null.</param>
    /// <returns><see langword="true"/> if the cell contains a non-null value.</returns>
    public static bool TryGetValue<T>(this T?[,,] array, GridIndex position, out T? value)
    {
        value = array[position.Y, position.X, position.Z];
        return value != null;
    }

    /// <summary>Gets a cell at numeric coordinates and reports whether it is non-null.</summary>
    /// <typeparam name="T">Cell value type.</typeparam>
    /// <param name="array">Source array.</param>
    /// <param name="x">Column index.</param>
    /// <param name="y">Row index.</param>
    /// <param name="value">When this method returns, the cell value if non-null.</param>
    /// <param name="z">Depth slice; defaults to <c>0</c>.</param>
    /// <returns><see langword="true"/> if the cell contains a non-null value.</returns>
    public static bool TryGetValue<T>(this T?[,,] array, uint x, uint y, out T? value, uint z = 0)
    {
        value = array[y, x, z];
        return value != null;
    }

    /// <summary>Gets a cell at letter labels and reports whether it is non-null.</summary>
    /// <typeparam name="T">Cell value type.</typeparam>
    /// <param name="array">Source array.</param>
    /// <param name="x">Column label.</param>
    /// <param name="y">Row label.</param>
    /// <param name="value">When this method returns, the cell value if non-null.</param>
    /// <param name="z">Depth slice; defaults to <c>0</c>.</param>
    /// <returns><see langword="true"/> if the cell contains a non-null value.</returns>
    public static bool TryGetValue<T>(this T?[,,] array, string x, string y, out T? value, uint z = 0) =>
        array.TryGetValue(new GridIndex(x, y, z), out value);

    /// <summary>Enumerates one row.</summary>
    /// <typeparam name="T">Cell value type.</typeparam>
    /// <param name="array">Source array.</param>
    /// <param name="row">Row index.</param>
    /// <param name="z">Depth slice; defaults to <c>0</c>.</param>
    /// <returns>Row values in column order.</returns>
    public static IEnumerable<T?> GetRow<T>(this T?[,,] array, uint row, uint z = 0)
    {
        for (var x = 0u; x < array.GetLength(1); x++)
            yield return array[row, x, z];
    }

    /// <summary>Enumerates one column.</summary>
    /// <typeparam name="T">Cell value type.</typeparam>
    /// <param name="array">Source array.</param>
    /// <param name="column">Column index.</param>
    /// <param name="z">Depth slice; defaults to <c>0</c>.</param>
    /// <returns>Column values in row order.</returns>
    public static IEnumerable<T?> GetColumn<T>(this T?[,,] array, uint column, uint z = 0)
    {
        for (var y = 0u; y < array.GetLength(0); y++)
            yield return array[y, column, z];
    }

    /// <summary>Enumerates one row identified by a letter label.</summary>
    /// <typeparam name="T">Cell value type.</typeparam>
    /// <param name="array">Source array.</param>
    /// <param name="row">Row label.</param>
    /// <param name="z">Depth slice; defaults to <c>0</c>.</param>
    /// <returns>Row values in column order.</returns>
    public static IEnumerable<T?> GetRow<T>(this T?[,,] array, string row, uint z = 0)
    {
        var position = new GridIndex(row, 0, z);
        for (var x = 0u; x < array.GetLength(1); x++)
            yield return array[position.Y, x, z];
    }

    /// <summary>Enumerates one column identified by a letter label.</summary>
    /// <typeparam name="T">Cell value type.</typeparam>
    /// <param name="array">Source array.</param>
    /// <param name="column">Column label.</param>
    /// <param name="z">Depth slice; defaults to <c>0</c>.</param>
    /// <returns>Column values in row order.</returns>
    public static IEnumerable<T?> GetColumn<T>(this T?[,,] array, string column, uint z = 0)
    {
        var position = new GridIndex(0, column, z);
        for (var y = 0u; y < array.GetLength(0); y++)
            yield return array[y, position.X, z];
    }

    /// <summary>Yields non-null cells that satisfy <paramref name="predicate"/>.</summary>
    /// <typeparam name="T">Cell value type.</typeparam>
    /// <param name="array">Source array.</param>
    /// <param name="predicate">Filter applied to non-null values.</param>
    /// <returns>Matching cell values.</returns>
    public static IEnumerable<T?> Find<T>(this T?[,,] array, Func<T, bool> predicate)
    {
        for (var y = 0u; y < array.GetLength(0); y++)
        for (var x = 0u; x < array.GetLength(1); x++)
        for (var z = 0u; z < array.GetLength(2); z++)
            if (array[y, x, z] != null && predicate(array[y, x, z]!))
                yield return array[y, x, z]!;
    }

    /// <summary>Yields non-null cells in a row that satisfy <paramref name="predicate"/>.</summary>
    /// <typeparam name="T">Cell value type.</typeparam>
    /// <param name="array">Source array.</param>
    /// <param name="row">Row index.</param>
    /// <param name="predicate">Filter applied to each cell in the row.</param>
    /// <param name="z">Depth slice; defaults to <c>0</c>.</param>
    /// <returns>Matching cell values.</returns>
    public static IEnumerable<T?> FindInRow<T>(this T?[,,] array, uint row, Func<T?, bool> predicate, uint z = 0)
    {
        for (var x = 0u; x < array.GetLength(1); x++)
            if (array[row, x, z] != null && predicate(array[row, x, z]))
                yield return array[row, x, z];
    }

    /// <summary>Yields non-null cells in a column that satisfy <paramref name="predicate"/>.</summary>
    /// <typeparam name="T">Cell value type.</typeparam>
    /// <param name="array">Source array.</param>
    /// <param name="column">Column index.</param>
    /// <param name="predicate">Filter applied to each cell in the column.</param>
    /// <param name="z">Depth slice; defaults to <c>0</c>.</param>
    /// <returns>Matching cell values.</returns>
    public static IEnumerable<T?> FindInColumn<T>(this T?[,,] array, uint column, Func<T?, bool> predicate, uint z = 0)
    {
        for (var y = 0u; y < array.GetLength(0); y++)
            if (array[y, column, z] != null && predicate(array[y, column, z]))
                yield return array[y, column, z];
    }

    /// <summary>Yields non-null cells at the given positions that satisfy <paramref name="predicate"/>.</summary>
    /// <typeparam name="T">Cell value type.</typeparam>
    /// <param name="array">Source array.</param>
    /// <param name="predicate">Filter applied to non-null values.</param>
    /// <param name="positions">Cells to test.</param>
    /// <returns>Matching cell values.</returns>
    public static IEnumerable<T?> FindIn<T>(this T?[,,] array, Func<T, bool> predicate, params GridIndex[] positions)
    {
        foreach (var position in positions)
            if (array[position.Y, position.X, position.Z] != null &&
                predicate(array[position.Y, position.X, position.Z]!))
                yield return array[position.Y, position.X, position.Z]!;
    }

    /// <summary>
    /// Yields non-null cells at positions derived from <see cref="Vector3"/> coordinates (truncated to <c>uint</c>).
    /// </summary>
    /// <typeparam name="T">Cell value type.</typeparam>
    /// <param name="array">Source array.</param>
    /// <param name="predicate">Filter applied to non-null values.</param>
    /// <param name="positions">World or lattice positions mapped to grid indices.</param>
    /// <returns>Matching cell values.</returns>
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
