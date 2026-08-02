using System.Numerics;
using Novolis.Math.Geometry;
using Novolis.Math.Topology;
using TUnit.Core;

namespace Novolis.Math.Geometry.Tests;

public class ShapeExtensionsTests
{
    [Test]
    public async Task GetAxisAlignedBoundingBox_EmptyShape_ReturnsZero()
    {
        var shape = new Shape { Polygon = new Polygon(Array.Empty<Vector3>()), Color = Rgba32.White };
        var (min, max) = shape.GetAxisAlignedBoundingBox();
        await Assert.That(min).IsEqualTo(Vector3.Zero);
        await Assert.That(max).IsEqualTo(Vector3.Zero);
    }

    [Test]
    public async Task GetAxisAlignedBoundingBox_MeshOnly_UsesMeshBounds()
    {
        var mesh = new TriangleMesh(
            [Vector3.Zero, Vector3.UnitX, Vector3.UnitY],
            [0, 1, 2]);
        var shape = new Shape { TriangleMesh = mesh, Color = Rgba32.Red };
        var (min, max) = shape.GetAxisAlignedBoundingBox();
        await Assert.That(max.X).IsEqualTo(1f).Within(0.001f);
        await Assert.That(max.Y).IsEqualTo(1f).Within(0.001f);
    }

    [Test]
    public async Task GetAxisAlignedBoundingBox_PolygonAndMesh_UnionBounds()
    {
        var poly = PolygonFactory.CreateRectangle(2f, 2f, new Vector3(10f, 0f, 0f));
        var mesh = new TriangleMesh([Vector3.Zero, Vector3.UnitX, Vector3.UnitY], [0, 1, 2]);
        var shape = new Shape { Polygon = poly, TriangleMesh = mesh, Color = Rgba32.White };
        var (min, max) = shape.GetAxisAlignedBoundingBox();
        await Assert.That(min.X).IsEqualTo(0f).Within(0.001f);
        await Assert.That(max.X).IsEqualTo(11f).Within(0.001f);
    }

    [Test]
    public async Task BoundingBoxesOverlap_SeparatedShapes_ReturnsFalse()
    {
        var a = new Shape { Polygon = PolygonFactory.CreateRectangle(1f, 1f, Vector3.Zero), Color = Rgba32.White };
        var b = new Shape { Polygon = PolygonFactory.CreateRectangle(1f, 1f, new Vector3(100f, 0f, 0f)), Color = Rgba32.White };
        await Assert.That(a.BoundingBoxesOverlap(b)).IsFalse();
    }

    [Test]
    public async Task BoundingBoxesOverlap_OverlappingMeshes_ReturnsTrue()
    {
        var mesh = new TriangleMesh([Vector3.Zero, Vector3.UnitX, Vector3.UnitY], [0, 1, 2]);
        var a = new Shape { TriangleMesh = mesh, Color = Rgba32.White };
        var b = new Shape { TriangleMesh = mesh.Translate(new Vector3(0.5f, 0f, 0f)), Color = Rgba32.White };
        await Assert.That(a.BoundingBoxesOverlap(b)).IsTrue();
    }

    [Test]
    public async Task Intersect_MeshShapes_UsesAabbBroadPhase()
    {
        var mesh = new TriangleMesh([Vector3.Zero, Vector3.UnitX, Vector3.UnitY], [0, 1, 2]);
        var a = new Shape { TriangleMesh = mesh, Color = Rgba32.White };
        var b = new Shape { TriangleMesh = mesh.Translate(new Vector3(0.1f, 0f, 0f)), Color = Rgba32.White };
        await Assert.That(a.Intersect(b)).IsTrue();
    }

    [Test]
    public async Task GetIntersectionPoints_MeshOverlap_ReturnsApproximatePoint()
    {
        var mesh = new TriangleMesh([Vector3.Zero, Vector3.UnitX, Vector3.UnitY], [0, 1, 2]);
        var a = new Shape { TriangleMesh = mesh, Color = Rgba32.White };
        var b = new Shape { TriangleMesh = mesh, Color = Rgba32.White };
        var points = a.GetIntersectionPoints(b).ToList();
        await Assert.That(points.Count).IsEqualTo(1);
        await Assert.That(points[0].X).IsGreaterThan(0f).And.IsLessThan(1f);
    }

    [Test]
    public async Task GetIntersectionPoints_MeshDisjoint_ReturnsEmpty()
    {
        var mesh = new TriangleMesh([Vector3.Zero, Vector3.UnitX, Vector3.UnitY], [0, 1, 2]);
        var a = new Shape { TriangleMesh = mesh, Color = Rgba32.White };
        var b = new Shape { TriangleMesh = mesh.Translate(new Vector3(50f, 0f, 0f)), Color = Rgba32.White };
        await Assert.That(a.GetIntersectionPoints(b).Any()).IsFalse();
    }

    [Test]
    public async Task Transform_AppliesScaleRotationTranslation()
    {
        var shape = new Shape
        {
            Polygon = PolygonFactory.CreateRectangle(1f, 1f, Vector3.Zero),
            Color = Rgba32.Chartreuse,
        };
        var transform = RigidTransform.FromPose(
            new Vector3(0f, 10f, 0f),
            Quaternion.CreateFromAxisAngle(Vector3.UnitZ, MathF.PI / 2f),
            uniformScale: 2f);
        var world = shape.Transform(transform);
        await Assert.That(world.Polygon[0].Y).IsEqualTo(9f).Within(0.001f);
        await Assert.That(world.Color).IsEqualTo(Rgba32.Chartreuse);
    }

    [Test]
    public async Task GetCopy_TranslateRotateScale_DoNotMutateOriginal()
    {
        var original = new Shape
        {
            Polygon = PolygonFactory.CreateCube(1f),
            TriangleMesh = new TriangleMesh([Vector3.Zero, Vector3.UnitX, Vector3.UnitY], [0, 1, 2]),
            Color = Rgba32.Crimson,
        };
        var copy = original.GetCopy();
        _ = copy.Translate(new Vector3(5f, 0f, 0f));
        _ = copy.Rotate(Quaternion.CreateFromAxisAngle(Vector3.UnitY, 1f));
        _ = copy.Scale(3f);
        await Assert.That(original.Polygon[0]).IsEqualTo(new Vector3(0f, 0f, 0f));
        await Assert.That(original.TriangleMesh!.Vertices[0]).IsEqualTo(Vector3.Zero);
    }
}
