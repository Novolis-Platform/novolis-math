using System.Numerics;
using Novolis.Math.Geometry;
using TUnit.Core;

namespace Novolis.Math.Geometry.Tests;

public class BvhRaycastTests
{
    [Test]
    public async Task Raycast_BvhMatchesBruteForce_OnUnitTriangle()
    {
        var vertices = new[] { Vector3.Zero, Vector3.UnitX, Vector3.UnitY };
        var indices = new[] { 0, 1, 2 };
        var bvh = TriangleBvhBuilder.Build(vertices, indices);
        var ray = new Ray(new Vector3(0.2f, 0.2f, 1f), -Vector3.UnitZ);

        var bvhHit = bvh.Raycast(in ray, float.MaxValue, out var bvhT, out _, out _, out var bvhTri);
        await Assert.That(bvhHit).IsTrue();
        await Assert.That(bvhTri).IsEqualTo(0);

        var bruteHit = TriangleRay.TryHit(in ray, vertices[0], vertices[1], vertices[2], float.MaxValue, out var bruteT, out _);
        await Assert.That(bruteHit).IsTrue();
        await Assert.That(bvhT).IsEqualTo(bruteT).Within(1e-5f);
    }

    [Test]
    public async Task AxisAlignedBox_RayInterval_HitsUnitCubeFace()
    {
        var box = AxisAlignedBox.FromMinMax(Vector3.Zero, Vector3.One);
        var ray = new Ray(new Vector3(0.5f, 0.5f, 2f), -Vector3.UnitZ);

        var hit = box.RayInterval(in ray, 0f, float.MaxValue, out var tEnter, out var tExit);
        await Assert.That(hit).IsTrue();
        await Assert.That(tEnter).IsEqualTo(1f).Within(1e-5f);
        await Assert.That(tExit).IsEqualTo(2f).Within(1e-5f);
    }
}
