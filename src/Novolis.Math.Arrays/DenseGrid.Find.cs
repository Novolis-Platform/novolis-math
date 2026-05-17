namespace Novolis.Math.Arrays;

public partial class DenseGrid<T>
{
    public IEnumerable<T?> Find(Func<T, bool> predicate) => _array.Find(predicate);

    public IEnumerable<T?> FindInRow(uint row, Func<T?, bool> predicate, uint z = 0) =>
        GetRow(row, z).Where(predicate);

    public IEnumerable<T?> FindInColumn(uint column, Func<T?, bool> predicate, uint z = 0) =>
        GetColumn(column, z).Where(predicate);

    public IEnumerable<T?> FindIn(Func<T, bool> predicate, params GridIndex[] positions) =>
        _array.FindIn(predicate, positions);
}
