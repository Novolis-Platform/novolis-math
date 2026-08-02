using System.Numerics;
using Novolis.Math.Topology;
using TUnit.Core;

namespace Novolis.Math.Topology.Tests;

public class EdgeAndFaceFactoryTests
{
    [Test]
    public async Task EdgeExtensions_MidpointDirectionLengthAngle()
    {
        var edge = new Edge(Vector3.Zero, new Vector3(3f, 4f, 0f));
        await Assert.That(edge.GetMidpoint()).IsEqualTo(new Vector3(1.5f, 2f, 0f));
        await Assert.That(edge.GetLength()).IsEqualTo(5f).Within(0.001f);
        await Assert.That(edge.GetDirection()).IsEqualTo(new Vector3(3f, 4f, 0f));
        await Assert.That(edge.GetAngle()).IsEqualTo(MathF.Atan2(4f, 3f)).Within(0.001f);
    }

    [Test]
    public async Task EdgeExtensions_GetCharacteristicLength()
    {
        var edges = new[]
        {
            new Edge(Vector3.Zero, new Vector3(3f, 0f, 0f)),
            new Edge(Vector3.Zero, new Vector3(0f, 4f, 0f)),
        };
        var len = edges.GetCharacteristicLength();
        await Assert.That(len).IsEqualTo(3.535f).Within(0.01f);
    }

    [Test]
    public async Task EdgeExtensions_Intersect_FindsCrossingSegments()
    {
        var a = new Edge(new Vector3(0f, 0f, 0f), new Vector3(2f, 2f, 0f));
        var b = new Edge(new Vector3(0f, 2f, 0f), new Vector3(2f, 0f, 0f));
        await Assert.That(a.Intersect(b, out var point)).IsTrue();
        await Assert.That(point).IsNotNull();
        await Assert.That(point!.Value.X).IsEqualTo(1f).Within(0.01f);
        await Assert.That(a.Intersect(new[] { b })).IsTrue();
        var hits = a.GetIntersectionPoints(new[] { b }).ToList();
        await Assert.That(hits.Count).IsGreaterThan(0);
    }

    [Test]
    public async Task EdgeExtensions_Intersect_ParallelSegments_ReturnFalse()
    {
        var a = new Edge(Vector3.Zero, Vector3.UnitX);
        var b = new Edge(new Vector3(0f, 1f, 0f), new Vector3(1f, 1f, 0f));
        await Assert.That(a.Intersect(b, out _)).IsFalse();
    }

    [Test]
    public async Task EdgeExtensions_GetPointAndPoints_SampleAlongEdge()
    {
        var edge = new Edge(Vector3.Zero, new Vector3(10f, 0f, 0f));
        await Assert.That(edge.GetPoint(0.5f).X).IsEqualTo(5f).Within(0.001f);
        var samples = edge.GetPoints(step: 2f).ToList();
        await Assert.That(samples.Count).IsGreaterThan(2);
        await Assert.That(samples[0]).IsEqualTo(Vector3.Zero);
    }

    [Test]
    public async Task FaceFactory_CreateSingleAndFromPolygon()
    {
        var face = FaceFactory.Create(Vector3.Zero, Vector3.UnitX, Vector3.UnitY);
        await Assert.That(face.A).IsEqualTo(Vector3.Zero);

        var square = PolygonFactory.CreateRectangle(2f, 2f);
        var faces = FaceFactory.Create(square, parallel: false).ToList();
        await Assert.That(faces.Count).IsEqualTo(square.Length);

        var parallelFaces = FaceFactory.Create(square, parallel: true).ToList();
        await Assert.That(parallelFaces.Count).IsEqualTo(square.Length);
    }
}
