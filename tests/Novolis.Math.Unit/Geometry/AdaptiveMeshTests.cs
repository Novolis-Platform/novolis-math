using System.Numerics;
using Novolis.Math.Geometry;

namespace Novolis.Math.Unit.Geometry;

public class AdaptiveMeshTests
{
    [Test]
    public async Task FromCapsuleGraph_BindAdapt_IsIdentity()
    {
        var handles = new AdaptiveMeshHandle[]
        {
            new(new Vector3(0f, 0f, 0f), 0.1f),
            new(new Vector3(0f, 1f, 0f), 0.1f),
        };
        var mesh = AdaptiveMeshFactory.FromCapsuleGraph(handles, [(0, 1)], radialSegments: 4, ringsPerEdge: 2);
        var bind = mesh.BindMesh();
        var adapted = mesh.AdaptToMesh(new[] { handles[0].Position, handles[1].Position });

        await Assert.That(adapted.VertexCount).IsEqualTo(bind.VertexCount);
        await Assert.That(adapted.TriangleCount).IsEqualTo(bind.TriangleCount);
        for (var i = 0; i < bind.VertexCount; i++)
        {
            var d = Vector3.Distance(bind.Vertices[i], adapted.Vertices[i]);
            await Assert.That(d).IsLessThan(1e-4f);
        }
    }

    [Test]
    public async Task Adapt_MovesSurface_WithHandles()
    {
        var handles = new AdaptiveMeshHandle[]
        {
            new(Vector3.Zero, 0.1f),
            new(new Vector3(0f, 1f, 0f), 0.1f),
        };
        var mesh = AdaptiveMeshFactory.FromCapsuleGraph(handles, [(0, 1)], radialSegments: 4, ringsPerEdge: 1);
        var moved = mesh.AdaptToMesh(new[] { new Vector3(2f, 0f, 0f), new Vector3(2f, 1f, 0f) });
        var centroid = Vector3.Zero;
        foreach (var v in moved.Vertices)
            centroid += v;
        centroid /= moved.VertexCount;
        await Assert.That(centroid.X).IsEqualTo(2f).Within(0.15f);
    }
}
