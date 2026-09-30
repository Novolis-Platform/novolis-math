using System.Numerics;

namespace Novolis.Math.Geometry;

public sealed record OptimizeOptions(
    bool WeldDuplicateVertices = true,
    bool RemoveDuplicateFaces = true,
    bool RemoveDegenerateFaces = true,
    bool RemoveUnusedVertices = true,
    bool FixFaceWinding = false,
    float WeldTolerance = 1e-5f,
    float DegenerateAreaTolerance = 1e-12f);
