using System.Numerics;
using Novolis.Math.Geometry;
using TUnit.Core;

namespace Novolis.Math.Geometry.Tests;

public class EditableMeshModelingTests
{
    [Test]
    public async Task Weld_MergesCloseVertices()
    {
        var mesh = new EditableMesh(
            [new Vector3(0, 0, 0), new Vector3(0.0001f, 0, 0), new Vector3(1, 0, 0), new Vector3(0, 1, 0)],
            [0, 2, 3, 1, 2, 3]);
        MeshWeld.Apply(mesh, new WeldOptions(0.001f));
        await Assert.That(mesh.VertexCount).IsLessThan(4);
        await Assert.That(mesh.TriangleCount).IsEqualTo(2);
    }

    [Test]
    public async Task Optimize_RemovesDegenerate()
    {
        var mesh = new EditableMesh(
            [new Vector3(0, 0, 0), new Vector3(1, 0, 0), new Vector3(0, 1, 0)],
            [0, 1, 2, 0, 0, 1]);
        var result = MeshOptimize.Apply(mesh);
        await Assert.That(result.Mesh.TriangleCount).IsEqualTo(1);
        await Assert.That(result.Diagnostics.Any(d => d.Code == "degenerateFacesRemoved")).IsTrue();
    }

    [Test]
    public async Task Bridge_EqualLoops_AddsFaces()
    {
        // Two open quads sharing no verts — create two squares as separate islands then bridge
        var mesh = new EditableMesh();
        var a = new[]
        {
            mesh.AddVertex(new Vector3(0, 0, 0)),
            mesh.AddVertex(new Vector3(1, 0, 0)),
            mesh.AddVertex(new Vector3(1, 1, 0)),
            mesh.AddVertex(new Vector3(0, 1, 0)),
        };
        mesh.AddTriangle(a[0], a[1], a[2]);
        mesh.AddTriangle(a[0], a[2], a[3]);
        // Remove one face to create a boundary loop of 4? Actually full quad has boundary 4.
        var b = new[]
        {
            mesh.AddVertex(new Vector3(0, 0, 2)),
            mesh.AddVertex(new Vector3(1, 0, 2)),
            mesh.AddVertex(new Vector3(1, 1, 2)),
            mesh.AddVertex(new Vector3(0, 1, 2)),
        };
        mesh.AddTriangle(b[0], b[2], b[1]);
        mesh.AddTriangle(b[0], b[3], b[2]);

        var loops = mesh.FindBoundaryLoops();
        await Assert.That(loops.Count).IsGreaterThanOrEqualTo(2);
        var loopA = loops[0];
        var loopB = loops.First(l => l.Count == loopA.Count && !ReferenceEquals(l, loopA));
        var bridged = MeshBridge.Apply(mesh, loopA, loopB);
        await Assert.That(bridged.TriangleCount).IsGreaterThan(mesh.TriangleCount);
    }

    [Test]
    public async Task PlaneSplit_SeparatesBox()
    {
        var box = new EditableMesh(
            [
                new(-1, -1, -1), new(1, -1, -1), new(1, 1, -1), new(-1, 1, -1),
                new(-1, -1, 1), new(1, -1, 1), new(1, 1, 1), new(-1, 1, 1),
            ],
            [
                0, 1, 2, 0, 2, 3, 4, 6, 5, 4, 7, 6, 0, 4, 5, 0, 5, 1,
                2, 6, 7, 2, 7, 3, 0, 3, 7, 0, 7, 4, 1, 5, 6, 1, 6, 2,
            ]);
        var split = MeshPlaneSplit.Split(box, new Plane(Vector3.UnitX, 0));
        await Assert.That(split.Positive.TriangleCount + split.Negative.TriangleCount).IsEqualTo(box.TriangleCount);
    }

