namespace Novolis.Math.Geometry;

/// <summary>CPU RGBA texture referenced by <see cref="PlanarTriangle.TextureId"/>.</summary>
/// <param name="Id">Positive id matching triangles.</param>
/// <param name="Width">Pixel width.</param>
/// <param name="Height">Pixel height.</param>
/// <param name="Pixels">Row-major RGBA, length <c>Width * Height</c>.</param>
public readonly record struct PlanarTextureBlob(int Id, int Width, int Height, Rgba32[] Pixels);
