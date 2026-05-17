using System.Numerics;
using FluentAssertions;
using Novolis.Math.Arrays;
using Novolis.Math.Geometry;

namespace Novolis.Math.Geometry.Tests;

public class GridCollision2DTests
{
    [Test]
    public void TryMove_OpenFloor_AppliesFullDelta()
    {
        var map = new DenseGrid<byte>(4, 4);
        var start = new Vector2(1.5f, 1.5f);
        var end = GridCollision2D.TryMove(map, start, new Vector2(0.3f, 0.2f), 0.2f);
        end.X.Should().BeApproximately(1.8f, 1e-4f);
        end.Y.Should().BeApproximately(1.7f, 1e-4f);
    }

    [Test]
    public void TryMove_IntoWall_BlocksAxis()
    {
        var map = new DenseGrid<byte>(4, 4);
        map.Set(new GridIndex(2, 1), 1);

        var start = new Vector2(1.5f, 1.5f);
        var end = GridCollision2D.TryMove(map, start, new Vector2(1f, 0f), 0.2f);
        end.X.Should().BeLessThan(2f);
        end.Y.Should().BeApproximately(1.5f, 1e-4f);
    }

    [Test]
    public void OverlapsWall_OutsideMap_ReturnsTrue()
    {
        var map = new DenseGrid<byte>(3, 3);
        GridCollision2D.OverlapsWall(map, new Vector2(-1f, 1f), 0.2f).Should().BeTrue();
    }

    [Test]
    public void OverlapsWall_InsideOpenCell_ReturnsFalse()
    {
        var map = new DenseGrid<byte>(5, 5);
        map.Set(new GridIndex(2, 2), 1);
        GridCollision2D.OverlapsWall(map, new Vector2(1.5f, 1.5f), 0.25f).Should().BeFalse();
    }
}
