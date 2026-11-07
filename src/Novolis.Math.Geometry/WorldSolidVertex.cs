namespace Novolis.Math.Geometry;

/// <summary>Host-neutral colored world-space solid vertex (Silk / apps compose; Math handshake only).</summary>
public readonly record struct WorldSolidVertex(float X, float Y, float Z, Rgba32 Color);
