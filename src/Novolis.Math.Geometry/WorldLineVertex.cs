namespace Novolis.Math.Geometry;

/// <summary>Host-neutral colored world-space line vertex (Silk / apps compose; Math handshake only).</summary>
public readonly record struct WorldLineVertex(float X, float Y, float Z, Rgba32 Color);
