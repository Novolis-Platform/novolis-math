namespace Novolis.Math.Geometry;

/// <summary>One textured (or solid) triangle in a <see cref="PlanarDrawList"/>.</summary>
/// <param name="A">First vertex.</param>
/// <param name="B">Second vertex.</param>
/// <param name="C">Third vertex.</param>
/// <param name="TextureId">Registered blob id; 0 means untextured.</param>
public readonly record struct PlanarTriangle(PlanarVertex A, PlanarVertex B, PlanarVertex C, int TextureId);
