using System.Numerics;

namespace Novolis.Math.Geometry;

/// <summary>Binary BVH node for triangle acceleration (structure only).</summary>
public readonly record struct TriangleBvhNode(
    AxisAlignedBox Bounds,
    int TriangleOrderOffset,
    int TriangleCount,
    int LeftChild,
    int RightChild,
    bool IsLeaf);
