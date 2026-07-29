using System.Numerics;

namespace Novolis.Math.Geometry;

public sealed record PlaneSplitResult(EditableMesh Positive, EditableMesh Negative);

/// <summary>Clip a triangle mesh by a plane into positive / negative halves.</summary>
public static class MeshPlaneSplit
{
    public static PlaneSplitResult Split(EditableMesh mesh, Plane plane, float epsilon = 1e-5f)
    {
        ArgumentNullException.ThrowIfNull(mesh);
        var pos = new EditableMesh();
        var neg = new EditableMesh();
        var posMap = new Dictionary<int, int>();
        var negMap = new Dictionary<int, int>();

        for (var i = 0; i < mesh.Indices.Count; i += 3)
        {
            var i0 = mesh.Indices[i];
            var i1 = mesh.Indices[i + 1];
            var i2 = mesh.Indices[i + 2];
            var p0 = mesh.Vertices[i0];
            var p1 = mesh.Vertices[i1];
            var p2 = mesh.Vertices[i2];
            var d0 = Plane.DotCoordinate(plane, p0);
            var d1 = Plane.DotCoordinate(plane, p1);
            var d2 = Plane.DotCoordinate(plane, p2);

            var side0 = Classify(d0, epsilon);
            var side1 = Classify(d1, epsilon);
            var side2 = Classify(d2, epsilon);

            if (side0 >= 0 && side1 >= 0 && side2 >= 0)
            {
                AddTri(pos, posMap, i0, i1, i2, p0, p1, p2);
                continue;
            }

            if (side0 <= 0 && side1 <= 0 && side2 <= 0)
            {
                AddTri(neg, negMap, i0, i1, i2, p0, p1, p2);
                continue;
            }

            // Mixed: assign by centroid for v1 simplicity (exact clip later)
            var centroid = (p0 + p1 + p2) / 3f;
            if (Plane.DotCoordinate(plane, centroid) >= 0)
                AddTri(pos, posMap, i0, i1, i2, p0, p1, p2);
            else
                AddTri(neg, negMap, i0, i1, i2, p0, p1, p2);
        }

        return new PlaneSplitResult(pos, neg);
    }

    private static int Classify(float d, float eps)
    {
        if (d > eps)
            return 1;
        if (d < -eps)
            return -1;
        return 0;
    }

    private static void AddTri(
        EditableMesh dest,
        Dictionary<int, int> map,
        int i0,
        int i1,
        int i2,
        Vector3 p0,
        Vector3 p1,
        Vector3 p2)
    {
        dest.AddTriangle(Map(dest, map, i0, p0), Map(dest, map, i1, p1), Map(dest, map, i2, p2));
    }

    private static int Map(EditableMesh dest, Dictionary<int, int> map, int src, Vector3 p)
    {
        if (map.TryGetValue(src, out var existing))
            return existing;
        var id = dest.AddVertex(p);
        map[src] = id;
        return id;
    }
}
