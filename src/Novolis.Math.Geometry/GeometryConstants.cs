namespace Novolis.Math.Geometry;

/// <summary>Shared tolerances for intersection and ray tests.</summary>
public static class GeometryConstants
{
    /// <summary>Epsilon for parallel ray vs box slab tests.</summary>
    public const float RayParallelEpsilon = 1e-15f;

    /// <summary>Epsilon for Möller–Trumbore ray–triangle tests.</summary>
    public const float RayTriangleEpsilon = 1e-7f;
}
