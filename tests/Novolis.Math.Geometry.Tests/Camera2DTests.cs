using System.Numerics;
using FluentAssertions;
using Novolis.Math.Geometry;
using TUnit.Core;

namespace Novolis.Math.Geometry.Tests;

public class Camera2DTests
{
    [Test]
    public void Camera2D_WorldToScreen_WithIdentityZoom_CentersTarget()
    {
        var cam = new Camera2D
        {
            Target = Vector2.Zero,
            Offset = new Vector2(400f, 300f),
            Zoom = 1f,
            RotationDegrees = 0f,
            ViewportWidth = 800,
            ViewportHeight = 600
        };

        var screen = cam.WorldToScreen(Vector2.Zero);
        screen.X.Should().BeApproximately(400f, 0.01f);
        screen.Y.Should().BeApproximately(300f, 0.01f);
    }
}
