using System.Numerics;

namespace Novolis.Math.Geometry;

/// <summary>
///     Indexed triangle list for arbitrary meshes (OBJ with <c>f</c> lines, FBX/glTF via Assimp). Use instead of
///     <see cref="Polygon" /> when geometry is not a single closed vertex loop fan-triangulated from vertex 0.
/// </summary>
public sealed class TriangleMesh
{
    private readonly Vector3[] _vertices;
    private readonly int[] _indices;

    /// <summary>
    ///     Creates a mesh from vertex positions and a triangle index list (3 indices per triangle, counter-clockwise when
    ///     viewed from the front face).
    /// </summary>
    public TriangleMesh(IReadOnlyList<Vector3> vertices, IReadOnlyList<int> indices)
    {
        ArgumentNullException.ThrowIfNull(vertices);
        ArgumentNullException.ThrowIfNull(indices);
        if (indices.Count % 3 != 0)
            throw new ArgumentException("Index count must be a multiple of 3 for triangles.", nameof(indices));

        _vertices = vertices.Count == 0 ? Array.Empty<Vector3>() : new Vector3[vertices.Count];
        for (var i = 0; i < vertices.Count; i++)
            _vertices[i] = vertices[i];

        _indices = indices.Count == 0 ? Array.Empty<int>() : new int[indices.Count];
        for (var i = 0; i < indices.Count; i++)
            _indices[i] = indices[i];

        if (_vertices.Length == 0 && _indices.Length != 0)
            throw new ArgumentException("Cannot have indices with zero vertices.", nameof(indices));

        for (var i = 0; i < _indices.Length; i++)
        {
            var idx = _indices[i];
            if (idx < 0 || idx >= _vertices.Length)
                throw new ArgumentOutOfRangeException(nameof(indices), $"Index {idx} is out of range for {_vertices.Length} vertices.");
        }
    }

    /// <summary>Vertex positions.</summary>
    public ReadOnlySpan<Vector3> Vertices => _vertices;

    /// <summary>Triangle index buffer (three indices per triangle).</summary>
    public ReadOnlySpan<int> Indices => _indices;

    /// <summary>Number of vertices.</summary>
    public int VertexCount => _vertices.Length;

    /// <summary>Number of triangles (<c>Indices.Length / 3</c>).</summary>
    public int TriangleCount => _indices.Length / 3;

    /// <summary>Returns the three vertices of triangle <paramref name="triangleIndex"/>.</summary>
    public void GetTriangle(int triangleIndex, out Vector3 v0, out Vector3 v1, out Vector3 v2)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual((uint)triangleIndex, (uint)TriangleCount);
        var i = triangleIndex * 3;
        v0 = _vertices[_indices[i]];
        v1 = _vertices[_indices[i + 1]];
        v2 = _vertices[_indices[i + 2]];
    }

    /// <summary>Axis-aligned bounds of triangle <paramref name="triangleIndex"/>.</summary>
    public AxisAlignedBox TriangleBounds(int triangleIndex)
    {
        GetTriangle(triangleIndex, out var v0, out var v1, out var v2);
        var box = AxisAlignedBox.FromMinMax(v0, v0);
        box = AxisAlignedBox.Expand(box, v1);
        return AxisAlignedBox.Expand(box, v2);
    }

    /// <summary>Axis-aligned bounds enclosing all vertices.</summary>
    public AxisAlignedBox GetAxisAlignedBox()
    {
        if (_vertices.Length == 0)
            return new AxisAlignedBox(Vector3.Zero, Vector3.Zero);

        var box = AxisAlignedBox.FromMinMax(_vertices[0], _vertices[0]);
        for (var i = 1; i < _vertices.Length; i++)
            box = AxisAlignedBox.Expand(box, _vertices[i]);
        return box;
    }

    /// <summary>Builds a binary BVH over this mesh (reuses internal vertex/index buffers).</summary>
    public TriangleBvh CreateBvh() => TriangleBvhBuilder.Build(_vertices, _indices);

    /// <summary>All triangles as <see cref="Face" /> values in world/index order.</summary>
    public IEnumerable<Face> GetFaces()
    {
        for (var i = 0; i < _indices.Length; i += 3)
        {
            var a = _vertices[_indices[i]];
            var b = _vertices[_indices[i + 1]];
            var c = _vertices[_indices[i + 2]];
            yield return new Face(a, b, c);
        }
    }
}
