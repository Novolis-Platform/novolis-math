using System.Numerics;
using Novolis.Math.Geometry;
using TUnit.Core;

namespace Novolis.Math.Geometry.Tests;

public class MeshOptimizeTests
{
    [Test]
    public async Task Apply_RemovesDegenerateDuplicateAndUnusedVertices()
    {
        var mesh = new EditableMesh(
            [
                Vector3.Zero,
                Vector3.UnitX,
                Vector3.UnitY,
                new Vector3(1e-8f, 0f, 0f),
            ],
            [0, 1, 2, 0, 0, 1, 0, 1, 1, 1, 2, 3]);
        var result = MeshOptimize.Apply(mesh, new OptimizeOptions(
            WeldDuplicateVertices: true,
            RemoveDuplicateFaces: true,
            RemoveDegenerateFaces: true,
            RemoveUnusedVertices: true,
            WeldTolerance: 1e-4f));
        await Assert.That(result.Mesh.TriangleCount).IsLessThan(4);
        await Assert.That(result.Diagnostics.Count).IsGreaterThan(0);
    }

    [Test]
    public async Task Apply_ReportsNonManifoldEdges()
    {
        var mesh = new EditableMesh(
            [Vector3.Zero, Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, new Vector3(0.5f, 0.5f, 0f)],
            [0, 1, 2, 1, 2, 3, 1, 2, 4]);
        var result = MeshOptimize.Apply(mesh, new OptimizeOptions(
            WeldDuplicateVertices: false,
            RemoveDuplicateFaces: false,
            RemoveDegenerateFaces: false,
            RemoveUnusedVertices: false));
        await Assert.That(result.Diagnostics.Any(d => d.Code == "nonManifoldEdges")).IsTrue();
    }

    [Test]
    public async Task Apply_RemovesExactDuplicateFaces()
    {
        var mesh = new EditableMesh(
            [Vector3.Zero, Vector3.UnitX, Vector3.UnitY],
            [0, 1, 2, 0, 1, 2]);
        var result = MeshOptimize.Apply(mesh, new OptimizeOptions(
            WeldDuplicateVertices: false,
            RemoveDegenerateFaces: false,
            RemoveUnusedVertices: false));
        await Assert.That(result.Mesh.TriangleCount).IsEqualTo(1);
    }

    [Test]
    public async Task Apply_NullMesh_Throws()
    {
        var act = () => MeshOptimize.Apply(null!);
        await Assert.That(act).Throws<ArgumentNullException>();
    }
}
