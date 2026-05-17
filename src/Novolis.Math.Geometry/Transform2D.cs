using System.Numerics;

namespace Novolis.Math.Geometry;

/// <summary>
///     2D transform: position of the sprite pivot, rotation, and non-uniform scale.
/// </summary>
public sealed class Transform2D
{
    /// <summary>World-space pivot (see <see sprite origin />).</summary>
    public Vector2 Position { get; set; }

    /// <summary>Clockwise rotation in degrees (Raylib / MonoGame sprite conventions).</summary>
    public float RotationDegrees { get; set; }

    public Vector2 Scale { get; set; } = Vector2.One;
}
