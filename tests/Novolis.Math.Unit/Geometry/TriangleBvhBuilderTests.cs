using System.Numerics;
using Novolis.Math.Geometry;
using TUnit.Core;

namespace Novolis.Math.Geometry.Tests;

public class TriangleBvhBuilderTests
{
    [Test]
    public async Task Build_FromMesh_MatchesCreateBvh()
    {
        var mesh = new TriangleMesh(
            [Vector3.Zero, Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ],
            [0, 1, 2, 0, 2, 3]);
        var fromBuilder = TriangleBvhBuilder.Build(mesh);
        var fromMesh = mesh.CreateBvh();
        await Assert.That(fromBuilder.TriangleCount).IsEqualTo(fromMesh.TriangleCount);
    }

    [Test]
    public async Task Build_FromArrays_RaycastsTriangle()
    {
        var verts = new[]
        {
            new Vector3(-1f, 0f, -1f),
            new Vector3(1f, 0f, -1f),
            new Vector3(1f, 0f, 1f),
            new Vector3(-1f, 0f, 1f),
        };
        var indices = new[] { 0, 1, 2, 0, 2, 3 };
        var bvh = TriangleBvhBuilder.Build(verts, indices);
        var hit = bvh.Raycast(new Ray(new Vector3(0f, 5f, 0f), -Vector3.UnitY), 20f, out var t, out _, out _, out _);
        await Assert.That(hit).IsTrue();
        await Assert.That(t).IsEqualTo(5f).Within(0.05f);
    }

    [Test]
    public async Task Build_EmptyIndices_ReturnsEmptyBvh()
    {
        var bvh = TriangleBvhBuilder.Build([Vector3.Zero], []);
        await Assert.That(bvh.TriangleCount).IsEqualTo(0);
    }

    [Test]
    public async Task Build_InvalidIndexCount_Throws()
    {
        var act = () => TriangleBvhBuilder.Build([Vector3.Zero, Vector3.UnitX, Vector3.UnitY], [0, 1]);
        await Assert.That(act).Throws<ArgumentException>();
    }

    [Test]
    public async Task Build_NullArguments_Throw()
    {
        await Assert.That(() => TriangleBvhBuilder.Build(null!, [0, 1, 2])).Throws<ArgumentNullException>();
        await Assert.That(() => TriangleBvhBuilder.Build([Vector3.Zero], null!)).Throws<ArgumentNullException>();
    }
}
