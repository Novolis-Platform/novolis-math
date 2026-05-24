using System.Numerics;

namespace Novolis.Math.Geometry;

/// <summary>Transform and bounds helpers for <see cref="TriangleMesh"/>.</summary>
public static class TriangleMeshExtensions
{
    /// <summary>Creates a deep copy of the mesh.</summary>
    /// <param name="mesh">Source mesh.</param>
    /// <returns>New mesh with copied buffers.</returns>
    public static TriangleMesh GetCopy(this TriangleMesh mesh)
    {
        var v = mesh.Vertices.ToArray();
        var ix = mesh.Indices.ToArray();
        return new TriangleMesh(v, ix);
    }

    /// <summary>Translates all vertices by <paramref name="delta"/>.</summary>
    /// <param name="mesh">Source mesh.</param>
    /// <param name="delta">Translation vector.</param>
    /// <returns>New mesh with translated vertices.</returns>
    public static TriangleMesh Translate(this TriangleMesh mesh, Vector3 delta)
    {
        var v = mesh.Vertices.ToArray();
        for (var i = 0; i < v.Length; i++)
            v[i] += delta;

        return new TriangleMesh(v, mesh.Indices.ToArray());
    }

    /// <summary>Rotates all vertices by <paramref name="rotation"/>.</summary>
    /// <param name="mesh">Source mesh.</param>
    /// <param name="rotation">World rotation.</param>
    /// <returns>New mesh with rotated vertices.</returns>
    public static TriangleMesh Rotate(this TriangleMesh mesh, Quaternion rotation)
    {
        var v = mesh.Vertices.ToArray();
        for (var i = 0; i < v.Length; i++)
            v[i] = Vector3.Transform(v[i], rotation);

        return new TriangleMesh(v, mesh.Indices.ToArray());
    }

    /// <summary>Uniformly scales all vertices.</summary>
    /// <param name="mesh">Source mesh.</param>
    /// <param name="uniformScale">Scale factor.</param>
    /// <returns>New mesh with scaled vertices.</returns>
    public static TriangleMesh Scale(this TriangleMesh mesh, float uniformScale)
    {
        var v = mesh.Vertices.ToArray();
        for (var i = 0; i < v.Length; i++)
            v[i] *= uniformScale;

        return new TriangleMesh(v, mesh.Indices.ToArray());
    }

    /// <summary>Computes an axis-aligned bounding box over all vertices.</summary>
    /// <param name="mesh">Source mesh.</param>
    /// <returns>Minimum and maximum corners.</returns>
    public static (Vector3 Min, Vector3 Max) GetAxisAlignedBoundingBox(this TriangleMesh mesh)
    {
        if (mesh.VertexCount == 0)
            return (Vector3.Zero, Vector3.Zero);

        var min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
        var max = new Vector3(float.MinValue, float.MinValue, float.MinValue);
        foreach (var vertex in mesh.Vertices)
        {
            min = Vector3.Min(min, vertex);
            max = Vector3.Max(max, vertex);
        }

        return (min, max);
    }
}
