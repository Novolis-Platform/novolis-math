using System.Numerics;
using Novolis.Math.Arrays;
using TUnit.Core;

namespace Novolis.Math.Arrays.Tests;

public class DenseGridMapAndIndexerTests
{
    [Test]
    public async Task GetMap_RendersOccupiedCells()
    {
        var grid = new DenseGrid<int>(3, 2);
        grid.Set(new GridIndex(1, 0), 1);
        var map = grid.GetMap();
        await Assert.That(map).Contains("X");
        await Assert.That(map.Split('\n').Length).IsGreaterThanOrEqualTo(2);
    }

    [Test]
    public async Task Indexer_GetSet_ByNumericAndLetter()
    {
        var grid = new DenseGrid<string>(4, 4);
        grid[1u, 0u, 0u] = "cell";
        grid["b", "c", 0] = "letter";
        await Assert.That(grid[1u, 0u, 0u]).IsEqualTo("cell");
        await Assert.That(grid["b", "c", 0]).IsEqualTo("letter");
    }

    [Test]
    public async Task ToString_IncludesDimensions()
    {
        var grid = new DenseGrid<int>(5, 7, 2);
        await Assert.That(grid.ToString()).Contains("z=0");
    }
}
