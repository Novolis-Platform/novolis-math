using System.Numerics;
using FluentAssertions;
using Novolis.Math.Geometry;

namespace Novolis.Math.Geometry.Tests;

public class FirstPersonCameraTests
{
    [Test]
    public void GetForwardXZ_AtYawZero_PointsAlongPositiveZ()
    {
        var cam = new FirstPersonCamera { Yaw = 0f };
        cam.GetForwardXZ().Should().Be(new Vector3(0f, 0f, 1f));
    }

    [Test]
    public void GetForwardXZ_AtYawHalfPi_PointsAlongPositiveX()
    {
        var cam = new FirstPersonCamera { Yaw = MathF.PI / 2f };
        cam.GetForwardXZ().X.Should().BeApproximately(1f, 1e-4f);
        cam.GetForwardXZ().Z.Should().BeApproximately(0f, 1e-4f);
    }

    [Test]
    public void GetRightXZ_IsPerpendicularToForwardXZ()
    {
        var cam = new FirstPersonCamera { Yaw = 0.7f };
        var dot = Vector3.Dot(cam.GetForwardXZ(), cam.GetRightXZ());
        dot.Should().BeApproximately(0f, 1e-5f);
    }

    [Test]
    public void AddLookDelta_ClampsPitch()
    {
        var cam = new FirstPersonCamera();
        cam.AddLookDelta(0f, 10f);
        cam.Pitch.Should().BeLessThan(MathF.PI / 2f);
        cam.Pitch.Should().BeGreaterThan(MathF.PI / 2f - 0.1f);
    }

    [Test]
    public void GetLookDirection_WithPitch_HasYComponent()
    {
        var cam = new FirstPersonCamera { Yaw = 0f, Pitch = 0.3f };
        cam.GetLookDirection().Y.Should().NotBe(0f);
    }
}
