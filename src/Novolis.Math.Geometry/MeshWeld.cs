using System.Numerics;

namespace Novolis.Math.Geometry;

public enum WeldPositionMode
{
    FirstVertex,
    LastVertex,
    Average,
}

public enum WeldScope
{
    SelectedVertices,
    EntireMesh,
    BoundaryOnly,
}

public sealed record WeldOptions(
    float Tolerance,
    WeldPositionMode PositionMode = WeldPositionMode.Average,
    WeldScope Scope = WeldScope.EntireMesh);

/// <summary>Spatial-hash vertex welding.</summary>
public static class MeshWeld
{
    public static EditableMesh Apply(EditableMesh mesh, WeldOptions options, IReadOnlyCollection<int>? selectedVertices = null)
    {
        ArgumentNullException.ThrowIfNull(mesh);
        if (options.Tolerance <= 0)
            throw new ArgumentOutOfRangeException(nameof(options), "Tolerance must be positive.");

        HashSet<int>? candidates = options.Scope switch
        {
            WeldScope.EntireMesh => null,
            WeldScope.BoundaryOnly => mesh.FindBoundaryVertices(),
            WeldScope.SelectedVertices => selectedVertices is null
                ? throw new ArgumentException("SelectedVertices scope requires selectedVertices.", nameof(selectedVertices))
                : selectedVertices as HashSet<int> ?? [.. selectedVertices],
            _ => null,
        };

        var cellSize = options.Tolerance;
        var buckets = new Dictionary<(int, int, int), List<int>>();
        for (var i = 0; i < mesh.VertexCount; i++)
        {
            if (candidates is not null && !candidates.Contains(i))
                continue;
            var p = mesh.Vertices[i];
            var cell = (
                (int)MathF.Floor(p.X / cellSize),
                (int)MathF.Floor(p.Y / cellSize),
                (int)MathF.Floor(p.Z / cellSize));
            if (!buckets.TryGetValue(cell, out var list))
                buckets[cell] = list = [];
            list.Add(i);
        }

        var remap = new int[mesh.VertexCount];
        for (var i = 0; i < remap.Length; i++)
            remap[i] = i;

        var visited = new bool[mesh.VertexCount];
        var tolSq = options.Tolerance * options.Tolerance;

        foreach (var (_, members) in buckets)
        {
            foreach (var i in members)
            {
                if (visited[i])
                    continue;
                var cluster = new List<int> { i };
                visited[i] = true;
                var pi = mesh.Vertices[i];
                foreach (var j in Nearby(buckets, mesh.Vertices[i], cellSize))
                {
                    if (visited[j] || j == i)
                        continue;
                    if (candidates is not null && !candidates.Contains(j))
                        continue;
                    var d = mesh.Vertices[j] - pi;
                    if (d.LengthSquared() <= tolSq)
                    {
                        cluster.Add(j);
                        visited[j] = true;
                    }
                }

                if (cluster.Count == 1)
                    continue;

                var target = ResolvePosition(mesh, cluster, options.PositionMode);
                var keep = cluster[0];
                mesh.SetVertex(keep, target);
                foreach (var idx in cluster)
                    remap[idx] = keep;
            }
        }

        // Compact unused vertices
        var used = new bool[mesh.VertexCount];
        var newIndices = new List<int>(mesh.Indices.Count);
        for (var i = 0; i < mesh.Indices.Count; i++)
        {
            var mapped = remap[mesh.Indices[i]];
            newIndices.Add(mapped);
            used[mapped] = true;
        }

        var newVerts = new List<Vector3>();
        var compact = new int[mesh.VertexCount];
        Array.Fill(compact, -1);
        for (var i = 0; i < mesh.VertexCount; i++)
        {
            if (!used[i])
                continue;
            compact[i] = newVerts.Count;
            newVerts.Add(mesh.Vertices[i]);
        }

        for (var i = 0; i < newIndices.Count; i++)
            newIndices[i] = compact[newIndices[i]];

        mesh.ReplaceContents(newVerts, newIndices);
        return mesh;
    }

    private static Vector3 ResolvePosition(EditableMesh mesh, List<int> cluster, WeldPositionMode mode)
    {
        if (mode == WeldPositionMode.FirstVertex)
            return mesh.Vertices[cluster[0]];
        if (mode == WeldPositionMode.LastVertex)
            return mesh.Vertices[cluster[^1]];

        var sum = Vector3.Zero;
        foreach (var i in cluster)
            sum += mesh.Vertices[i];
        return sum / cluster.Count;
    }

    private static IEnumerable<int> Nearby(Dictionary<(int, int, int), List<int>> buckets, Vector3 p, float cellSize)
    {
        var cx = (int)MathF.Floor(p.X / cellSize);
        var cy = (int)MathF.Floor(p.Y / cellSize);
        var cz = (int)MathF.Floor(p.Z / cellSize);
        for (var dx = -1; dx <= 1; dx++)
        for (var dy = -1; dy <= 1; dy++)
        for (var dz = -1; dz <= 1; dz++)
        {
            if (!buckets.TryGetValue((cx + dx, cy + dy, cz + dz), out var list))
                continue;
            foreach (var i in list)
                yield return i;
        }
    }
}
