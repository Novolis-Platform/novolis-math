using System.Linq;
using System.Numerics;
using FluentAssertions;
using Novolis.Math.Geometry;
using TUnit.Core;

namespace Novolis.Math.Geometry.Tests;

public class PolygonTest
{
    [Test]
    public void TestPolygonIntersection2D()
    {
        var polygon1 = PolygonFactory.CreateRectangle(4, 4, new Vector3(-1, 0, 0));
        var polygon2 = PolygonFactory.CreateRectangle(4, 4, new Vector3(1, 1, 0));

        var intersections = polygon1.GetIntersectionPoints(polygon2).ToList();

        intersections.Should().NotBeEmpty();
    }

    [Test]
    public void TestPolygonIntersection3D()
    {
        var polygon1 = PolygonFactory.CreateCube(10);
        polygon1.Translate(new Vector3(5, 0, 0));
        var polygon2 = PolygonFactory.CreateCube(10);
        polygon2.Translate(new Vector3(0, 0, 0));

        var intersections = polygon1.GetIntersectionPoints(polygon2).ToList();

        intersections.Should().NotBeEmpty();
    }
}
