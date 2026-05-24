using System.Numerics;
using Novolis.Math.Topology;
using TUnit.Core;

namespace Novolis.Math.Topology.Tests;

public class PolygonFactoryTests
{
    [Test]
    public async Task CreateCube_SingleArg_HasEightVertices()
    {
        var poly = PolygonFactory.CreateCube(2f);

        await Assert.That(poly.Length).IsEqualTo(8);
    }

    [Test]
    public async Task CreateRectangle_HasFourVertices()
    {
        var poly = PolygonFactory.CreateRectangle(4f, 2f, Vector3.Zero);

        await Assert.That(poly.Length).IsEqualTo(4);
    }

    [Test]
    public async Task CreateLine_HasTwoVertices()
    {
        var poly = PolygonFactory.CreateLine(Vector3.Zero, Vector3.UnitX);

        await Assert.That(poly.Length).IsEqualTo(2);
    }

    [Test]
    public async Task CreateCircle_HasExpectedVertexCount()
    {
        const int sides = 12;
        var poly = PolygonFactory.CreateCircle(sides, 1f, Vector3.Zero);

        await Assert.That(poly.Length).IsEqualTo(sides);
    }
}
