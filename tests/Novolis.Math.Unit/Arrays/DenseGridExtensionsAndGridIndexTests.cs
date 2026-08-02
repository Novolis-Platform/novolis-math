using System.Numerics;
using Novolis.Math.Arrays;
using Novolis.Math.Geometry;
using Novolis.Math.Topology;
using TUnit.Core;

namespace Novolis.Math.Arrays.Tests;

public class DenseGridExtensionsAndGridIndexTests
{
    [Test]
    public async Task GridIndex_LetterLabels_MapToZeroBased()
    {
        var idx = new GridIndex("a", "b", z: 1);
        await Assert.That(idx.X).IsEqualTo(0u);
        await Assert.That(idx.Y).IsEqualTo(1u);
        await Assert.That(idx.Z).IsEqualTo(1u);

        var mixed = new GridIndex("ab", 2u);
        await Assert.That(mixed.X).IsEqualTo(1u);
        await Assert.That(mixed.Y).IsEqualTo(2u);
    }

    [Test]
    public async Task GridIndex_InvalidCharacter_Throws()
    {
        var act = () => _ = new GridIndex("a1", "b");
        await Assert.That(act).Throws<ArgumentException>();
    }

    [Test]
    public async Task DenseGridExtensions_GetSetTryGetValues()
    {
        int?[, ,] array = new int?[2, 3, 2];
        array[1, 2, 1] = 42;
        array.SetValue(7, 0, 0, 0);

        await Assert.That(array.GetValue(new GridIndex(2, 1, 1))).IsEqualTo(42);
        await Assert.That(array.GetValue(0u, 0u, 0u)).IsEqualTo(7);
        await Assert.That(array.GetValue("a", "a", 0)).IsEqualTo(7);
        await Assert.That(array.GetValue(0u, "a", 0)).IsEqualTo(7);
        await Assert.That(array.GetValue("a", 0u, 0)).IsEqualTo(7);
        await Assert.That(array.GetValues().Count()).IsEqualTo(12);
        await Assert.That(array.TryGetValue(2, 1, out var v, 1)).IsTrue();
        await Assert.That(v).IsEqualTo(42);
        await Assert.That(array.TryGetValue("c", "b", out _, 1)).IsTrue();
    }

    [Test]
    public async Task DenseGridExtensions_RowColumnFind()
    {
        int?[, ,] array = new int?[3, 3, 1];
        array[0, 1, 0] = 5;
        array[1, 1, 0] = 9;
        array[2, 2, 0] = 11;

        await Assert.That(array.GetRow(0).Count()).IsEqualTo(3);
        await Assert.That(array.GetColumn(1).Count()).IsEqualTo(3);
        await Assert.That(array.GetRow("a").Count()).IsEqualTo(3);
        await Assert.That(array.GetColumn("b").Count()).IsEqualTo(3);

        var found = array.Find(x => x > 8).ToList();
        await Assert.That(found).Contains(9);
        await Assert.That(found).Contains(11);

        var rowHits = array.FindInRow(0, x => x == 5).ToList();
        await Assert.That(rowHits.Count).IsEqualTo(1);

        var colHits = array.FindInColumn(1, x => x is 5 or 9).ToList();
        await Assert.That(colHits.Count).IsEqualTo(2);

        var inCells = array.FindIn(x => x == 11, new GridIndex(2, 2, 0)).ToList();
        await Assert.That(inCells).Contains(11);

        var inVec = array.FindIn(x => x == 5, new Vector3(1f, 0f, 0f)).ToList();
        await Assert.That(inVec).Contains(5);
    }

    [Test]
    public async Task DenseGrid_FindMethods_DelegateToExtensions()
    {
        var grid = new DenseGrid<int>(4, 4);
        grid.Set(new GridIndex(1, 2), 10);
        grid.Set(new GridIndex(2, 2), 20);
        await Assert.That(grid.Find(x => x >= 10).Count()).IsEqualTo(2);
        await Assert.That(grid.FindInRow(2, x => x == 20).Count()).IsEqualTo(1);
        await Assert.That(grid.FindInColumn(1, x => x == 10).Count()).IsEqualTo(1);
        await Assert.That(grid.FindIn(x => x == 20, new GridIndex(2, 2)).Count()).IsEqualTo(1);
    }
}
