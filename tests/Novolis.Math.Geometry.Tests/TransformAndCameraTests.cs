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

    [Test]
    public async Task Camera_GetViewMatrix_IsNotDefault()
    {
        var camera = new Camera
        {
            Position = new Vector3(0f, 0f, 5f),
            Target = Vector3.Zero
        };

        var view = camera.GetViewMatrix();

        await Assert.That(view).IsNotEqualTo(default(Matrix4x4));
    }

    [Test]
    public async Task Camera_GetProjectionMatrix_IsNotDefault()
    {
        var camera = new Camera { FieldOfView = 60f, AspectRatio = 16f / 9f };

        var proj = camera.GetProjectionMatrix();

        await Assert.That(proj).IsNotEqualTo(default(Matrix4x4));
    }

    [Test]
    public async Task Camera_MoveForward_TranslatesPositionAndTarget()
    {
        var camera = new Camera
        {
            Position = new Vector3(0f, 0f, 10f),
            Target = Vector3.Zero
        };
        var beforeTarget = camera.Target;

        camera.MoveForward(1f);

        await Assert.That(camera.Position.Z).IsLessThan(10f);
        await Assert.That((camera.Target - beforeTarget).Length()).IsGreaterThan(0f);
    }
}
