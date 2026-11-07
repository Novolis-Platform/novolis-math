using System.Numerics;
using Novolis.Math.Geometry;
using TUnit.Core;

namespace Novolis.Math.Geometry.Tests;

public class MeshIoWriterTests
{
    [Test]
    public async Task WavefrontObj_writes_vertices_and_faces()
    {
        var mesh = new EditableMesh(
            [Vector3.Zero, Vector3.UnitX, Vector3.UnitY],
            [0, 1, 2]);
        var path = Path.Combine(Path.GetTempPath(), "novolis-obj-" + Guid.NewGuid().ToString("N") + ".obj");
        try
        {
            WavefrontObjWriter.Write(path, mesh, "tri");
            var text = await File.ReadAllTextAsync(path);
            await Assert.That(text).Contains("o tri");
            await Assert.That(text).Contains("v 0");
            await Assert.That(text).Contains("f 1 2 3");
        }
        finally
        {
            try { File.Delete(path); } catch { /* ignore */ }
        }
    }

    [Test]
    public async Task StlBinary_writes_header_and_triangle_count()
    {
        var mesh = new EditableMesh(
            [Vector3.Zero, Vector3.UnitX, Vector3.UnitY],
            [0, 1, 2]);
        var path = Path.Combine(Path.GetTempPath(), "novolis-stl-" + Guid.NewGuid().ToString("N") + ".stl");
        try
        {
            StlBinaryWriter.Write(path, mesh, "tri");
            var bytes = await File.ReadAllBytesAsync(path);
            await Assert.That(bytes.Length).IsEqualTo(84 + 50);
            var count = BitConverter.ToUInt32(bytes, 80);
            await Assert.That(count).IsEqualTo(1u);
        }
        finally
        {
            try { File.Delete(path); } catch { /* ignore */ }
        }
    }
}
