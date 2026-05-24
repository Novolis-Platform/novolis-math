using System.Numerics;

namespace Novolis.Math.Geometry;

/// <summary>Shared binary BVH traversal for triangle acceleration structures.</summary>
public static class BvhRaycast
{
    /// <summary>Delegate for testing a logical triangle index during BVH traversal.</summary>
    /// <param name="triangleIndex">Triangle ordinal in the builder order array.</param>
    /// <param name="ray">World-space ray.</param>
    /// <param name="maxDistance">Maximum hit distance along the ray.</param>
    /// <param name="distance">Hit distance when returning <see langword="true"/>.</param>
    /// <param name="normal">Hit normal when returning <see langword="true"/>.</param>
    public delegate bool TryHitTriangleDelegate(
        int triangleIndex,
        in Ray ray,
        float maxDistance,
        out float distance,
        out Vector3 normal);

    /// <summary>
    /// Traverses a flat BVH and returns the closest hit found by <paramref name="tryHitTriangle"/>.
    /// </summary>
    /// <param name="nodes">Flat BVH node array.</param>
    /// <param name="triangleOrder">Triangle index permutation for leaf nodes.</param>
    /// <param name="rootIndex">Root node index, or <c>-1</c> when empty.</param>
    /// <param name="ray">World-space ray.</param>
    /// <param name="maxDistance">Maximum hit distance along the ray.</param>
    /// <param name="tryHitTriangle">Per-triangle hit test.</param>
    /// <param name="distance">Closest hit distance when returning <see langword="true"/>.</param>
    /// <param name="normal">Closest hit normal when returning <see langword="true"/>.</param>
    /// <param name="triangleIndex">Logical triangle index of the closest hit, or <c>-1</c>.</param>
    /// <returns><see langword="true"/> if any triangle was hit.</returns>
    public static bool Traverse(
        ReadOnlySpan<TriangleBvhNode> nodes,
        ReadOnlySpan<int> triangleOrder,
        int rootIndex,
        in Ray ray,
        float maxDistance,
        TryHitTriangleDelegate tryHitTriangle,
        out float distance,
        out Vector3 normal,
        out int triangleIndex)
    {
        distance = 0f;
        normal = default;
        triangleIndex = -1;
        if (rootIndex < 0 || nodes.IsEmpty)
        {
            return false;
        }

        var bestT = maxDistance;
        var found = false;
        TraverseNode(nodes, triangleOrder, rootIndex, in ray, maxDistance, tryHitTriangle, ref bestT, ref found, ref distance, ref normal, ref triangleIndex);
        return found;
    }

    private static void TraverseNode(
        ReadOnlySpan<TriangleBvhNode> nodes,
        ReadOnlySpan<int> triangleOrder,
        int nodeIndex,
        in Ray ray,
        float maxDistance,
        TryHitTriangleDelegate tryHitTriangle,
        ref float bestT,
        ref bool found,
        ref float bestDistance,
        ref Vector3 bestNormal,
        ref int bestTriangle)
    {
        ref readonly var node = ref nodes[nodeIndex];
        if (!AxisAlignedBox.RayInterval(node.Bounds, ray.Origin, ray.Direction, 0f, maxDistance, out var tEnter, out var tExit))
        {
            return;
        }

        if (tExit < 0f || tEnter > bestT)
        {
            return;
        }

        if (node.IsLeaf)
        {
            for (var i = 0; i < node.TriangleCount; i++)
            {
                var tri = triangleOrder[node.TriangleOrderOffset + i];
                if (!tryHitTriangle(tri, in ray, bestT, out var t, out var n))
                {
                    continue;
                }

                found = true;
                bestT = t;
                bestDistance = t;
                bestNormal = n;
                bestTriangle = tri;
            }

            return;
        }

        TraverseNode(nodes, triangleOrder, node.LeftChild, in ray, maxDistance, tryHitTriangle, ref bestT, ref found, ref bestDistance, ref bestNormal, ref bestTriangle);
        TraverseNode(nodes, triangleOrder, node.RightChild, in ray, maxDistance, tryHitTriangle, ref bestT, ref found, ref bestDistance, ref bestNormal, ref bestTriangle);
    }
}
