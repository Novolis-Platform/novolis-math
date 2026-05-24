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
    public bool Raycast(in Ray ray, float maxDistance, out float distance, out Vector3 point, out Vector3 normal, out int triangleIndex)
    {
        var found = BvhRaycast.Traverse(
            Nodes,
            TriangleOrder,
            RootIndex,
            in ray,
            maxDistance,
            TryHitTriangle,
            out distance,
            out normal,
            out triangleIndex);
        point = found ? ray.PointAt(distance) : default;
        return found;
    }

    private bool TryHitTriangle(int triangleIndex, in Ray ray, float maxDistance, out float distance, out Vector3 normal)
    {
        GetTriangle(triangleIndex, out var v0, out var v1, out var v2);
        return TriangleRay.TryHit(in ray, v0, v1, v2, maxDistance, out distance, out normal);
    }
}
