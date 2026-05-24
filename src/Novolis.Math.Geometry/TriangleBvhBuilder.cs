using System.Numerics;

namespace Novolis.Math.Geometry;

/// <summary>Builds a binary BVH over indexed triangles.</summary>
public static class TriangleBvhBuilder
{
    /// <summary>
    /// Builds a binary BVH over the given indexed triangle list.
    /// </summary>
    /// <param name="vertices">Vertex buffer referenced by indices.</param>
    /// <param name="triangleIndices">Three indices per triangle.</param>
    /// <returns>Acceleration structure for ray queries.</returns>
    public static TriangleBvh Build(Vector3[] vertices, int[] triangleIndices)
    {
        ArgumentNullException.ThrowIfNull(vertices);
        ArgumentNullException.ThrowIfNull(triangleIndices);
        if (triangleIndices.Length % 3 != 0)
        {
            throw new ArgumentException("Triangle index count must be a multiple of 3.", nameof(triangleIndices));
        }

        var n = triangleIndices.Length / 3;
        var order = new int[n];
        for (var i = 0; i < n; i++)
        {
            order[i] = i;
        }

        if (n == 0)
        {
            return new TriangleBvh([], order, -1, vertices, triangleIndices);
        }

        var nodes = new List<TriangleBvhNode>();
        var root = BuildRecursive(vertices, triangleIndices, order, 0, n, nodes, 0);
        return new TriangleBvh(nodes.ToArray(), order, root, vertices, triangleIndices);
    }

    private static int BuildRecursive(
        Vector3[] vertices,
        int[] triangleIndices,
        int[] triangleOrder,
        int offset,
        int count,
        List<TriangleBvhNode> nodes,
        int depth)
    {
        var bounds = ComputeBounds(vertices, triangleIndices, triangleOrder, offset, count);
        if (count <= 4 || depth > 24)
        {
            var index = nodes.Count;
            nodes.Add(new TriangleBvhNode(bounds, offset, count, 0, 0, true));
            return index;
        }

        var span = triangleOrder.AsSpan(offset, count);
        var box = bounds;
        span.Sort((a, b) =>
        {
            Centroid(vertices, triangleIndices, a, out var ca);
            Centroid(vertices, triangleIndices, b, out var cb);
            var axis = LongestAxis(box);
            var ka = axis == 0 ? ca.X : axis == 1 ? ca.Y : ca.Z;
            var kb = axis == 0 ? cb.X : axis == 1 ? cb.Y : cb.Z;
            return ka.CompareTo(kb);
        });

        var mid = count / 2;
        if (mid == 0 || mid == count)
        {
            var leaf = nodes.Count;
            nodes.Add(new TriangleBvhNode(bounds, offset, count, 0, 0, true));
            return leaf;
        }

        var thisIndex = nodes.Count;
        nodes.Add(default);
        var left = BuildRecursive(vertices, triangleIndices, triangleOrder, offset, mid, nodes, depth + 1);
        var right = BuildRecursive(vertices, triangleIndices, triangleOrder, offset + mid, count - mid, nodes, depth + 1);
        nodes[thisIndex] = new TriangleBvhNode(bounds, 0, 0, left, right, false);
        return thisIndex;
    }

    private static void Centroid(Vector3[] vertices, int[] triangleIndices, int triangleIndex, out Vector3 centroid)
    {
        var i = triangleIndex * 3;
        var v0 = vertices[triangleIndices[i]];
        var v1 = vertices[triangleIndices[i + 1]];
        var v2 = vertices[triangleIndices[i + 2]];
        centroid = (v0 + v1 + v2) / 3f;
    }

    private static AxisAlignedBox3 ComputeBounds(Vector3[] vertices, int[] triangleIndices, int[] order, int offset, int count)
    {
        TriangleBounds(vertices, triangleIndices, order[offset], out var b);
        for (var i = 1; i < count; i++)
        {
            TriangleBounds(vertices, triangleIndices, order[offset + i], out var tb);
            b = Union(b, tb);
        }

        return b;
    }

    private static void TriangleBounds(Vector3[] vertices, int[] triangleIndices, int triangleIndex, out AxisAlignedBox3 box)
    {
        var i = triangleIndex * 3;
        var v0 = vertices[triangleIndices[i]];
        var v1 = vertices[triangleIndices[i + 1]];
        var v2 = vertices[triangleIndices[i + 2]];
        box = AxisAlignedBox3.FromMinMax(v0, v0);
        box = AxisAlignedBox3.Expand(box, v1);
        box = AxisAlignedBox3.Expand(box, v2);
    }

    private static int LongestAxis(AxisAlignedBox3 b)
    {
        var e = b.Max - b.Min;
        if (e.X >= e.Y && e.X >= e.Z)
        {
            return 0;
        }

        return e.Y >= e.Z ? 1 : 2;
    }

    private static AxisAlignedBox3 Union(AxisAlignedBox3 a, AxisAlignedBox3 b) =>
        AxisAlignedBox3.FromMinMax(
            new Vector3(MathF.Min(a.Min.X, b.Min.X), MathF.Min(a.Min.Y, b.Min.Y), MathF.Min(a.Min.Z, b.Min.Z)),
            new Vector3(MathF.Max(a.Max.X, b.Max.X), MathF.Max(a.Max.Y, b.Max.Y), MathF.Max(a.Max.Z, b.Max.Z)));
}
