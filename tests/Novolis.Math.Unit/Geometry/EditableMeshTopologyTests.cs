using System.Numerics;
using Novolis.Math.Geometry;
using TUnit.Core;

namespace Novolis.Math.Geometry.Tests;

public class EditableMeshTopologyTests
{
    [Test]
    public async Task Clone_FromTriangleMesh_MirrorAndTransform()
    {
        var mesh = new EditableMesh(
            [Vector3.Zero, Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ],
            [0, 1, 2, 0, 2, 3]);
        var clone = mesh.Clone();
        await Assert.That(clone.TriangleCount).IsEqualTo(mesh.TriangleCount);

        var roundTrip = EditableMesh.FromTriangleMesh(mesh.ToTriangleMesh());
        await Assert.That(roundTrip.VertexCount).IsEqualTo(4);

        var mirrored = mesh.Mirror(new Plane(Vector3.UnitX, 0));
        await Assert.That(mirrored.VertexCount).IsEqualTo(mesh.VertexCount);

        mesh.Transform(Matrix4x4.CreateTranslation(new Vector3(5f, 0f, 0f)));
        await Assert.That(mesh.Vertices[0].X).IsEqualTo(5f).Within(0.001f);
    }

    [Test]
    public async Task ReverseWinding_AndBoundaryDiscovery()
    {
        var mesh = new EditableMesh();
        var a = mesh.AddVertex(Vector3.Zero);
        var b = mesh.AddVertex(Vector3.UnitX);
        var c = mesh.AddVertex(Vector3.UnitY);
        mesh.AddTriangle(a, b, c);
        mesh.ReverseWinding();
        var edges = mesh.FindBoundaryEdges();
        var verts = mesh.FindBoundaryVertices();
        var loops = mesh.FindBoundaryLoops();
        await Assert.That(edges.Count).IsEqualTo(3);
        await Assert.That(verts.Count).IsEqualTo(3);
        await Assert.That(loops.Count).IsGreaterThanOrEqualTo(1);
    }

    [Test]
    public async Task ReplaceContents_And_InvalidTriangle_Throw()
    {
        var mesh = new EditableMesh([Vector3.Zero, Vector3.UnitX, Vector3.UnitY], [0, 1, 2]);
        mesh.ReplaceContents([Vector3.Zero, Vector3.UnitX], [0, 1, 0]);
        await Assert.That(mesh.TriangleCount).IsEqualTo(1);

        var actIdx = () => mesh.AddTriangle(0, 1, 99);
        await Assert.That(actIdx).Throws<ArgumentOutOfRangeException>();

        var actCtor = () => _ = new EditableMesh([Vector3.Zero], [0, 1]);
        await Assert.That(actCtor).Throws<ArgumentException>();
    }
}
