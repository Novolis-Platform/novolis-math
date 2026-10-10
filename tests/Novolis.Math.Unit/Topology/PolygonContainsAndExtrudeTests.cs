using System.Numerics;
using Novolis.Math.Topology;

namespace Novolis.Math.Topology.Tests;

public sealed class PolygonContainsAndExtrudeTests
{
    [Test]
    public async Task ContainsXz_EvenOddOnUnitSquare()
    {
        var square = new Polygon(
        [
            new Vector3(0f, 2f, 0f),
            new Vector3(2f, 2f, 0f),
            new Vector3(2f, 2f, 2f),
            new Vector3(0f, 2f, 2f),
        ]);

        await Assert.That(square.ContainsXz(new Vector3(1f, 99f, 1f))).IsTrue();
        await Assert.That(square.ContainsXz(new Vector3(3f, 0f, 1f))).IsFalse();
    }

    [Test]
    public async Task Extrude_BuildsFourSidesAndLiftedCap()
    {
        var footprint = new Polygon(
        [
            new Vector3(0f, 0f, 0f),
            new Vector3(2f, 0f, 0f),
            new Vector3(2f, 0f, 1f),
            new Vector3(0f, 0f, 1f),
        ]);
        var offset = new Vector3(-0.2f, 0f, 0.5f);
        var prism = footprint.Extrude(offset);

        await Assert.That(prism.Sides.Count).IsEqualTo(4);
        await Assert.That(prism.Cap.Length).IsEqualTo(4);
        await Assert.That(prism.Cap[0]).IsEqualTo(footprint[0] + offset);
        await Assert.That(prism.Sides[0].Start).IsEqualTo(footprint[0]);
        await Assert.That(prism.Sides[0].End).IsEqualTo(footprint[1]);
    }
}
