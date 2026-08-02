using System.Numerics;
using Novolis.Math.Geometry;
using Novolis.Math.Topology;
using TUnit.Core;

namespace Novolis.Math.Geometry.Tests;

public class ShapeFactoryAndPrimitivesTests
{
    [Test]
    public async Task ShapeFactory_CreateCube_HasEightVertices()
    {
        var shape = ShapeFactory.CreateCube(Rgba32.Red, size: 4f);
        await Assert.That(shape.Polygon.Length).IsEqualTo(8);
        await Assert.That(shape.Color).IsEqualTo(Rgba32.Red);
    }

    [Test]
    public async Task ShapeFactory_CreateSphere_HasExpectedVertexCount()
    {
        var shape = ShapeFactory.CreateSpere(Rgba32.White, radius: 5f, resolution: 8);
        await Assert.That(shape.Polygon.Length).IsEqualTo(64);
    }

    [Test]
    public async Task ShapeFactory_CreateCylinder_HasTwoRings()
    {
        var shape = ShapeFactory.CreateCylinder(Rgba32.Black, radius: 2f, height: 3f, resolution: 6);
        await Assert.That(shape.Polygon.Length).IsEqualTo(12);
    }

    [Test]
    public async Task ShapeFactory_CreateCone_HasApexPlusRing()
    {
        var shape = ShapeFactory.CreateCone(Rgba32.Chartreuse, radius: 1f, height: 2f, resolution: 5);
        await Assert.That(shape.Polygon.Length).IsEqualTo(6);
    }

    [Test]
    public async Task ShapeFactory_CreatePyramid_HasFiveVertices()
    {
        var shape = ShapeFactory.CreatePyramid(Rgba32.Crimson, radius: 2f, height: 3f);
        await Assert.That(shape.Polygon.Length).IsEqualTo(5);
    }

    [Test]
    public async Task PolygonFactory_CreateCube3D_SizesVertices()
    {
        var poly = PolygonFactory.CreateCube(2f, 3f, 4f);
        await Assert.That(poly.Length).IsEqualTo(8);
        await Assert.That(poly[6].X).IsEqualTo(2f).Within(0.001f);
        await Assert.That(poly[6].Y).IsEqualTo(3f).Within(0.001f);
        await Assert.That(poly[6].Z).IsEqualTo(4f).Within(0.001f);
    }

    [Test]
    public async Task PolygonFactory_CreateSphere_ProducesPointsOnShell()
    {
        var poly = PolygonFactory.CreateSphere(2f, segments: 4);
        await Assert.That(poly.Length).IsEqualTo(16);
        await Assert.That(poly[0].Length()).IsEqualTo(2f).Within(0.01f);
    }

    [Test]
    public async Task PolygonFactory_CreateCylinder_RingsAtZ0AndHeight()
    {
        var poly = PolygonFactory.CreateCylinder(1f, 5f, segments: 4);
        await Assert.That(poly[0].Z).IsEqualTo(0f).Within(0.001f);
        await Assert.That(poly[4].Z).IsEqualTo(5f).Within(0.001f);
    }

    [Test]
    public async Task PolygonFactory_CreateCone_ApexAtHeight()
    {
        var poly = PolygonFactory.CreateCone(1f, 4f, segments: 6);
        await Assert.That(poly[^1]).IsEqualTo(new Vector3(0f, 0f, 4f));
    }

    [Test]
    public async Task PolygonFactory_CreatePyramid_HasApex()
    {
        var poly = PolygonFactory.CreatePyramid(4f, 6f);
        await Assert.That(poly[4].Z).IsEqualTo(6f).Within(0.001f);
    }

    [Test]
    public async Task PolygonFactory_CreateTriangleAndHexagon_HaveExpectedCounts()
    {
        var tri = PolygonFactory.CreateTriangle(2f, 3f);
        var hex = PolygonFactory.CreateHexagon(1f);
        await Assert.That(tri.Length).IsEqualTo(3);
        await Assert.That(hex.Length).IsEqualTo(6);
    }

    [Test]
    public async Task PolygonFactory_CreateCircleVariants_MatchSideCount()
    {
        var centered = PolygonFactory.CreateCircle(6, radius: 2f, center: new Vector3(1f, 2f, 0f));
        var origin = PolygonFactory.CreateCircle(radius: 2f, sides: 6);
        var rect = PolygonFactory.CreateRectangle(3f, 4f);
        await Assert.That(centered.Length).IsEqualTo(6);
        await Assert.That(origin.Length).IsEqualTo(6);
        await Assert.That(rect.Length).IsEqualTo(4);
    }

    [Test]
    public async Task CapsuleAndSphere_StoreComponents()
    {
        var cap = new Capsule(Vector3.Zero, Vector3.UnitY, 0.5f);
        var sph = new Sphere(new Vector3(1f, 2f, 3f), 2f);
        await Assert.That(cap.Radius).IsEqualTo(0.5f);
        await Assert.That(cap.B).IsEqualTo(Vector3.UnitY);
        await Assert.That(sph.Center).IsEqualTo(new Vector3(1f, 2f, 3f));
        await Assert.That(sph.Radius).IsEqualTo(2f);
    }

    [Test]
    public async Task Rgba32_EqualityAndToString()
    {
        var a = new Rgba32(10, 20, 30, 40);
        var b = Rgba32.FromArgb(40, 10, 20, 30);
        await Assert.That(a).IsEqualTo(b);
        await Assert.That(a == b).IsTrue();
        await Assert.That(a != new Rgba32(0, 0, 0)).IsTrue();
        await Assert.That(a.ToString()).Contains("10");
        await Assert.That(Rgba32.White.A).IsEqualTo((byte)255);
    }

    [Test]
    public async Task LatticePointAndBounds_Equality()
    {
        var p = new LatticePoint(1, 2, 3);
        var b = new LatticeBounds(0, 0, 0, 4, 4, 4);
        await Assert.That(p).IsEqualTo(new LatticePoint(1, 2, 3));
        await Assert.That(b).IsEqualTo(new LatticeBounds(0, 0, 0, 4, 4, 4));
        await Assert.That(p != new LatticePoint(0, 0, 0)).IsTrue();
        await Assert.That(b != new LatticeBounds(1, 0, 0, 4, 4, 4)).IsTrue();
    }
}
