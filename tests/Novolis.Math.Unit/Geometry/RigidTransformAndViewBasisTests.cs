using System.Numerics;
using Novolis.Math.Geometry;
using TUnit.Core;

namespace Novolis.Math.Geometry.Tests;

public class RigidTransformAndViewBasisTests
{
    [Test]
    public async Task RigidTransform_IdentityAndFromPose()
    {
        await Assert.That(RigidTransform.Identity.Position).IsEqualTo(Vector3.Zero);
        await Assert.That(RigidTransform.Identity.UniformScale).IsEqualTo(1f);

        var pose = RigidTransform.FromPose(new Vector3(1f, 2f, 3f), Quaternion.Identity, 2f);
        await Assert.That(pose.Position.Y).IsEqualTo(2f);
        await Assert.That(pose.UniformScale).IsEqualTo(2f);
    }

    [Test]
    public async Task RigidTransform_TransformPointAndDirection()
    {
        var t = RigidTransform.FromPose(
            new Vector3(10f, 0f, 0f),
            Quaternion.CreateFromAxisAngle(Vector3.UnitY, MathF.PI / 2f),
            uniformScale: 2f);
        var point = t.TransformPoint(Vector3.UnitX);
        await Assert.That(point.X).IsEqualTo(10f).Within(0.01f);
        await Assert.That(MathF.Abs(point.Z)).IsGreaterThan(1.9f);

        var dir = t.TransformDirection(Vector3.UnitX);
        await Assert.That(dir.Length()).IsEqualTo(2f).Within(0.01f);
    }

    [Test]
    public async Task RigidTransformExtensions_ChainUpdates()
    {
        var t = RigidTransform.Identity
            .Translated(new Vector3(1f, 0f, 0f))
            .MovedTo(new Vector3(2f, 0f, 0f))
            .RotatedBy(new Vector3(0f, MathF.PI / 4f, 0f))
            .ScaledBy(2f)
            .ScaledTo(3f);
        await Assert.That(t.Position.X).IsEqualTo(2f);
        await Assert.That(t.UniformScale).IsEqualTo(3f);
    }

    [Test]
    public async Task ViewBasis_FromLookAt_BuildsOrthonormalAxes()
    {
        var basis = ViewBasis.FromLookAt(
            eye: new Vector3(0f, 0f, 5f),
            target: Vector3.Zero,
            upHint: Vector3.UnitY);
        await Assert.That(basis.Forward.Length()).IsEqualTo(1f).Within(0.001f);
        await Assert.That(basis.Right.Length()).IsEqualTo(1f).Within(0.001f);
        await Assert.That(basis.Up.Length()).IsEqualTo(1f).Within(0.001f);
        await Assert.That(Vector3.Dot(basis.Forward, basis.Up)).IsEqualTo(0f).Within(0.001f);
    }

    [Test]
    public async Task ViewBasis_FromLookAt_CoincidentEyeTarget_UsesFallbackForward()
    {
        var basis = ViewBasis.FromLookAt(Vector3.Zero, Vector3.Zero, Vector3.UnitY);
        await Assert.That(basis.Forward).IsEqualTo(-Vector3.UnitZ);
    }

    [Test]
    public async Task ViewBasis_PrimaryRayDirection_IsNormalized()
    {
        var basis = ViewBasis.FromLookAt(new Vector3(0f, 0f, 10f), Vector3.Zero, Vector3.UnitY);
        var dir = ViewBasis.PrimaryRayDirection(basis, u: 0f, v: 0f, tanHalfFov: 0.5f, aspect: 1.6f);
        await Assert.That(float.IsFinite(dir.X)).IsTrue();
        await Assert.That(dir.Length()).IsEqualTo(1f).Within(0.01f);
        await Assert.That(Vector3.Dot(dir, basis.Forward)).IsGreaterThan(0.9f);
    }
}
