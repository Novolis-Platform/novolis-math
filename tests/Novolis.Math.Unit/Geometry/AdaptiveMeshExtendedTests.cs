using System.Numerics;
using Novolis.Math.Geometry;
using TUnit.Core;

namespace Novolis.Math.Unit.Geometry;

public class AdaptiveMeshExtendedTests
{
    [Test]
    public async Task Handle_WithPositionAndRadius_UpdatesCopy()
    {
        var h = new AdaptiveMeshHandle(Vector3.Zero, 0.2f);
        var moved = h.WithPosition(new Vector3(1f, 2f, 3f));
        var scaled = h.WithRadius(0.5f);
        await Assert.That(moved.Position).IsEqualTo(new Vector3(1f, 2f, 3f));
        await Assert.That(scaled.Radius).IsEqualTo(0.5f);
    }

    [Test]
    public async Task Handle_InvalidRadius_Throws()
    {
        var act = () => _ = new AdaptiveMeshHandle(Vector3.Zero, 0f);
        await Assert.That(act).Throws<ArgumentOutOfRangeException>();
    }

    [Test]
    public async Task Adapt_WithScaledHandles_ChangesRadius()
    {
        var handles = new AdaptiveMeshHandle[]
        {
            new(Vector3.Zero, 0.1f),
            new(new Vector3(0f, 1f, 0f), 0.1f),
        };
        var mesh = AdaptiveMeshFactory.FromCapsuleGraph(handles, [(0, 1)], radialSegments: 6, ringsPerEdge: 2);
        var bind = mesh.BindMesh();
        var big = mesh.AdaptToMesh(new[] { handles[0].Position, handles[1].Position });
        var scaledHandles = new[] { handles[0].WithRadius(0.2f), handles[1].WithRadius(0.2f) };
        var dest = new Vector3[mesh.VertexCount];
        mesh.Adapt(scaledHandles, dest);
        var maxBind = bind.Vertices.ToArray().Max(v => v.Length());
        var maxScaled = dest.Max(v => v.Length());
        await Assert.That(maxScaled).IsGreaterThan(maxBind);
    }

    [Test]
    public async Task Constructor_Validation_RejectsBadBindings()
    {
        var handles = new[] { new AdaptiveMeshHandle(Vector3.Zero, 0.1f) };
        var bindings = new[] { AdaptiveVertexBinding.ForSphere(handle: 5, bindUnitDirection: Vector3.UnitY) };
        var act = () => _ = new AdaptiveMesh(handles, bindings, [0, 0, 0]);
        await Assert.That(act).Throws<ArgumentOutOfRangeException>();
    }

    [Test]
    public async Task Adapt_RejectsShortDestination()
    {
        var handles = new AdaptiveMeshHandle[] { new(Vector3.Zero, 0.1f) };
        var mesh = AdaptiveMeshFactory.FromCapsuleGraph(handles, [], radialSegments: 4, ringsPerEdge: 1);
        var act = () => mesh.Adapt(handles, new Vector3[1]);
        await Assert.That(act).Throws<ArgumentException>();
    }

    [Test]
    public async Task Adapt_CollapsedCapsuleHandle_UsesFallbackAxis()
    {
        var handles = new AdaptiveMeshHandle[]
        {
            new(Vector3.Zero, 0.2f),
            new(Vector3.Zero, 0.2f),
        };
        var mesh = AdaptiveMeshFactory.FromCapsuleGraph(handles, [(0, 1)], radialSegments: 4, ringsPerEdge: 1);
        var dest = new Vector3[mesh.VertexCount];
        mesh.Adapt(handles, dest);
        await Assert.That(dest[0].Length()).IsGreaterThan(0f);
    }

    [Test]
    public async Task Adapt_VerticalCapsule_UsesAlternateUpVector()
    {
        var handles = new AdaptiveMeshHandle[]
        {
            new(Vector3.Zero, 0.2f),
            new(new Vector3(0f, 1f, 0f), 0.2f),
        };
        var mesh = AdaptiveMeshFactory.FromCapsuleGraph(handles, [(0, 1)], radialSegments: 4, ringsPerEdge: 2);
        var dest = new Vector3[mesh.VertexCount];
        mesh.Adapt(new[] { Vector3.Zero, new Vector3(0f, 2f, 0f) }, dest);
        await Assert.That(dest.Max(v => v.Y)).IsGreaterThan(1.5f);
    }

    [Test]
    public async Task Constructor_RejectsBadIndex()
    {
        var handles = new[] { new AdaptiveMeshHandle(Vector3.Zero, 0.1f) };
        var bindings = new[] { AdaptiveVertexBinding.ForSphere(handle: 0, bindUnitDirection: Vector3.UnitY) };
        var act = () => _ = new AdaptiveMesh(handles, bindings, [99, 0, 0]);
        await Assert.That(act).Throws<ArgumentOutOfRangeException>();
    }

    [Test]
    public async Task Constructor_RejectsCapsuleSameHandle()
    {
        var handles = new[] { new AdaptiveMeshHandle(Vector3.Zero, 0.1f), new(Vector3.UnitY, 0.1f) };
        var bindings = new[]
        {
            AdaptiveVertexBinding.ForCapsule(handleA: 0, handleB: 0, t: 0.5f, radialY: 0.1f, radialZ: 0f),
        };
        var act = () => _ = new AdaptiveMesh(handles, bindings, [0, 0, 0]);
        await Assert.That(act).Throws<ArgumentException>();
    }
}
