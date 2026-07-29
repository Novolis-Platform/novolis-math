using System.Numerics;

namespace Novolis.Math.Geometry;

/// <summary>Mutable indexed triangle mesh with topology helpers for modeling ops.</summary>
public sealed class EditableMesh
{
    private readonly List<Vector3> _vertices;
    private readonly List<int> _indices;

    public EditableMesh()
    {
        _vertices = [];
        _indices = [];
    }

    public EditableMesh(IReadOnlyList<Vector3> vertices, IReadOnlyList<int> indices)
    {
        ArgumentNullException.ThrowIfNull(vertices);
        ArgumentNullException.ThrowIfNull(indices);
        if (indices.Count % 3 != 0)
            throw new ArgumentException("Index count must be a multiple of 3.", nameof(indices));

        _vertices = [.. vertices];
        _indices = [.. indices];
        ValidateIndices();
    }

    public IReadOnlyList<Vector3> Vertices => _vertices;

    public IReadOnlyList<int> Indices => _indices;

    public int VertexCount => _vertices.Count;

    public int TriangleCount => _indices.Count / 3;

    public EditableMesh Clone() => new(_vertices, _indices);

    public TriangleMesh ToTriangleMesh() => new(_vertices, _indices);

    public static EditableMesh FromTriangleMesh(TriangleMesh mesh)
    {
        var verts = new Vector3[mesh.VertexCount];
        mesh.Vertices.CopyTo(verts);
        var inds = new int[mesh.Indices.Length];
        mesh.Indices.CopyTo(inds);
        return new EditableMesh(verts, inds);
    }

    public int AddVertex(Vector3 position)
    {
        _vertices.Add(position);
        return _vertices.Count - 1;
    }

    public void AddTriangle(int a, int b, int c)
    {
        if (a < 0 || b < 0 || c < 0 || a >= _vertices.Count || b >= _vertices.Count || c >= _vertices.Count)
            throw new ArgumentOutOfRangeException(nameof(a), "Triangle indices out of range.");
        _indices.Add(a);
        _indices.Add(b);
        _indices.Add(c);
    }

    public void SetVertex(int index, Vector3 position) => _vertices[index] = position;

    public void Transform(Matrix4x4 matrix)
    {
        for (var i = 0; i < _vertices.Count; i++)
            _vertices[i] = Vector3.Transform(_vertices[i], matrix);
    }

    public void ReverseWinding()
    {
        for (var i = 0; i < _indices.Count; i += 3)
            (_indices[i + 1], _indices[i + 2]) = (_indices[i + 2], _indices[i + 1]);
    }

    public EditableMesh Mirror(Plane plane)
    {
        var mirrored = Clone();
        for (var i = 0; i < mirrored._vertices.Count; i++)
        {
            var p = mirrored._vertices[i];
            var d = Plane.DotCoordinate(plane, p);
            mirrored._vertices[i] = p - 2f * d * plane.Normal;
        }

        mirrored.ReverseWinding();
        return mirrored;
    }

    /// <summary>Boundary edges as undirected pairs (min,max) appearing once.</summary>
    public List<(int A, int B)> FindBoundaryEdges()
    {
        var counts = new Dictionary<(int, int), int>();
        for (var i = 0; i < _indices.Count; i += 3)
        {
            AddEdge(counts, _indices[i], _indices[i + 1]);
            AddEdge(counts, _indices[i + 1], _indices[i + 2]);
            AddEdge(counts, _indices[i + 2], _indices[i]);
        }

        return counts.Where(kv => kv.Value == 1).Select(kv => kv.Key).ToList();
    }

    public HashSet<int> FindBoundaryVertices()
    {
        var set = new HashSet<int>();
        foreach (var (a, b) in FindBoundaryEdges())
        {
            set.Add(a);
            set.Add(b);
        }

        return set;
    }

    /// <summary>Ordered closed loops of boundary vertices (best-effort).</summary>
    public List<List<int>> FindBoundaryLoops()
    {
        var edges = FindBoundaryEdges();
        var adj = new Dictionary<int, List<int>>();
        foreach (var (a, b) in edges)
        {
            if (!adj.TryGetValue(a, out var la))
                adj[a] = la = [];
            if (!adj.TryGetValue(b, out var lb))
                adj[b] = lb = [];
            la.Add(b);
            lb.Add(a);
        }

        var used = new HashSet<(int, int)>();
        var loops = new List<List<int>>();
        foreach (var start in adj.Keys.OrderBy(i => i))
        {
            foreach (var next in adj[start])
            {
                var key = EdgeKey(start, next);
                if (!used.Add(key))
                    continue;

                var loop = new List<int> { start };
                var prev = start;
                var cur = next;
                while (cur != start)
                {
                    loop.Add(cur);
                    if (!adj.TryGetValue(cur, out var neighbors))
                        break;
                    var advanced = false;
                    foreach (var n in neighbors)
                    {
                        if (n == prev)
                            continue;
                        var ek = EdgeKey(cur, n);
                        if (!used.Add(ek))
                            continue;
                        prev = cur;
                        cur = n;
                        advanced = true;
                        break;
                    }

                    if (!advanced)
                        break;
                }

                if (loop.Count >= 3)
                    loops.Add(loop);
            }
        }

        return loops;
    }

    public void ReplaceContents(IReadOnlyList<Vector3> vertices, IReadOnlyList<int> indices)
    {
        _vertices.Clear();
        _vertices.AddRange(vertices);
        _indices.Clear();
        _indices.AddRange(indices);
        if (_indices.Count % 3 != 0)
            throw new ArgumentException("Index count must be a multiple of 3.", nameof(indices));
        ValidateIndices();
    }

    private void ValidateIndices()
    {
        for (var i = 0; i < _indices.Count; i++)
        {
            var idx = _indices[i];
            if (idx < 0 || idx >= _vertices.Count)
                throw new ArgumentOutOfRangeException(nameof(_indices), $"Index {idx} out of range.");
        }
    }

    private static void AddEdge(Dictionary<(int, int), int> counts, int a, int b)
    {
        var key = EdgeKey(a, b);
        counts.TryGetValue(key, out var c);
        counts[key] = c + 1;
    }

    private static (int, int) EdgeKey(int a, int b) => a < b ? (a, b) : (b, a);
}
