using System.Numerics;
using Novolis.Math.Geometry;
using TUnit.Core;

namespace Novolis.Math.Geometry.Tests;

public class RigidTransformTests
{
    [Test]
    public async Task Translated_AddsToPosition()
    {
        var t = RigidTransform.FromPosition(new Vector3(1f, 2f, 3f));
        var moved = t.Translated(new Vector3(4f, 5f, 6f));
        await Assert.That(moved.Position).IsEqualTo(new Vector3(5f, 7f, 9f));
    }

    [Test]
    public async Task MovedTo_ReplacesPosition()
    {
        var t = RigidTransform.FromPosition(Vector3.One);
        var moved = t.MovedTo(Vector3.Zero);
        await Assert.That(moved.Position).IsEqualTo(Vector3.Zero);
    }

    [Test]
    public async Task ScaledBy_MultipliesScale()
    {
        var t = RigidTransform.FromPosition(Vector3.Zero, uniformScale: 2f);
        var scaled = t.ScaledBy(3f);
        await Assert.That(scaled.UniformScale).IsEqualTo(6f);
    }
}
