using System.Numerics;

namespace Novolis.Math.Geometry;

/// <summary>Obsolete perspective camera; use <c>Novolis.Simulation.View.Camera</c>.</summary>
[Obsolete("Use Novolis.Simulation.View.Camera.")]
public record Camera
{
    /// <summary>Eye position in world space.</summary>
    public Vector3 Position { get; set; } = Vector3.UnitZ * 100;

    /// <summary>Look-at target.</summary>
    public Vector3 Target { get; set; } = Vector3.Zero;

    /// <summary>Width ÷ height.</summary>
    public float AspectRatio { get; set; } = 1.6666666666f;

    /// <summary>World up vector.</summary>
    public Vector3 Up { get; set; } = Vector3.UnitY;

    /// <summary>Near clip distance.</summary>
    public float NearPlaneDistance { get; set; } = 1f;

    /// <summary>Far clip distance.</summary>
    public float FarPlaneDistance { get; set; } = 1_000f;

    /// <summary>Vertical field of view in degrees.</summary>
    public float FieldOfView { get; set; } = 45f;
}
