using Novolis.Math.Geometry;
using TUnit.Core;

namespace Novolis.Math.Geometry.Tests;

public class MeshLodTests
{
    [Test]
    public async Task Decimate_ReducesTriangleCount()
    {
        // 4x4 grid of quads → 32 tris
        var verts = new List<System.Numerics.Vector3>();
        var inds = new List<int>();
        for (var y = 0; y < 5; y++)
        for (var x = 0; x < 5; x++)
            verts.Add(new System.Numerics.Vector3(x, y, 0));

        for (var y = 0; y < 4; y++)
        for (var x = 0; x < 4; x++)
        {
            var i = y * 5 + x;
            inds.Add(i); inds.Add(i + 1); inds.Add(i + 5);
            inds.Add(i + 1); inds.Add(i + 6); inds.Add(i + 5);
        }

        var mesh = new TriangleMesh(verts, inds);
        var lod = MeshLod.Decimate(mesh, 8);
        await Assert.That(lod.TriangleCount).IsLessThanOrEqualTo(8);
        await Assert.That(lod.TriangleCount).IsGreaterThan(0);
        await Assert.That(lod.VertexCount).IsGreaterThan(0);
    }

    [Test]
    public async Task Decimate_AlreadySmall_PreservesCount()
    {
        var mesh = new TriangleMesh(
            [new(0, 0, 0), new(1, 0, 0), new(0, 1, 0)],
            [0, 1, 2]);
        var lod = MeshLod.Decimate(mesh, 100);
        await Assert.That(lod.TriangleCount).IsEqualTo(1);
        await Assert.That(lod.VertexCount).IsEqualTo(3);
    }
}
