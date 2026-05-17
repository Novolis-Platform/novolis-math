using System.Numerics;
using Novolis.Math.Geometry;
using TUnit.Core;

namespace Novolis.Math.Geometry.Tests;

public class TransformAndCameraTests
{
    [Test]
    public async Task Translate_AddsToPosition()
    {
        var t = new Transform { Position = new Vector3(1f, 2f, 3f) };
        t.Translate(new Vector3(4f, 5f, 6f));
        await Assert.That(t.Position).IsEqualTo(new Vector3(5f, 7f, 9f));
    }

    [Test]
    public async Task MoveTo_ReplacesPosition()
    {
        var t = new Transform { Position = Vector3.One };
        t.MoveTo(Vector3.Zero);
        await Assert.That(t.Position).IsEqualTo(Vector3.Zero);
    }

    [Test]
    public async Task ScaleBy_MultipliesScale()
    {
        var t = new Transform { Scale = 2f };
        t.ScaleBy(3f);
        await Assert.That(t.Scale).IsEqualTo(6f);
    }

}
