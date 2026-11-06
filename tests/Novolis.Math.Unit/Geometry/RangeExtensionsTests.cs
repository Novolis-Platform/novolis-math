using Novolis.Math.Geometry;
using TUnit.Core;

namespace Novolis.Math.Geometry.Tests;

public sealed class RangeExtensionsTests
{
    [Test]
    public async Task GetCenter_And_CenterAligned_Homes_On_Spine()
    {
        var span = (Min: -8f, Max: -5f);
        await Assert.That(span.GetCenter()).IsEqualTo(-6.5f).Within(0.001f);
        await Assert.That(span.GetLength()).IsEqualTo(3f).Within(0.001f);

        var centered = span.CenterAligned();
        await Assert.That(centered.GetCenter()).IsEqualTo(0f).Within(0.001f);
        await Assert.That(centered.Min).IsEqualTo(-1.5f).Within(0.001f);
        await Assert.That(centered.Max).IsEqualTo(1.5f).Within(0.001f);
        await Assert.That(centered.ContainsCenter()).IsTrue();
    }
}
