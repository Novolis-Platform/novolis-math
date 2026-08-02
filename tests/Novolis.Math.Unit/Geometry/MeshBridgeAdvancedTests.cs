using System.Numerics;
using Novolis.Math.Geometry;
using TUnit.Core;

namespace Novolis.Math.Geometry.Tests;

public class MeshBridgeAdvancedTests
{
    [Test]
    public async Task Apply_MultiSegment_InsertsIntermediateRings()
    {
        var mesh = BuildTwoQuads(out var loopA, out var loopB);
        var bridged = MeshBridge.Apply(mesh, loopA, loopB, new BridgeOptions(Segments: 3));
        await Assert.That(bridged.VertexCount).IsGreaterThan(mesh.VertexCount);
        await Assert.That(bridged.TriangleCount).IsGreaterThan(mesh.TriangleCount);
    }

    [Test]
    public async Task Apply_ReverseSecondLoop_AndTwist_ChangesConnectivity()
    {
        var mesh = BuildTwoQuads(out var loopA, out var loopB);
        var plain = MeshBridge.Apply(mesh, loopA, loopB);
        var reversed = MeshBridge.Apply(mesh, loopA, loopB, new BridgeOptions(ReverseSecondLoop: true, Twist: 1));
        await Assert.That(reversed.TriangleCount).IsEqualTo(plain.TriangleCount);
        await Assert.That(reversed.VertexCount).IsGreaterThanOrEqualTo(plain.VertexCount);
    }

    [Test]
    public async Task Apply_UnequalLoops_Throws()
    {
        var mesh = BuildTwoQuads(out var loopA, out _);
        var act = () => MeshBridge.Apply(mesh, loopA, [0, 1, 2]);
        await Assert.That(act).Throws<ArgumentException>();
    }

    [Test]
    public async Task Apply_ShortLoop_Throws()
    {
        var mesh = new EditableMesh();
        var a = mesh.AddVertex(Vector3.Zero);
        var b = mesh.AddVertex(Vector3.UnitX);
        var act = () => MeshBridge.Apply(mesh, [a, b], [a, b]);
        await Assert.That(act).Throws<ArgumentException>();
    }

    private static EditableMesh BuildTwoQuads(out int[] loopA, out int[] loopB)
    {
        var mesh = new EditableMesh();
        loopA =
        [
            mesh.AddVertex(new Vector3(0, 0, 0)),
            mesh.AddVertex(new Vector3(1, 0, 0)),
            mesh.AddVertex(new Vector3(1, 1, 0)),
            mesh.AddVertex(new Vector3(0, 1, 0)),
        ];
        mesh.AddTriangle(loopA[0], loopA[1], loopA[2]);
        mesh.AddTriangle(loopA[0], loopA[2], loopA[3]);

        loopB =
        [
            mesh.AddVertex(new Vector3(0, 0, 2)),
            mesh.AddVertex(new Vector3(1, 0, 2)),
            mesh.AddVertex(new Vector3(1, 1, 2)),
            mesh.AddVertex(new Vector3(0, 1, 2)),
        ];
        mesh.AddTriangle(loopB[0], loopB[2], loopB[1]);
        mesh.AddTriangle(loopB[0], loopB[3], loopB[2]);
        return mesh;
    }
}
