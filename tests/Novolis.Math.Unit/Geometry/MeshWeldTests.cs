using System.Numerics;
using Novolis.Math.Geometry;
using TUnit.Core;

namespace Novolis.Math.Geometry.Tests;

public class MeshWeldTests
{
    [Test]
    public async Task Apply_WeldsNearbyVertices()
    {
        var mesh = new EditableMesh(
            [Vector3.Zero, new Vector3(1e-6f, 0f, 0f), Vector3.UnitY],
            [0, 1, 2]);
        var welded = MeshWeld.Apply(mesh, new WeldOptions(0.01f));
        await Assert.That(welded.VertexCount).IsLessThan(3);
    }

    [Test]
    public async Task Apply_BoundaryScope_OnlyWeldsBoundary()
    {
        var mesh = new EditableMesh(
            [Vector3.Zero, Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ],
            [0, 1, 2, 0, 2, 3]);
        var welded = MeshWeld.Apply(mesh, new WeldOptions(1e-4f, Scope: WeldScope.BoundaryOnly));
        await Assert.That(welded.VertexCount).IsGreaterThan(0);
    }

    [Test]
    public async Task Apply_SelectedScope_RequiresSelection()
    {
        var mesh = new EditableMesh([Vector3.Zero, Vector3.UnitX, Vector3.UnitY], [0, 1, 2]);
        var act = () => MeshWeld.Apply(mesh, new WeldOptions(0.01f, Scope: WeldScope.SelectedVertices));
        await Assert.That(act).Throws<ArgumentException>();
        var welded = MeshWeld.Apply(mesh, new WeldOptions(0.01f, Scope: WeldScope.SelectedVertices), selectedVertices: [0, 1]);
        await Assert.That(welded.VertexCount).IsGreaterThan(0);
    }

    [Test]
    public async Task Apply_InvalidTolerance_Throws()
    {
        var mesh = new EditableMesh([Vector3.Zero, Vector3.UnitX, Vector3.UnitY], [0, 1, 2]);
        var act = () => MeshWeld.Apply(mesh, new WeldOptions(0f));
        await Assert.That(act).Throws<ArgumentOutOfRangeException>();
    }
}
