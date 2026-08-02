using System.Numerics;
using Novolis.Math.Geometry;
using Novolis.Math.Topology;
using TUnit.Core;

namespace Novolis.Math.Geometry.Tests;

public class PrimitiveSerializersTests
{
    [Test]
    public async Task Vector3Serializer_RoundTrip()
    {
        var v = new Vector3(1.25f, -2.5f, 3.75f);
        var text = PrimitiveSerializers.SerializeVector3(v, "F2");
        var back = PrimitiveSerializers.DeserializeVector3(text);
        await Assert.That(back.X).IsEqualTo(v.X).Within(0.01f);
        await Assert.That(back.Y).IsEqualTo(v.Y).Within(0.01f);
        await Assert.That(back.Z).IsEqualTo(v.Z).Within(0.01f);
    }

    [Test]
    public async Task EdgeSerializer_RoundTrip()
    {
        var edge = new Edge(Vector3.Zero, Vector3.UnitX);
        var text = PrimitiveSerializers.SerializeEdge(edge);
        var back = PrimitiveSerializers.DeserializeEdge(text);
        await Assert.That(back.A).IsEqualTo(Vector3.Zero);
        await Assert.That(back.B).IsEqualTo(Vector3.UnitX);
    }

    [Test]
    public async Task SerializeLists_IncludeTitles()
    {
        var vectors = new[] { Vector3.Zero, Vector3.UnitY };
        var edges = new[] { new Edge(Vector3.Zero, Vector3.UnitX) };
        var vecText = PrimitiveSerializers.SerializeVector3s(vectors);
        var edgeText = PrimitiveSerializers.SerializeEdges(edges);
        await Assert.That(vecText).Contains("Vectors");
        await Assert.That(edgeText).Contains("Edges");
    }

    [Test]
    public async Task SerializePolygon_ReturnsEmptyString()
    {
        var poly = PolygonFactory.CreateRectangle(1f, 1f);
        await Assert.That(PrimitiveSerializers.SerializePolygon(poly)).IsEqualTo("");
    }
}
