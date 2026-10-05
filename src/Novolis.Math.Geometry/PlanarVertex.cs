namespace Novolis.Math.Geometry;

/// <summary>Screen-space vertex for a planar draw list (pixel X/Y, Y down).</summary>
/// <param name="X">Horizontal pixel.</param>
/// <param name="Y">Vertical pixel from the top.</param>
/// <param name="U">Texture U.</param>
/// <param name="V">Texture V.</param>
/// <param name="Color">Vertex tint.</param>
public readonly record struct PlanarVertex(float X, float Y, float U, float V, Rgba32 Color);
