using System.Numerics;
using Novolis.Math.Arrays;
using Novolis.Math.Geometry;
using TUnit.Core;

namespace Novolis.Math.Geometry.Tests;

public class GridCollision2DTests
{
    [Test]
    public async Task TryMove_OpenFloor_AppliesFullDelta()
    {
        var map = new DenseGrid<byte>(4, 4);
        var start = new Vector2(1.5f, 1.5f);
        var end = GridCollision2D.TryMove(map, start, new Vector2(0.3f, 0.2f), 0.2f);
        await Assert.That(end.X).IsEqualTo(1.8f).Within(1e-4f);
        await Assert.That(end.Y).IsEqualTo(1.7f).Within(1e-4f);
    }

    [Test]
    public async Task TryMove_IntoWall_BlocksAxis()
    {
        var map = new DenseGrid<byte>(4, 4);
        map.Set(new GridIndex(2, 1), 1);

        var start = new Vector2(1.5f, 1.5f);
        var end = GridCollision2D.TryMove(map, start, new Vector2(1f, 0f), 0.2f);
        await Assert.That(end.X).IsLessThan(2f);
        await Assert.That(end.Y).IsEqualTo(1.5f).Within(1e-4f);
    }

    [Test]
    public async Task OverlapsWall_OutsideMap_ReturnsTrue()
    {
        var map = new DenseGrid<byte>(3, 3);
        await Assert.That(GridCollision2D.OverlapsWall(map, new Vector2(-1f, 1f), 0.2f)).IsTrue();
    }

    [Test]
    public async Task OverlapsWall_InsideOpenCell_ReturnsFalse()
    {
        var map = new DenseGrid<byte>(5, 5);
        map.Set(new GridIndex(2, 2), 1);
        await Assert.That(GridCollision2D.OverlapsWall(map, new Vector2(1.5f, 1.5f), 0.25f)).IsFalse();
    }

    [Test]
    public async Task HasLineOfSight_OpenLine_ReturnsTrue()
    {
        var map = new DenseGrid<byte>(5, 5);
        await Assert.That(GridCollision2D.HasLineOfSight(map, new Vector2(0.5f, 0.5f), new Vector2(3.5f, 0.5f))).IsTrue();
    }

    [Test]
    public async Task HasLineOfSight_WallBetween_ReturnsFalse()
    {
        var map = new DenseGrid<byte>(5, 5);
        map.Set(new GridIndex(2, 0), 1);
        map.Set(new GridIndex(2, 1), 1);
        map.Set(new GridIndex(2, 2), 1);
        map.Set(new GridIndex(2, 3), 1);
        map.Set(new GridIndex(2, 4), 1);
        await Assert.That(GridCollision2D.HasLineOfSight(map, new Vector2(0.5f, 2.5f), new Vector2(4.5f, 2.5f))).IsFalse();
    }

    [Test]
    public async Task HasLineOfSight_AroundCorner_ReturnsFalse()
    {
        var map = new DenseGrid<byte>(5, 5);
        map.Set(new GridIndex(2, 1), 1);
        map.Set(new GridIndex(2, 2), 1);
        map.Set(new GridIndex(1, 2), 1);
        await Assert.That(GridCollision2D.HasLineOfSight(map, new Vector2(0.5f, 0.5f), new Vector2(3.5f, 3.5f))).IsFalse();
    }
}
