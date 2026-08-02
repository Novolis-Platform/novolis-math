using Novolis.Math.Arrays;
using TUnit.Core;

namespace Novolis.Math.Arrays.Tests;

public class DenseGridGetSetTests
{
    [Test]
    public async Task GetSet_AllIndexerOverloads()
    {
        var grid = new DenseGrid<int>(4, 3, depth: 2);
        grid.Set(new GridIndex(1, 2, 1), 42);
        grid.Set(2u, 1u, 99, z: 0);
        grid.Set("a", "b", 7, z: 0);
        grid.Set(1u, "a", 8, z: 0);
        grid.Set("b", 2u, 9, z: 0);

        await Assert.That(grid.Get(new GridIndex(1, 2, 1))).IsEqualTo(42);
        await Assert.That(grid.Get(2u, 1u)).IsEqualTo(99);
        await Assert.That(grid.Get("a", "b")).IsEqualTo(7);
        await Assert.That(grid.Get(1u, "a")).IsEqualTo(8);
        await Assert.That(grid.Get("b", 2u)).IsEqualTo(9);
        await Assert.That(grid[1u, 2u, 1]).IsEqualTo(42);
        grid["a", "b", 0] = 11;
        grid["b", 1u, 0] = 12;
        await Assert.That(grid["a", "b", 0]).IsEqualTo(11);
        await Assert.That(grid["b", 1u, 0]).IsEqualTo(12);
    }

    [Test]
    public async Task GetRowColumn_ArrayCopies()
    {
        var grid = new DenseGrid<string>(3, 2);
        grid.Set(1, 0, "mid");
        var row = grid.GetRow(0);
        var col = grid.GetColumn(1);
        var rowLetter = grid.GetRow("a");
        var colLetter = grid.GetColumn("b");
        await Assert.That(row.Length).IsEqualTo(3);
        await Assert.That(col.Length).IsEqualTo(2);
        await Assert.That(row[1]).IsEqualTo("mid");
        await Assert.That(col[0]).IsEqualTo("mid");
        await Assert.That(rowLetter[1]).IsEqualTo("mid");
        await Assert.That(colLetter.Length).IsEqualTo(2);
    }

    [Test]
    public async Task Slice_CopiesSubRegionAtDepth()
    {
        var grid = new DenseGrid<int>(4, 4, 2);
        grid.Set(2, 2, 5, z: 1);
        var slice = grid.Slice(1, 1, 2, 2, z: 1);
        await Assert.That(slice[1, 1, 0]).IsEqualTo(5);
        await Assert.That(slice.Depth).IsEqualTo(1u);
    }
}
