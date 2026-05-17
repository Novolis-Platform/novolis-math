using System;
using System.Linq;
using Novolis.Math.Arrays;
using FluentAssertions;
using JetBrains.Annotations;
using TUnit.Core;

namespace Novolis.Math.Arrays.Tests;

[TestSubject(typeof(DenseGrid<>))]
public class DenseGridTests
{
    [Test]
    public void Constructor_WithWidthHeightDepth_ShouldSetPropertiesCorrectly()
    {
        var grid = new DenseGrid<int>(5, 5);
        grid.Width.Should().Be(5);
        grid.Height.Should().Be(5);
        grid.Depth.Should().Be(1);
    }

    [Test]
    public void GetRow_WithValidRow_ShouldReturnRow()
    {
        var grid = new DenseGrid<TestRecord?>(5, 5);
        grid.Set(new GridIndex(1, 1), new TestRecord("Frank", 10));
        grid.Set(new GridIndex(2, 2), new TestRecord("Bob", 20));
        grid.Set(new GridIndex(3, 3), new TestRecord("John", 30));
        grid.Set(new GridIndex(4, 4), new TestRecord("Jane", 40));
        grid.GetRow(2).Should().HaveCount(5);
    }

    private record TestRecord(string Name, int Age);

    [Test]
    public void GetRow_WithInvalidRow_ShouldThrowException()
    {
        var grid = new DenseGrid<int>(5, 5);
        var act = () => grid.GetRow(6);
        act.Should().Throw<IndexOutOfRangeException>();
    }

    [Test]
    public void GetColumn_WithValidColumn_ShouldReturnColumn()
    {
        var grid = new DenseGrid<int>(5, 5);
        grid.GetColumn(2).Should().HaveCount(5);
    }

    [Test]
    public void GetColumn_WithInvalidColumn_ShouldThrowException()
    {
        var grid = new DenseGrid<int>(5, 5);
        var act = () => grid.GetColumn(6);
        act.Should().Throw<IndexOutOfRangeException>();
    }

    [Test]
    public void Set_WithValidValues_ShouldSetValuesCorrectly()
    {
        var grid = new DenseGrid<int>(5, 5);
        grid.Set(new GridIndex(2, 2), 10);
        grid[new GridIndex(2, 2)].Should().Be(10);
    }

    [Test]
    public void FindInRow_WithValidPredicate_ShouldFindValuesInRow()
    {
        var grid = new DenseGrid<int>(5, 5);
        grid.Set(new GridIndex(1, 1), 10);
        grid.Set(new GridIndex(2, 2), 20);
        grid.FindInRow(1, x => x == 10).Should().HaveCount(1);
    }

    [Test]
    public void FindInColumn_WithValidPredicate_ShouldFindValuesInColumn()
    {
        var grid = new DenseGrid<int>(5, 5);
        grid.Set(new GridIndex(1, 1), 10);
        grid.Set(new GridIndex(2, 2), 20);
        grid.FindInColumn(2, x => x == 20).Should().HaveCount(1);
    }

    [Test]
    public void Slice_WithValidValues_ShouldReturnNewDenseGrid()
    {
        var grid = new DenseGrid<int>(5, 5);
        grid.Set(new GridIndex(1, 1), 10);
        var slice = grid.Slice(1, 1, 3, 3);
        slice.Should().NotBeNull();
        slice.Width.Should().Be(3);
    }
}
