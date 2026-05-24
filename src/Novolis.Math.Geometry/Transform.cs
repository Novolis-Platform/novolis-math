using System.Numerics;

namespace Novolis.Math.Geometry;

/// <summary>
///     Represents a transform. A transform is a position, rotation, and scale.
/// </summary>
[Obsolete("Use RigidTransform.", error: false)]
public class Transform
{
    /// <summary>
    ///     The position of the transform.
    /// </summary>
    public Vector3 Position { get; set; } = Vector3.Zero;

    /// <summary>
    ///     The rotation of the transform.
    /// </summary>
    public Quaternion Rotation { get; set; } = Quaternion.Identity;

    /// <summary>
    ///     The scale of the transform.
    /// </summary>
    public float Scale { get; set; } = 1f;
}