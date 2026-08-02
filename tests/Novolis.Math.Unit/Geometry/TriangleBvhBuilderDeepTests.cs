using System.Numerics;
using Novolis.Math.Geometry;
using TUnit.Core;

namespace Novolis.Math.Geometry.Tests;

public class TriangleBvhBuilderDeepTests
{
    [Test]
    public async Task Build_LargeGrid_CreatesInternalNodesAndRaycasts()
    {
        var verts = new List<Vector3>();
        var inds = new List<int>();
        for (var y = 0; y < 8; y++)
        for (var x = 0; x < 8; x++)
            verts.Add(new Vector3(x, y, (x + y) % 3));

        for (var y = 0; y < 7; y++)
        for (var x = 0; x < 7; x++)
        {
            var i = y * 8 + x;
            inds.Add(i); inds.Add(i + 1); inds.Add(i + 8);
            inds.Add(i + 1); inds.Add(i + 9); inds.Add(i + 8);
        }

        var bvh = TriangleBvhBuilder.Build(verts.ToArray(), inds.ToArray());
        await Assert.That(bvh.TriangleCount).IsEqualTo(inds.Count / 3);
        await Assert.That(bvh.RootIndex).IsGreaterThanOrEqualTo(0);

        var hit = bvh.Raycast(
            new Ray(new Vector3(3.5f, 3.5f, 10f), -Vector3.UnitZ),
            20f,
            out var t,
            out _,
            out _,
            out _);
        await Assert.That(hit).IsTrue();
        await Assert.That(t).IsGreaterThan(0f);
    }

    [Test]
    public async Task Build_FromMesh_DelegatesToMeshBvh()
    {
        var mesh = new TriangleMesh(
            [Vector3.Zero, Vector3.UnitX, Vector3.UnitY, new Vector3(0f, 1f, 1f)],
            [0, 1, 2, 0, 2, 3]);
        var bvh = TriangleBvhBuilder.Build(mesh);
        await Assert.That(bvh.TriangleCount).IsEqualTo(2);
    }

    [Test]
    public async Task Build_AxisAlignedExtents_HitsLongestAxisBranches()
    {
        // Thin along X — forces Y/Z split logic in builder.
        var verts = new[]
        {
            new Vector3(0f, 0f, 0f), new Vector3(0.01f, 0f, 0f), new Vector3(0f, 10f, 0f),
            new Vector3(0.01f, 10f, 0f), new Vector3(0f, 0f, 10f), new Vector3(0.01f, 0f, 10f),
            new Vector3(0f, 10f, 10f), new Vector3(0.01f, 10f, 10f),
        };
        var inds = new[] { 0, 1, 2, 1, 3, 2, 4, 6, 5, 5, 6, 7, 0, 4, 2, 2, 4, 6 };
        var bvh = TriangleBvhBuilder.Build(verts, inds);
        await Assert.That(bvh.TriangleCount).IsEqualTo(6);
    }
}
