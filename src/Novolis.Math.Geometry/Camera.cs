using System.Numerics;

namespace Novolis.Math.Geometry;

[Obsolete("Use Novolis.Simulation.View.Camera.")]
public record Camera
{
    public Vector3 Position { get; set; } = Vector3.UnitZ * 100;
    public Vector3 Target { get; set; } = Vector3.Zero;
    public float AspectRatio { get; set; } = 1.6666666666f;
    public Vector3 Up { get; set; } = Vector3.UnitY;
    public float NearPlaneDistance { get; set; } = 1f;
    public float FarPlaneDistance { get; set; } = 1_000f;
    public float FieldOfView { get; set; } = 45f;
}
