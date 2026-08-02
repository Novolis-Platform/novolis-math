using System.Numerics;
using Novolis.Math.Geometry;
using TUnit.Core;

namespace Novolis.Math.Geometry.Tests;

public class TriangleMeshTests
{
    [Test]
    public async Task Constructor_RejectsInvalidIndex()
    {
        var verts = new[] { Vector3.Zero, Vector3.UnitX, Vector3.UnitY };
        var bad = new[] { 0, 1, 3 };
        var act = () => _ = new TriangleMesh(verts, bad);
        await Assert.That(act).Throws<ArgumentOutOfRangeException>();
    }

    [Test]
    public async Task Translate_ShiftsBoundingBox()
    {
        var mesh = new TriangleMesh(
            [Vector3.Zero, Vector3.UnitX, Vector3.UnitY],
            [0, 1, 2]);
        var moved = mesh.Translate(new Vector3(10f, 0f, 0f));
        var (min, max) = moved.GetAxisAlignedBoundingBox();
        await Assert.That(min.X).IsEqualTo(10f).Within(0.001f);
        await Assert.That(max.X).IsEqualTo(11f).Within(0.001f);
    }

    [Test]
    public async Task GetFaces_YieldsOnePerTriangle()
    {
        var mesh = new TriangleMesh(
            [Vector3.Zero, Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ],
            [0, 1, 2, 0, 2, 3]);
        await Assert.That(mesh.GetFaces().Count()).IsEqualTo(2);
    }

    [Test]
    public async Task GetTriangle_And_Bounds_MatchVertices()
    {
        var mesh = new TriangleMesh(
            [Vector3.Zero, Vector3.UnitX, Vector3.UnitY],
            [0, 1, 2]);
        mesh.GetTriangle(0, out var v0, out var v1, out var v2);
        await Assert.That(v0).IsEqualTo(Vector3.Zero);
        await Assert.That(v1).IsEqualTo(Vector3.UnitX);
        await Assert.That(v2).IsEqualTo(Vector3.UnitY);

        var tri = mesh.TriangleBounds(0);
        await Assert.That(tri.Min.X).IsEqualTo(0f).Within(0.001f);
        await Assert.That(tri.Max.X).IsEqualTo(1f).Within(0.001f);

        var box = mesh.GetAxisAlignedBox();
        await Assert.That(box.Max.Y).IsEqualTo(1f).Within(0.001f);
    }

    [Test]
    public async Task CreateBvh_RaycastsFloorTriangle()
    {
        var mesh = new TriangleMesh(
            [
                new Vector3(-1f, 0f, -1f),
                new Vector3(1f, 0f, -1f),
                new Vector3(1f, 0f, 1f),
                new Vector3(-1f, 0f, 1f),
            ],
            [0, 1, 2, 0, 2, 3]);
        var bvh = mesh.CreateBvh();
        var hit = bvh.Raycast(new Ray(new Vector3(0f, 5f, 0f), -Vector3.UnitY), 20f, out var t, out _, out _, out _);
        await Assert.That(hit).IsTrue();
        await Assert.That(t).IsEqualTo(5f).Within(0.01f);
    }

    [Test]
    public async Task Shape_WithMesh_TransformsVertices()
    {
        var shape = new Shape
        {
            Polygon = new Polygon(Array.Empty<Vector3>()),
            TriangleMesh = new TriangleMesh(
                [Vector3.Zero, Vector3.UnitX, Vector3.UnitY],
                [0, 1, 2]),
            Color = Rgba32.White
        };
        var t = RigidTransform.FromPosition(new Vector3(0f, 5f, 0f));
        var world = shape.GetTransformedShape(t);
        await Assert.That(world.TriangleMesh).IsNotNull();
        await Assert.That(world.TriangleMesh!.Vertices[0].Y).IsEqualTo(5f).Within(0.001f);
    }
}
