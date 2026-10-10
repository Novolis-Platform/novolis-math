using System.Numerics;
using Novolis.Math.Geometry;

namespace Novolis.Math.Geometry.Tests;

public sealed class PlanarVectorAndRgbaTests
{
    [Test]
    public async Task NormalizeXz_DropsYAndFallsBack()
    {
        var unit = new Vector3(3f, 9f, 4f).NormalizeXz();
        await Assert.That(unit.Y).IsEqualTo(0f);
        await Assert.That(unit.LengthXz()).IsEqualTo(1f).Within(1e-5f);

        var fallback = Vector3.Zero.NormalizeXz(new Vector3(0f, 4f, -1f));
        await Assert.That(fallback).IsEqualTo(new Vector3(0f, 0f, -1f));
    }

    [Test]
    public async Task DistanceXz_IgnoresY()
    {
        var d = Vector3PlanarExtensions.DistanceXz(new Vector3(0f, 8f, 0f), new Vector3(3f, -2f, 4f));
        await Assert.That(d).IsEqualTo(5f).Within(1e-5f);
    }

    [Test]
    public async Task Rgba32_Lerp_BlendsChannels()
    {
        var mid = Rgba32.Lerp(new Rgba32(0, 0, 0, 0), new Rgba32(100, 50, 0, 200), 0.5f);
        await Assert.That(mid.R).IsEqualTo((byte)50);
        await Assert.That(mid.A).IsEqualTo((byte)100);
    }
}
