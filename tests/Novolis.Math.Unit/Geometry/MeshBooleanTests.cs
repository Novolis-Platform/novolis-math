using System.Numerics;
using Novolis.Math.Geometry;
using TUnit.Core;

namespace Novolis.Math.Geometry.Tests;

public class MeshBooleanTests
{
    private static EditableMesh UnitSquareSolid()
    {
        var mesh = new EditableMesh(
            [Vector3.Zero, Vector3.UnitX, Vector3.UnitY, Vector3.One],
            [0, 1, 2, 1, 3, 2]);
        return mesh;
    }

    [Test]
    public async Task Apply_UnionDifferenceIntersection()
    {
        var left = UnitSquareSolid();
        var cutter = new EditableMesh(
            [new Vector3(0.25f, 0.25f, 0f), new Vector3(0.75f, 0.25f, 0f), new Vector3(0.25f, 0.75f, 0f), new Vector3(0.75f, 0.75f, 0f)],
            [0, 1, 2, 1, 3, 2]);

        var union = MeshBoolean.Apply(left, cutter, MeshBooleanKind.Union);
        var diff = MeshBoolean.Apply(left, cutter, MeshBooleanKind.Difference);
        var inter = MeshBoolean.Apply(left, cutter, MeshBooleanKind.Intersection);

        await Assert.That(union.TriangleCount).IsEqualTo(4);
        await Assert.That(diff.TriangleCount).IsLessThan(left.TriangleCount);
        await Assert.That(inter.TriangleCount).IsGreaterThan(0);
    }

    [Test]
    public async Task Concat_And_EmptyMeshBounds()
    {
        var a = UnitSquareSolid();
        var b = new EditableMesh([Vector3.UnitZ], [0, 0, 0]);
        var merged = MeshBoolean.Concat(a, b);
        await Assert.That(merged.VertexCount).IsGreaterThan(a.VertexCount);

        var empty = new EditableMesh([], []);
        var kept = MeshBoolean.IntersectionKeepInside(a, empty);
        await Assert.That(kept.TriangleCount).IsEqualTo(0);
    }

    [Test]
    public async Task Apply_NullMesh_Throws()
    {
        var mesh = UnitSquareSolid();
        var actLeft = () => MeshBoolean.Apply(null!, mesh, MeshBooleanKind.Union);
        var actRight = () => MeshBoolean.Apply(mesh, null!, MeshBooleanKind.Union);
        await Assert.That(actLeft).Throws<ArgumentNullException>();
        await Assert.That(actRight).Throws<ArgumentNullException>();
    }
}
