using System.Numerics;

namespace Novolis.Math.Geometry;

/// <summary>Immutable update helpers for <see cref="RigidTransform"/>.</summary>
public static class RigidTransformExtensions
{
    /// <summary>Returns a transform translated by <paramref name="direction"/>.</summary>
    public static RigidTransform Translated(this RigidTransform transform, Vector3 direction) =>
        new(transform.Position + direction, transform.Rotation, transform.UniformScale);

    /// <summary>Returns a transform with the given world position.</summary>
    public static RigidTransform MovedTo(this RigidTransform transform, Vector3 position) =>
        new(position, transform.Rotation, transform.UniformScale);

    /// <summary>Returns a transform rotated by yaw/pitch/roll radians.</summary>
    public static RigidTransform RotatedBy(this RigidTransform transform, Vector3 radians) =>
        new(transform.Position, transform.Rotation * Quaternion.CreateFromYawPitchRoll(radians.X, radians.Y, radians.Z), transform.UniformScale);

    /// <summary>Returns a transform with rotation set from yaw/pitch/roll radians.</summary>
    public static RigidTransform RotatedTo(this RigidTransform transform, Vector3 radians) =>
        new(transform.Position, Quaternion.CreateFromYawPitchRoll(radians.X, radians.Y, radians.Z), transform.UniformScale);

    /// <summary>Returns a transform with uniform scale multiplied by <paramref name="scale"/>.</summary>
    public static RigidTransform ScaledBy(this RigidTransform transform, float scale) =>
        new(transform.Position, transform.Rotation, transform.UniformScale * scale);

    /// <summary>Returns a transform with uniform scale set to <paramref name="scale"/>.</summary>
    public static RigidTransform ScaledTo(this RigidTransform transform, float scale) =>
        new(transform.Position, transform.Rotation, scale);
}
