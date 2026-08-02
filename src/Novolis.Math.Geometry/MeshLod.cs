using System.Numerics;

namespace Novolis.Math.Geometry;

/// <summary>
/// Lightweight mesh LOD helpers for realtime dogfood (not production remeshing).
/// </summary>
public static class MeshLod
{
    /// <summary>
    /// Reduces triangle count via spatial face bins, then compacting unused vertices.
    /// If <paramref name="mesh"/> already has ≤ <paramref name="targetTriangleCount"/> triangles, returns a copy.
    /// </summary>
    public static TriangleMesh Decimate(TriangleMesh mesh, int targetTriangleCount) =>
        Decimate(mesh, targetTriangleCount, out _);

    /// <summary>
    /// Reduces triangle count by picking faces whose centroids fall into distinct spatial bins
    /// (more even coverage than stride subsample — fewer giant holes). Compacts unused vertices.
    /// </summary>
    public static TriangleMesh Decimate(
        TriangleMesh mesh,
        int targetTriangleCount,
        out int[] sourceVertexIndices)
    {
        ArgumentNullException.ThrowIfNull(mesh);
        if (targetTriangleCount < 1)
            throw new ArgumentOutOfRangeException(nameof(targetTriangleCount), "Target must be ≥ 1.");

        var triCount = mesh.TriangleCount;
        if (triCount == 0)
        {
            sourceVertexIndices = [];
            return new TriangleMesh(Array.Empty<Vector3>(), Array.Empty<int>());
        }

        if (triCount <= targetTriangleCount)
        {
            var vCopy = new Vector3[mesh.VertexCount];
            mesh.Vertices.CopyTo(vCopy);
            var iCopy = new int[mesh.Indices.Length];
            mesh.Indices.CopyTo(iCopy);
            sourceVertexIndices = Enumerable.Range(0, mesh.VertexCount).ToArray();
            return new TriangleMesh(vCopy, iCopy);
        }

        var srcIdx = mesh.Indices;
        var srcVerts = mesh.Vertices;

        // Bounds of face centroids → grid resolution from target count.
        var min = new Vector3(float.MaxValue);
        var max = new Vector3(float.MinValue);
        var centroids = new Vector3[triCount];
        for (var f = 0; f < triCount; f++)
        {
            var i = f * 3;
            var c = (srcVerts[srcIdx[i]] + srcVerts[srcIdx[i + 1]] + srcVerts[srcIdx[i + 2]]) / 3f;
            centroids[f] = c;
            min = Vector3.Min(min, c);
            max = Vector3.Max(max, c);
        }

        var extent = Vector3.Max(max - min, new Vector3(1e-4f));
        var cellsPerAxis = System.Math.Max(2, (int)MathF.Ceiling(MathF.Pow(targetTriangleCount, 1f / 3f) * 1.6f));
        var cell = extent / cellsPerAxis;

        // Keep the face closest to each occupied cell center (stable, even).
        var best = new Dictionary<long, (int Face, float DistSq)>(targetTriangleCount * 2);
        for (var f = 0; f < triCount; f++)
        {
            var c = centroids[f];
            var ix = System.Math.Clamp((int)((c.X - min.X) / cell.X), 0, cellsPerAxis - 1);
            var iy = System.Math.Clamp((int)((c.Y - min.Y) / cell.Y), 0, cellsPerAxis - 1);
            var iz = System.Math.Clamp((int)((c.Z - min.Z) / cell.Z), 0, cellsPerAxis - 1);
            var key = ((long)ix << 42) | ((long)iy << 21) | (uint)iz;
            var center = min + new Vector3((ix + 0.5f) * cell.X, (iy + 0.5f) * cell.Y, (iz + 0.5f) * cell.Z);
            var d = Vector3.DistanceSquared(c, center);
            if (!best.TryGetValue(key, out var cur) || d < cur.DistSq)
                best[key] = (f, d);
        }

        var faces = best.Values.Select(v => v.Face).OrderBy(f => f).ToList();
        if (faces.Count > targetTriangleCount)
        {
            // Thin evenly if grid overshot.
            var stride = faces.Count / (double)targetTriangleCount;
            var thinned = new List<int>(targetTriangleCount);
            for (var k = 0; k < targetTriangleCount; k++)
                thinned.Add(faces[System.Math.Min(faces.Count - 1, (int)(k * stride))]);
            faces = thinned.Distinct().ToList();
        }
        else if (faces.Count < targetTriangleCount)
        {
            // Underfilled grid (thin silhouettes): pad with evenly spaced leftover faces.
            var have = new HashSet<int>(faces);
            var need = targetTriangleCount - faces.Count;
            var stride = triCount / (double)System.Math.Max(1, need);
            for (var k = 0; k < need * 4 && faces.Count < targetTriangleCount; k++)
            {
                var f = System.Math.Min(triCount - 1, (int)(k * stride));
                if (have.Add(f))
                    faces.Add(f);
            }
        }

        var keptFaces = new List<(int A, int B, int C)>(faces.Count);
        var used = new HashSet<int>(faces.Count * 3);
        foreach (var face in faces)
        {
            var baseIdx = face * 3;
            var a = srcIdx[baseIdx];
            var b = srcIdx[baseIdx + 1];
            var c = srcIdx[baseIdx + 2];
            if (a == b || b == c || a == c)
                continue;
            keptFaces.Add((a, b, c));
            used.Add(a);
            used.Add(b);
            used.Add(c);
        }

        if (keptFaces.Count == 0)
            throw new InvalidOperationException("Decimate produced no valid triangles.");

        var oldToNew = new Dictionary<int, int>(used.Count);
        var newVerts = new Vector3[used.Count];
        sourceVertexIndices = new int[used.Count];
        var next = 0;
        foreach (var old in used.OrderBy(i => i))
        {
            oldToNew[old] = next;
            newVerts[next] = srcVerts[old];
            sourceVertexIndices[next] = old;
            next++;
        }

        var newIndices = new int[keptFaces.Count * 3];
        for (var i = 0; i < keptFaces.Count; i++)
        {
            var (a, b, c) = keptFaces[i];
            newIndices[i * 3] = oldToNew[a];
            newIndices[i * 3 + 1] = oldToNew[b];
            newIndices[i * 3 + 2] = oldToNew[c];
        }

        return new TriangleMesh(newVerts, newIndices);
    }

    /// <summary>
    /// Decimate then weld nearby vertices (helps after face subsample).
    /// </summary>
    public static TriangleMesh DecimateAndWeld(
        TriangleMesh mesh,
        int targetTriangleCount,
        float weldTolerance = 1e-4f)
    {
        var decimated = Decimate(mesh, targetTriangleCount);
        var editable = EditableMesh.FromTriangleMesh(decimated);
        var welded = MeshWeld.Apply(editable, new WeldOptions(weldTolerance));
        return welded.ToTriangleMesh();
    }
}
