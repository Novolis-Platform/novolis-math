using System.Numerics;
using Novolis.Math.Geometry;
using TUnit.Core;

namespace Novolis.Math.Geometry.Tests;

public class MeshLodExtendedTests
{
    [Test]
    public async Task Decimate_EmptyMesh_ReturnsEmpty()
    {
        var mesh = new TriangleMesh([], []);
        var lod = MeshLod.Decimate(mesh, 1, out var map);
        await Assert.That(lod.TriangleCount).IsEqualTo(0);
        await Assert.That(map).IsEmpty();
    }

    [Test]
    public async Task Decimate_InvalidTarget_Throws()
    {
        var mesh = new TriangleMesh([Vector3.Zero, Vector3.UnitX, Vector3.UnitY], [0, 1, 2]);
        var act = () => MeshLod.Decimate(mesh, 0);
        await Assert.That(act).Throws<ArgumentOutOfRangeException>();
    }

    [Test]
    public async Task Decimate_ThinSilhouette_PadsToTarget()
    {
        // Narrow strip (2 rows) — grid underfills, exercises pad branch with valid triangles.
        var verts = new List<Vector3>();
        var inds = new List<int>();
        for (var y = 0; y < 2; y++)
        for (var x = 0; x < 20; x++)
            verts.Add(new Vector3(x, 0, y * 0.001f));
        for (var x = 0; x < 19; x++)
        {
            var i = x;
            inds.Add(i); inds.Add(i + 1); inds.Add(i + 20);
            inds.Add(i + 1); inds.Add(i + 21); inds.Add(i + 20);
        }

        var mesh = new TriangleMesh(verts, inds);
        var lod = MeshLod.Decimate(mesh, 12, out var src);
        await Assert.That(lod.TriangleCount).IsGreaterThan(0);
        await Assert.That(src.Length).IsGreaterThan(0);
    }

    [Test]
    public async Task DecimateAndWeld_CompactsVertices()
    {
        var verts = new List<Vector3>();
        var inds = new List<int>();
        for (var y = 0; y < 6; y++)
        for (var x = 0; x < 6; x++)
            verts.Add(new Vector3(x * 0.001f, y, 0));
        for (var y = 0; y < 5; y++)
        for (var x = 0; x < 5; x++)
        {
            var i = y * 6 + x;
            inds.Add(i); inds.Add(i + 1); inds.Add(i + 6);
            inds.Add(i + 1); inds.Add(i + 7); inds.Add(i + 6);
        }

        var mesh = new TriangleMesh(verts, inds);
        var welded = MeshLod.DecimateAndWeld(mesh, targetTriangleCount: 10, weldTolerance: 0.01f);
        await Assert.That(welded.TriangleCount).IsLessThanOrEqualTo(10);
        await Assert.That(welded.VertexCount).IsGreaterThan(0);
    }
}
