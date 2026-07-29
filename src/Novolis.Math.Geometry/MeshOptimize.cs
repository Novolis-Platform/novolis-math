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

public sealed record OptimizeResult(EditableMesh Mesh, IReadOnlyList<MeshDiagnostic> Diagnostics);

/// <summary>Mesh cleanup pipeline. Non-manifold edges are reported, not auto-repaired.</summary>
public static class MeshOptimize
{
    public static OptimizeResult Apply(EditableMesh mesh, OptimizeOptions? options = null)
    {
        ArgumentNullException.ThrowIfNull(mesh);
        options ??= new OptimizeOptions();
        var diagnostics = new List<MeshDiagnostic>();
        var work = mesh.Clone();

        if (options.RemoveDegenerateFaces)
            RemoveDegenerate(work, options.DegenerateAreaTolerance, diagnostics);

        if (options.RemoveDuplicateFaces)
            RemoveDuplicateFaces(work);

        if (options.WeldDuplicateVertices && options.WeldTolerance > 0)
            MeshWeld.Apply(work, new WeldOptions(options.WeldTolerance));

        if (options.RemoveUnusedVertices)
            RemoveUnused(work);

        ReportNonManifold(work, diagnostics);

        return new OptimizeResult(work, diagnostics);
    }

    private static void RemoveDegenerate(EditableMesh mesh, float areaTol, List<MeshDiagnostic> diagnostics)
    {
        var verts = mesh.Vertices;
        var kept = new List<int>();
        var removed = 0;
        for (var i = 0; i < mesh.Indices.Count; i += 3)
        {
            var a = mesh.Indices[i];
            var b = mesh.Indices[i + 1];
            var c = mesh.Indices[i + 2];
            if (a == b || b == c || a == c)
            {
                removed++;
                continue;
            }

            var ab = verts[b] - verts[a];
            var ac = verts[c] - verts[a];
            var area2 = Vector3.Cross(ab, ac).LengthSquared();
            if (area2 < areaTol)
            {
                removed++;
                continue;
            }

            kept.Add(a);
            kept.Add(b);
            kept.Add(c);
        }

        if (removed > 0)
        {
            diagnostics.Add(new MeshDiagnostic(
                MeshDiagnosticSeverity.Info,
                "degenerateFacesRemoved",
                $"Removed {removed} degenerate face(s).",
                []));
            mesh.ReplaceContents(verts.ToList(), kept);
        }
    }

    private static void RemoveDuplicateFaces(EditableMesh mesh)
    {
        var seen = new HashSet<(int, int, int)>();
        var kept = new List<int>();
        for (var i = 0; i < mesh.Indices.Count; i += 3)
        {
            var a = mesh.Indices[i];
            var b = mesh.Indices[i + 1];
            var c = mesh.Indices[i + 2];
            var key = CanonicalFace(a, b, c);
            if (!seen.Add(key))
                continue;
            kept.Add(a);
            kept.Add(b);
            kept.Add(c);
        }

        if (kept.Count != mesh.Indices.Count)
            mesh.ReplaceContents(mesh.Vertices.ToList(), kept);
    }

    private static void RemoveUnused(EditableMesh mesh)
    {
        var used = new bool[mesh.VertexCount];
        foreach (var i in mesh.Indices)
            used[i] = true;
        var compact = new int[mesh.VertexCount];
        var newVerts = new List<Vector3>();
        for (var i = 0; i < mesh.VertexCount; i++)
        {
            if (!used[i])
            {
                compact[i] = -1;
                continue;
            }

            compact[i] = newVerts.Count;
            newVerts.Add(mesh.Vertices[i]);
        }

        var newInds = mesh.Indices.Select(i => compact[i]).ToList();
        mesh.ReplaceContents(newVerts, newInds);
    }

    private static void ReportNonManifold(EditableMesh mesh, List<MeshDiagnostic> diagnostics)
    {
        var edgeCounts = new Dictionary<(int, int), int>();
        for (var i = 0; i < mesh.Indices.Count; i += 3)
        {
            Count(edgeCounts, mesh.Indices[i], mesh.Indices[i + 1]);
            Count(edgeCounts, mesh.Indices[i + 1], mesh.Indices[i + 2]);
            Count(edgeCounts, mesh.Indices[i + 2], mesh.Indices[i]);
        }

        var bad = edgeCounts.Where(kv => kv.Value > 2).Select(kv => kv.Key.Item1).Distinct().Take(32).ToList();
        if (bad.Count > 0)
        {
            diagnostics.Add(new MeshDiagnostic(
                MeshDiagnosticSeverity.Warning,
                "nonManifoldEdges",
                $"Detected {edgeCounts.Count(kv => kv.Value > 2)} non-manifold edge(s).",
                bad));
        }
    }

    private static void Count(Dictionary<(int, int), int> map, int a, int b)
    {
        var key = a < b ? (a, b) : (b, a);
        map.TryGetValue(key, out var c);
        map[key] = c + 1;
    }

    private static (int, int, int) CanonicalFace(int a, int b, int c)
    {
        // rotation-invariant, orientation-sensitive enough via sorted rotation start
        if (a <= b && a <= c)
            return (a, b, c);
        if (b <= a && b <= c)
            return (b, c, a);
        return (c, a, b);
    }
}