    [Test]
    public async Task Boolean_Difference_RemovesInside()
    {
        var left = new EditableMesh(
            [new(-2, -1, -1), new(2, -1, -1), new(2, 1, -1), new(-2, 1, -1),
             new(-2, -1, 1), new(2, -1, 1), new(2, 1, 1), new(-2, 1, 1)],
            [0, 1, 2, 0, 2, 3, 4, 6, 5, 4, 7, 6, 0, 4, 5, 0, 5, 1, 2, 6, 7, 2, 7, 3, 0, 3, 7, 0, 7, 4, 1, 5, 6, 1, 6, 2]);
        var right = new EditableMesh(
            [new(-0.5f, -0.5f, -0.5f), new(0.5f, -0.5f, -0.5f), new(0.5f, 0.5f, -0.5f), new(-0.5f, 0.5f, -0.5f),
             new(-0.5f, -0.5f, 0.5f), new(0.5f, -0.5f, 0.5f), new(0.5f, 0.5f, 0.5f), new(-0.5f, 0.5f, 0.5f)],
            [0, 1, 2, 0, 2, 3, 4, 6, 5, 4, 7, 6, 0, 4, 5, 0, 5, 1, 2, 6, 7, 2, 7, 3, 0, 3, 7, 0, 7, 4, 1, 5, 6, 1, 6, 2]);
        var diff = MeshBoolean.ApplySolid(left, right, MeshBooleanKind.Difference);
        await Assert.That(diff.TriangleCount).IsLessThanOrEqualTo(left.TriangleCount);
    }

    [Test]
    public async Task Boolean_Union_ConcatenatesTriangles()
    {
        var a = UnitBox();
        var b = UnitBox();
        var union = MeshBoolean.ApplySolid(a, b, MeshBooleanKind.Union);
        await Assert.That(union.TriangleCount).IsEqualTo(a.TriangleCount + b.TriangleCount);
        await Assert.That(union.VertexCount).IsEqualTo(a.VertexCount + b.VertexCount);
    }

    [Test]
    public async Task Boolean_Intersection_KeepsInsideAabb()
    {
        var left = new EditableMesh(
            [new(-2, -1, -1), new(2, -1, -1), new(2, 1, -1), new(-2, 1, -1),
             new(-2, -1, 1), new(2, -1, 1), new(2, 1, 1), new(-2, 1, 1)],
            [0, 1, 2, 0, 2, 3, 4, 6, 5, 4, 7, 6, 0, 4, 5, 0, 5, 1, 2, 6, 7, 2, 7, 3, 0, 3, 7, 0, 7, 4, 1, 5, 6, 1, 6, 2]);
        // Cover the +X face centroid (~(2,0,0)) so at least those tris are kept.
        var region = new EditableMesh(
            [
                new(1.5f, -1.2f, -1.2f), new(2.5f, -1.2f, -1.2f), new(2.5f, 1.2f, -1.2f), new(1.5f, 1.2f, -1.2f),
                new(1.5f, -1.2f, 1.2f), new(2.5f, -1.2f, 1.2f), new(2.5f, 1.2f, 1.2f), new(1.5f, 1.2f, 1.2f),
            ],
            [0, 1, 2, 0, 2, 3, 4, 6, 5, 4, 7, 6, 0, 4, 5, 0, 5, 1, 2, 6, 7, 2, 7, 3, 0, 3, 7, 0, 7, 4, 1, 5, 6, 1, 6, 2]);
        var intersect = MeshBoolean.ApplySolid(left, region, MeshBooleanKind.Intersection);
        await Assert.That(intersect.TriangleCount).IsGreaterThan(0);
        await Assert.That(intersect.TriangleCount).IsLessThanOrEqualTo(left.TriangleCount);
    }

    private static EditableMesh UnitBox() =>
        new(
            [
                new(-0.5f, -0.5f, -0.5f), new(0.5f, -0.5f, -0.5f), new(0.5f, 0.5f, -0.5f), new(-0.5f, 0.5f, -0.5f),
                new(-0.5f, -0.5f, 0.5f), new(0.5f, -0.5f, 0.5f), new(0.5f, 0.5f, 0.5f), new(-0.5f, 0.5f, 0.5f),
            ],
            [0, 1, 2, 0, 2, 3, 4, 6, 5, 4, 7, 6, 0, 4, 5, 0, 5, 1, 2, 6, 7, 2, 7, 3, 0, 3, 7, 0, 7, 4, 1, 5, 6, 1, 6, 2]);
}
