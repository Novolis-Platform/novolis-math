namespace Novolis.Math.Geometry;

/// <summary>Host-neutral planar frame: screen-space triangles plus CPU textures.</summary>
public sealed class PlanarDrawList
{
    /// <summary>Creates an empty draw list.</summary>
    public PlanarDrawList()
    {
        Textures = [];
        Triangles = [];
    }

    /// <summary>Framebuffer width in pixels.</summary>
    public int ViewportWidth { get; init; }

    /// <summary>Framebuffer height in pixels.</summary>
    public int ViewportHeight { get; init; }

    /// <summary>Clear color.</summary>
    public Rgba32 ClearColor { get; init; } = Rgba32.Black;

    /// <summary>Textures referenced by triangle ids.</summary>
    public PlanarTextureBlob[] Textures { get; init; }

    /// <summary>Triangles in paint order.</summary>
    public PlanarTriangle[] Triangles { get; init; }
}
