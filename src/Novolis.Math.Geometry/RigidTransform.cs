using System.Numerics;

namespace Novolis.Math.Geometry;

/// <summary>Position, rotation, and uniform scale without simulation or camera semantics.</summary>
public readonly struct RigidTransform(Vector3 position, Quaternion rotation, float uniformScale = 1f)
{
    /// <summary>Translation in world space.</summary>
    public Vector3 Position { get; } = position;

    /// <summary>Rotation.</summary>
    public Quaternion Rotation { get; } = rotation;

    /// <summary>Uniform scale factor.</summary>
    public float UniformScale { get; } = uniformScale;

    /// <summary>Identity transform.</summary>
    public static RigidTransform Identity => new(Vector3.Zero, Quaternion.Identity, 1f);

    /// <summary>Builds an affine matrix (scale, rotation, translation).</summary>
    /// <returns>World matrix.</returns>
    public Matrix4x4 ToMatrix4x4()
    {
        var s = Matrix4x4.CreateScale(UniformScale);
        var r = Matrix4x4.CreateFromQuaternion(Rotation);
        var t = Matrix4x4.CreateTranslation(Position);
        return s * r * t;
    }

    /// <summary>Transforms a point.</summary>
    /// <param name="point">Local or model-space point.</param>
    /// <returns>Transformed point in world space.</returns>
    public Vector3 TransformPoint(Vector3 point) => Vector3.Transform(point, ToMatrix4x4());

    /// <summary>Transforms a direction (ignores translation; uses rotation and scale).</summary>
    /// <param name="direction">Direction vector.</param>
    /// <returns>Transformed direction.</returns>
    public Vector3 TransformDirection(Vector3 direction)
    {
        var scaled = direction * UniformScale;
        return Vector3.Transform(scaled, Rotation);
    }
}
