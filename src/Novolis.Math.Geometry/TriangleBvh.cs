using System.Numerics;

namespace Novolis.Math.Geometry;

/// <summary>Binary BVH node for triangle acceleration (structure only).</summary>
public readonly record struct TriangleBvhNode(
    AxisAlignedBox3 Bounds,
    int TriangleOrderOffset,
    int TriangleCount,
    int LeftChild,
    int RightChild,
    bool IsLeaf);

/// <summary>Built BVH over triangle indices into a vertex/index buffer.</summary>
public sealed class TriangleBvh
{
    /// <summary>Creates a BVH from pre-built node and triangle-order arrays.</summary>
    /// <param name="nodes">Flat node array.</param>
    /// <param name="triangleOrder">Permutation of triangle indices for leaves.</param>
    /// <param name="rootIndex">Index of the root node, or <c>-1</c> when empty.</param>
    /// <param name="vertices">Vertex buffer.</param>
    /// <param name="triangleIndices">Three indices per triangle.</param>
    public TriangleBvh(TriangleBvhNode[] nodes, int[] triangleOrder, int rootIndex, Vector3[] vertices, int[] triangleIndices)
    {
        Nodes = nodes;
        TriangleOrder = triangleOrder;
        RootIndex = rootIndex;
        Vertices = vertices;
        TriangleIndices = triangleIndices;
    }

    /// <summary>Flat node storage.</summary>
    public TriangleBvhNode[] Nodes { get; }

    /// <summary>Triangle index permutation used by leaf nodes.</summary>
    public int[] TriangleOrder { get; }

    /// <summary>Root node index, or <c>-1</c> when the mesh has no triangles.</summary>
    public int RootIndex { get; }

    /// <summary>Shared vertex buffer.</summary>
    public Vector3[] Vertices { get; }

    /// <summary>Index buffer (length is a multiple of three).</summary>
    public int[] TriangleIndices { get; }

    /// <summary>Number of triangles (<c>TriangleIndices.Length / 3</c>).</summary>
    public int TriangleCount => TriangleIndices.Length / 3;

    /// <summary>Loads the three vertices for logical triangle <paramref name="triangleIndex"/>.</summary>
    /// <param name="triangleIndex">Triangle ordinal (not index-buffer offset).</param>
    /// <param name="v0">First vertex.</param>
    /// <param name="v1">Second vertex.</param>
    /// <param name="v2">Third vertex.</param>
    public void GetTriangle(int triangleIndex, out Vector3 v0, out Vector3 v1, out Vector3 v2)
    {
        var i = triangleIndex * 3;
        v0 = Vertices[TriangleIndices[i]];
        v1 = Vertices[TriangleIndices[i + 1]];
        v2 = Vertices[TriangleIndices[i + 2]];
    }

    /// <summary>
    /// Casts a ray against the BVH and returns the closest hit within <paramref name="maxDistance"/>.
    /// </summary>
    /// <param name="ray">World-space ray.</param>
    /// <param name="maxDistance">Maximum hit distance.</param>
    /// <param name="distance">Hit distance when returning <see langword="true"/>.</param>
    /// <param name="point">Hit point in world space.</param>
    /// <param name="normal">Interpolated face normal.</param>
    /// <param name="triangleIndex">Logical triangle index of the hit, or <c>-1</c>.</param>
    /// <returns><see langword="true"/> if a triangle was hit.</returns>
    public bool Raycast(in Ray3 ray, float maxDistance, out float distance, out Vector3 point, out Vector3 normal, out int triangleIndex)
    {
        distance = 0f;
        point = default;
        normal = default;
        triangleIndex = -1;
        if (RootIndex < 0)
        {
            return false;
        }

        var bestT = maxDistance;
        var found = false;
        Traverse(RootIndex, in ray, maxDistance, ref bestT, ref found, ref distance, ref point, ref normal, ref triangleIndex);
        return found;
    }

    private void Traverse(
        int nodeIndex,
        in Ray3 ray,
        float maxDistance,
        ref float bestT,
        ref bool found,
        ref float bestDistance,
        ref Vector3 bestPoint,
        ref Vector3 bestNormal,
        ref int bestTriangle)
    {
        ref readonly var node = ref Nodes[nodeIndex];
        if (!RaySlabIntersect(node.Bounds, ray.Origin, ray.Direction, 0f, maxDistance, out var tEnter, out var tExit))
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
                var tri = TriangleOrder[node.TriangleOrderOffset + i];
                GetTriangle(tri, out var v0, out var v1, out var v2);
                if (!TriangleRay.TryHit(in ray, v0, v1, v2, bestT, out var t, out var n))
                {
                    continue;
                }

                found = true;
                bestT = t;
                bestDistance = t;
                bestPoint = ray.PointAt(t);
                bestNormal = n;
                bestTriangle = tri;
            }

            return;
        }

        Traverse(node.LeftChild, in ray, maxDistance, ref bestT, ref found, ref bestDistance, ref bestPoint, ref bestNormal, ref bestTriangle);
        Traverse(node.RightChild, in ray, maxDistance, ref bestT, ref found, ref bestDistance, ref bestPoint, ref bestNormal, ref bestTriangle);
    }

    private static bool RaySlabIntersect(
        AxisAlignedBox3 box,
        Vector3 origin,
        Vector3 dir,
        float minT,
        float maxT,
        out float tEnter,
        out float tExit)
    {
        tEnter = minT;
        tExit = maxT;
        for (var axis = 0; axis < 3; axis++)
        {
            var o = axis == 0 ? origin.X : axis == 1 ? origin.Y : origin.Z;
            var d = axis == 0 ? dir.X : axis == 1 ? dir.Y : dir.Z;
            var min = axis == 0 ? box.Min.X : axis == 1 ? box.Min.Y : box.Min.Z;
            var max = axis == 0 ? box.Max.X : axis == 1 ? box.Max.Y : box.Max.Z;
            if (MathF.Abs(d) < 1e-15f)
            {
                if (o < min || o > max)
                {
                    return false;
                }

                continue;
            }

            var invD = 1f / d;
            var t0 = (min - o) * invD;
            var t1 = (max - o) * invD;
            if (t0 > t1)
            {
                (t0, t1) = (t1, t0);
            }

            tEnter = MathF.Max(tEnter, t0);
            tExit = MathF.Min(tExit, t1);
            if (tEnter > tExit)
            {
                return false;
            }
        }

        return true;
    }
}
