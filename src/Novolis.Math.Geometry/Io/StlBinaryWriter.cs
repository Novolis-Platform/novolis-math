using System.Buffers.Binary;
using System.Numerics;
using System.Text;

namespace Novolis.Math.Geometry;

/// <summary>Writes a triangle mesh as binary STL (little-endian).</summary>
public static class StlBinaryWriter
{
    /// <summary>Writes <paramref name="mesh"/> to <paramref name="path"/>.</summary>
    public static void Write(string path, EditableMesh mesh, string solidName = "mesh")
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        ArgumentNullException.ThrowIfNull(mesh);
        Write(path, mesh.ToTriangleMesh(), solidName);
    }

    /// <summary>Writes <paramref name="mesh"/> to <paramref name="path"/>.</summary>
    public static void Write(string path, TriangleMesh mesh, string solidName = "mesh")
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        ArgumentNullException.ThrowIfNull(mesh);

        var triCount = mesh.Indices.Length / 3;
        var bytes = new byte[84 + checked(triCount * 50)];
        var header = Encoding.ASCII.GetBytes(solidName.Length > 80 ? solidName[..80] : solidName);
        header.CopyTo(bytes, 0);
        BinaryPrimitives.WriteUInt32LittleEndian(bytes.AsSpan(80, 4), (uint)triCount);

        var verts = mesh.Vertices;
        var idx = mesh.Indices;
        var offset = 84;
        for (var t = 0; t < triCount; t++)
        {
            var i = t * 3;
            var a = verts[idx[i]];
            var b = verts[idx[i + 1]];
            var c = verts[idx[i + 2]];
            var n = Vector3.Normalize(Vector3.Cross(b - a, c - a));
            if (!float.IsFinite(n.X))
                n = Vector3.UnitY;

            WriteVec(bytes.AsSpan(offset, 12), n);
            WriteVec(bytes.AsSpan(offset + 12, 12), a);
            WriteVec(bytes.AsSpan(offset + 24, 12), b);
            WriteVec(bytes.AsSpan(offset + 36, 12), c);
            BinaryPrimitives.WriteUInt16LittleEndian(bytes.AsSpan(offset + 48, 2), 0);
            offset += 50;
        }

        Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(path))!);
        File.WriteAllBytes(path, bytes);
    }

    private static void WriteVec(Span<byte> dest, Vector3 v)
    {
        BinaryPrimitives.WriteSingleLittleEndian(dest[..4], v.X);
        BinaryPrimitives.WriteSingleLittleEndian(dest.Slice(4, 4), v.Y);
        BinaryPrimitives.WriteSingleLittleEndian(dest.Slice(8, 4), v.Z);
    }
}
