using System.Numerics;

namespace Novolis.Math.Geometry;

public enum MeshBooleanKind
{
    Union,
    Difference,
    Intersection,
}

/// <summary>
/// Lightweight mesh boolean for analytic-tessellated solids.
/// v1 uses AABB classification + keep/discard of triangles (good enough for box cutters).
/// </summary>
public static class MeshBoolean
{
    public static EditableMesh Apply(EditableMesh left, EditableMesh right, MeshBooleanKind kind)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);
        return ApplySolid(left, right, kind);
    }

    /// <summary>Union by concatenating meshes (compound). Use when Fuse is not required.</summary>
    public static EditableMesh Concat(EditableMesh a, EditableMesh b)
    {
        var result = a.Clone();
        var offset = result.VertexCount;
        foreach (var v in b.Vertices)
            result.AddVertex(v);
        for (var i = 0; i < b.Indices.Count; i += 3)
            result.AddTriangle(b.Indices[i] + offset, b.Indices[i + 1] + offset, b.Indices[i + 2] + offset);
        return result;
    }

    public static EditableMesh DifferenceKeepOutside(EditableMesh target, EditableMesh cutter) =>
        FilterTriangles(target, p =>
        {
            var (min, max) = Bounds(cutter);
            return !InsideAabb(p, min, max);
        });

    public static EditableMesh IntersectionKeepInside(EditableMesh target, EditableMesh region)
    {
        var (min, max) = Bounds(region);
        return FilterTriangles(target, p => InsideAabb(p, min, max));
    }

    public static EditableMesh ApplySolid(EditableMesh left, EditableMesh right, MeshBooleanKind kind) =>
        kind switch
        {
            MeshBooleanKind.Union => Concat(left, right),
            MeshBooleanKind.Difference => DifferenceKeepOutside(left, right),
            MeshBooleanKind.Intersection => IntersectionKeepInside(left, right),
            _ => left.Clone(),
        };

    private static EditableMesh FilterTriangles(EditableMesh mesh, Func<Vector3, bool> keepCentroid)
    {
        var result = new EditableMesh();
        var map = new Dictionary<int, int>();
        for (var i = 0; i < mesh.Indices.Count; i += 3)
        {
            var a = mesh.Indices[i];
            var b = mesh.Indices[i + 1];
            var c = mesh.Indices[i + 2];
            var centroid = (mesh.Vertices[a] + mesh.Vertices[b] + mesh.Vertices[c]) / 3f;
            if (!keepCentroid(centroid))
                continue;
            result.AddTriangle(Map(result, map, a, mesh.Vertices[a]), Map(result, map, b, mesh.Vertices[b]), Map(result, map, c, mesh.Vertices[c]));
        }

        return result;
    }

    private static int Map(EditableMesh dest, Dictionary<int, int> map, int src, Vector3 p)
    {
        if (map.TryGetValue(src, out var id))
            return id;
        id = dest.AddVertex(p);
        map[src] = id;
        return id;
    }

    private static (Vector3 Min, Vector3 Max) Bounds(EditableMesh mesh)
    {
        if (mesh.VertexCount == 0)
            return (Vector3.Zero, Vector3.Zero);
        var min = mesh.Vertices[0];
        var max = mesh.Vertices[0];
        for (var i = 1; i < mesh.VertexCount; i++)
        {
            min = Vector3.Min(min, mesh.Vertices[i]);
            max = Vector3.Max(max, mesh.Vertices[i]);
        }

        return (min, max);
    }

    private static bool InsideAabb(Vector3 p, Vector3 min, Vector3 max) =>
        p.X >= min.X && p.X <= max.X
        && p.Y >= min.Y && p.Y <= max.Y
        && p.Z >= min.Z && p.Z <= max.Z;
}
