using System.Numerics;
using System.Reflection;
using Novolis.Math.Topology;
using TUnit.Core;
namespace Novolis.Math.Topology.Tests;

public class FaceExtensionsTests
{
    [Test]
    public async Task GetNormal_IsUnitLength()
    {
        var face = new Face(Vector3.Zero, Vector3.UnitX, Vector3.UnitY);
        var n = face.GetNormal();
        await Assert.That(n.Length()).IsEqualTo(1f).Within(0.001f);
        await Assert.That(n.Z).IsGreaterThan(0.9f);
    }
}
