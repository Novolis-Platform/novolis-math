using System.Globalization;
using System.Text;

namespace Novolis.Math.Geometry;

/// <summary>Writes triangle meshes as Wavefront OBJ (ASCII, 1-based faces).</summary>
public static class WavefrontObjWriter
{
    /// <summary>Writes a single object to <paramref name="path"/>.</summary>
    public static void Write(string path, EditableMesh mesh, string objectName = "mesh") =>
        Write(path, [(Sanitize(objectName), mesh)]);

    /// <summary>Writes a single object to <paramref name="path"/>.</summary>
    public static void Write(string path, TriangleMesh mesh, string objectName = "mesh") =>
        Write(path, EditableMesh.FromTriangleMesh(mesh), objectName);

    /// <summary>Writes multiple named objects (groups) into one OBJ file.</summary>
    public static void Write(string path, IEnumerable<(string Name, EditableMesh Mesh)> parts)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        ArgumentNullException.ThrowIfNull(parts);

        var list = parts as IList<(string Name, EditableMesh Mesh)> ?? parts.ToList();
        var capacity = 256;
        foreach (var (_, mesh) in list)
        {
            ArgumentNullException.ThrowIfNull(mesh);
            capacity += mesh.VertexCount * 32 + mesh.TriangleCount * 24;
        }

        var sb = new StringBuilder(capacity);
        sb.AppendLine("# Novolis.Math.Geometry WavefrontObjWriter");
        var vertexBase = 1;
        foreach (var (name, mesh) in list)
        {
            if (mesh.VertexCount == 0 || mesh.TriangleCount == 0)
                continue;

            sb.Append("o ").Append(Sanitize(name)).AppendLine();
            foreach (var v in mesh.Vertices)
            {
                sb.Append("v ")
                    .Append(F(v.X)).Append(' ')
                    .Append(F(v.Y)).Append(' ')
                    .Append(F(v.Z)).AppendLine();
            }

            var idx = mesh.Indices;
            for (var i = 0; i + 2 < idx.Count; i += 3)
            {
                sb.Append("f ")
                    .Append(vertexBase + idx[i]).Append(' ')
                    .Append(vertexBase + idx[i + 1]).Append(' ')
                    .Append(vertexBase + idx[i + 2]).AppendLine();
            }

            vertexBase += mesh.VertexCount;
        }

        Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(path))!);
        File.WriteAllText(path, sb.ToString());
    }

    private static string F(float value) => value.ToString("G9", CultureInfo.InvariantCulture);

    private static string Sanitize(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return "mesh";
        var chars = name.Select(c => char.IsLetterOrDigit(c) || c is '_' or '-' ? c : '_').ToArray();
        return new string(chars);
    }
}
