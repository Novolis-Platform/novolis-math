using System.Numerics;
using System.Reflection;
using Novolis.Math.Topology;
using TUnit.Core;
namespace Novolis.Math.Topology.Tests;

public class PolygonInternalsTests
{
    [Test]
    public async Task EmptyPolygon_HasNoEdgesOrFaces()
    {
        var poly = new Polygon(Array.Empty<Vector3>());
        await Assert.That(poly.Length).IsEqualTo(0);
        await Assert.That(poly.FaceCount).IsEqualTo(0);
        await Assert.That(poly.Edges.Count()).IsEqualTo(0);
    }

    [Test]
    public async Task TwoVertexPolygon_HasEdgesButNoFaces()
    {
        var poly = PolygonFactory.CreateLine(Vector3.Zero, Vector3.UnitX);
        await Assert.That(poly.Length).IsEqualTo(2);
        await Assert.That(poly.FaceCount).IsEqualTo(0);
        await Assert.That(poly.EdgesSpan.Length).IsEqualTo(2);
    }

    [Test]
    public async Task IndexerSet_MutatesVertex()
    {
        var poly = PolygonFactory.CreateTriangle(2f, 2f);
        poly[0] = new Vector3(5f, 0f, 0f);
        await Assert.That(poly[0].X).IsEqualTo(5f);
    }

    [Test]
    public async Task InternalEmptyConstructor_BuildsEmptyPolygon()
    {
        var ctor = typeof(Polygon).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, Type.EmptyTypes, null);
        var poly = (Polygon)ctor!.Invoke(null);
        await Assert.That(poly.Length).IsEqualTo(0);
        await Assert.That(poly.ToString()).Contains("Vertices");
    }

    [Test]
    public async Task Position_And_GenericEnumerator()
    {
        var poly = PolygonFactory.CreateRectangle(2f, 2f);
        await Assert.That(poly.Position.X).IsEqualTo(1f).Within(0.001f);
        var list = new List<Vector3>();
        foreach (var v in poly)
            list.Add(v);
        await Assert.That(list.Count).IsEqualTo(4);
    }

    [Test]
    public async Task GetAxisAlignedBoundingBox_Empty_ReturnsZeros()
    {
        var poly = new Polygon(Array.Empty<Vector3>());
        var (min, max) = poly.GetAxisAlignedBoundingBox();
        await Assert.That(min).IsEqualTo(Vector3.Zero);
        await Assert.That(max).IsEqualTo(Vector3.Zero);
    }

    [Test]
    public async Task Enumerator_NonGeneric_WalksVertices()
    {
        var poly = PolygonFactory.CreateTriangle(1f, 1f);
        var list = new List<Vector3>();
        foreach (Vector3 v in (System.Collections.IEnumerable)poly)
            list.Add(v);
        await Assert.That(list.Count).IsEqualTo(3);
    }
}

public class FaceExtensionsTests
{
    [Test]
    public async Task GetNormal_IsUnitLength()
    {
        var face = new Face(Vector3.Zero, Vector3.UnitX, Vector3.UnitY);
        var n = face.GetNormal();
        await Assert.That(n.Length()).IsEqualTo(1f).Within(0.001f);
        await Assert.That(n.Z).IsGreaterThan(0.9f);
    }
}
