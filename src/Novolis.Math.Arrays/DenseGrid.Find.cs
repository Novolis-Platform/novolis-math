namespace Novolis.Math.Arrays;

public partial class DenseGrid<T>
{
    /// <summary>Yields non-null cells that satisfy <paramref name="predicate"/>.</summary>
    /// <param name="predicate">Filter applied to non-null values.</param>
    /// <returns>Matching cell values.</returns>
    public IEnumerable<T?> Find(Func<T, bool> predicate) => _array.Find(predicate);

    /// <summary>Yields non-null cells in a row that satisfy <paramref name="predicate"/>.</summary>
    /// <param name="row">Row index.</param>
    /// <param name="predicate">Filter applied to each cell in the row.</param>
    /// <param name="z">Depth slice; defaults to <c>0</c>.</param>
    /// <returns>Matching cell values.</returns>
    public IEnumerable<T?> FindInRow(uint row, Func<T?, bool> predicate, uint z = 0) =>
        GetRow(row, z).Where(predicate);

    /// <summary>Yields non-null cells in a column that satisfy <paramref name="predicate"/>.</summary>
    /// <param name="column">Column index.</param>
    /// <param name="predicate">Filter applied to each cell in the column.</param>
    /// <param name="z">Depth slice; defaults to <c>0</c>.</param>
    /// <returns>Matching cell values.</returns>
    public IEnumerable<T?> FindInColumn(uint column, Func<T?, bool> predicate, uint z = 0) =>
        GetColumn(column, z).Where(predicate);

    /// <summary>Yields non-null cells at the given positions that satisfy <paramref name="predicate"/>.</summary>
    /// <param name="predicate">Filter applied to non-null values.</param>
    /// <param name="positions">Cells to test.</param>
    /// <returns>Matching cell values.</returns>
    public IEnumerable<T?> FindIn(Func<T, bool> predicate, params GridIndex[] positions) =>
        _array.FindIn(predicate, positions);
}
