using System.Numerics;
using Novolis.Math.Topology;
using TUnit.Core;

namespace Novolis.Math.Topology.Tests;

public class PolygonExtensionsTests
{
    [Test]
    public async Task Intersect_OverlappingRectangles_ReturnsTrue()
    {
        var a = PolygonFactory.CreateRectangle(4f, 4f, Vector3.Zero);
        var b = PolygonFactory.CreateRectangle(4f, 4f, new Vector3(2f, 2f, 0f));
        await Assert.That(a.Intersect(b)).IsTrue();
        var points = a.GetIntersectionPoints(b).ToList();
        await Assert.That(points.Count).IsGreaterThan(0);
    }

    [Test]
    public async Task Scale_Rotate_GetCopy_AndCenter()
    {
        var poly = PolygonFactory.CreateRectangle(2f, 2f);
        var scaled = poly.Scale(3f);
        await Assert.That(scaled[1].X).IsEqualTo(6f).Within(0.001f);

        var centered = PolygonFactory.CreateRectangle(2f, 2f, Vector3.Zero);
        var rotated = centered.Rotate(Quaternion.CreateFromAxisAngle(Vector3.UnitZ, MathF.PI / 2f));
        await Assert.That(rotated[1]).IsNotEqualTo(centered[1]);

        var v0 = centered[0];
        var copy = centered.GetCopy();
        _ = copy.Translate(Vector3.UnitX);
        await Assert.That(centered[0]).IsEqualTo(v0);

        await Assert.That(poly.GetCenter().X).IsEqualTo(1f).Within(0.001f);
    }

    [Test]
    public async Task GetAxisAlignedBoundingBox_SpansAllVertices()
    {
        var tri = PolygonFactory.CreateTriangle(4f, 3f);
        var (min, max) = tri.GetAxisAlignedBoundingBox();
        await Assert.That(min).IsEqualTo(Vector3.Zero);
        await Assert.That(max.X).IsEqualTo(4f).Within(0.001f);
        await Assert.That(max.Y).IsEqualTo(3f).Within(0.001f);
    }
}
