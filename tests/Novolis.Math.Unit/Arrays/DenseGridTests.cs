using System;
using System.Linq;
using Novolis.Math.Arrays;
using JetBrains.Annotations;
using TUnit.Core;

namespace Novolis.Math.Arrays.Tests;

[TestSubject(typeof(DenseGrid<>))]
public class DenseGridTests
{
    [Test]
    public async Task Constructor_WithWidthHeightDepth_ShouldSetPropertiesCorrectly()
    {
        var grid = new DenseGrid<int>(5, 5);
        await Assert.That(grid.Width).IsEqualTo(5u);
        await Assert.That(grid.Height).IsEqualTo(5u);
        await Assert.That(grid.Depth).IsEqualTo(1u);
    }

    [Test]
    public async Task GetRow_WithValidRow_ShouldReturnRow()
    {
        var grid = new DenseGrid<TestRecord?>(5, 5);
        grid.Set(new GridIndex(1, 1), new TestRecord("Frank", 10));
        grid.Set(new GridIndex(2, 2), new TestRecord("Bob", 20));
        grid.Set(new GridIndex(3, 3), new TestRecord("John", 30));
        grid.Set(new GridIndex(4, 4), new TestRecord("Jane", 40));
        await Assert.That(grid.GetRow(2).Count()).IsEqualTo(5);
    }

    private record TestRecord(string Name, int Age);

    [Test]
    public async Task GetRow_WithInvalidRow_ShouldThrowException()
    {
        var grid = new DenseGrid<int>(5, 5);
        var act = () => grid.GetRow(6);
        await Assert.That(act).Throws<IndexOutOfRangeException>();
    }

    [Test]
    public async Task GetColumn_WithValidColumn_ShouldReturnColumn()
    {
        var grid = new DenseGrid<int>(5, 5);
        await Assert.That(grid.GetColumn(2).Count()).IsEqualTo(5);
    }

    [Test]
    public async Task GetColumn_WithInvalidColumn_ShouldThrowException()
    {
        var grid = new DenseGrid<int>(5, 5);
        var act = () => grid.GetColumn(6);
        await Assert.That(act).Throws<IndexOutOfRangeException>();
    }

    [Test]
    public async Task Set_WithValidValues_ShouldSetValuesCorrectly()
    {
        var grid = new DenseGrid<int>(5, 5);
        grid.Set(new GridIndex(2, 2), 10);
        await Assert.That(grid[new GridIndex(2, 2)]).IsEqualTo(10);
    }

    [Test]
    public async Task FindInRow_WithValidPredicate_ShouldFindValuesInRow()
    {
        var grid = new DenseGrid<int>(5, 5);
        grid.Set(new GridIndex(1, 1), 10);
        grid.Set(new GridIndex(2, 2), 20);
        await Assert.That(grid.FindInRow(1, x => x == 10).Count()).IsEqualTo(1);
    }

    [Test]
    public async Task FindInColumn_WithValidPredicate_ShouldFindValuesInColumn()
    {
        var grid = new DenseGrid<int>(5, 5);
        grid.Set(new GridIndex(1, 1), 10);
        grid.Set(new GridIndex(2, 2), 20);
        await Assert.That(grid.FindInColumn(2, x => x == 20).Count()).IsEqualTo(1);
    }

    [Test]
    public async Task Slice_WithValidValues_ShouldReturnNewDenseGrid()
    {
        var grid = new DenseGrid<int>(5, 5);
        grid.Set(new GridIndex(1, 1), 10);
        var slice = grid.Slice(1, 1, 3, 3);
        await Assert.That(slice).IsNotNull();
        await Assert.That(slice.Width).IsEqualTo(3u);
    }
}
