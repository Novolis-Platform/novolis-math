using System.Numerics;
using Novolis.Math.Geometry;

namespace Novolis.Math.Geometry.Tests;

public sealed class PlanarRayTests
{
    [Test]
    public async Task HitsHorizontalSegmentFromSouth()
    {
        var hit = PlanarRay.TryHitSegment(
            new Vector3(5f, 0f, 0f),
            new Vector3(0f, 0f, 1f),
            10f,
            new Vector3(0f, 0f, 4f),
            new Vector3(10f, 0f, 4f),
            out var wall);

        await Assert.That(hit).IsTrue();
        await Assert.That(wall.Distance).IsEqualTo(4f);
        await Assert.That(wall.Normal.Z).IsLessThan(0f);
    }

    [Test]
    public async Task MissesWhenRayIsParallel()
    {
        var hit = PlanarRay.TryHitSegment(
            new Vector3(0f, 0f, 0f),
            new Vector3(1f, 0f, 0f),
            10f,
            new Vector3(0f, 0f, 4f),
            new Vector3(10f, 0f, 4f),
            out _);

        await Assert.That(hit).IsFalse();
    }

    [Test]
    public async Task CircleHitOnActor()
    {
        var hit = PlanarRay.TryHitCircle(
            new Vector3(0f, 0f, 0f),
            new Vector3(1f, 0f, 0f),
            8f,
            new Vector3(5f, 0f, 0f),
            0.4f,
            out var distance);

        await Assert.That(hit).IsTrue();
        await Assert.That(distance).IsGreaterThan(4f).And.IsLessThan(5f);
    }
}
