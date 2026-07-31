using System.Numerics;

namespace Novolis.Math.Geometry;

/// <summary>Builds an <see cref="AdaptiveMesh"/> from a capsule graph (handles + edges).</summary>
public static class AdaptiveMeshFactory
{
    /// <summary>
    /// Creates a single connected person/prop hull: sphere caps at handles and tubes along edges.
    /// </summary>
    /// <param name="handles">Bind-pose control spheres.</param>
    /// <param name="edges">Undirected capsule edges (handle index pairs).</param>
    /// <param name="radialSegments">Vertices around each ring (≥3).</param>
    /// <param name="ringsPerEdge">Interior rings per edge (≥1).</param>
    public static AdaptiveMesh FromCapsuleGraph(
        ReadOnlySpan<AdaptiveMeshHandle> handles,
        ReadOnlySpan<(int A, int B)> edges,
        int radialSegments = 6,
        int ringsPerEdge = 3)
    {
        if (handles.Length == 0)
            throw new ArgumentException("At least one handle is required.", nameof(handles));
        if (radialSegments < 3)
            throw new ArgumentOutOfRangeException(nameof(radialSegments));
        if (ringsPerEdge < 1)
            throw new ArgumentOutOfRangeException(nameof(ringsPerEdge));

        var bindList = new AdaptiveMeshHandle[handles.Length];
        handles.CopyTo(bindList);

        var bindings = new List<AdaptiveVertexBinding>(handles.Length * radialSegments + edges.Length * ringsPerEdge * radialSegments);
        var indices = new List<int>(bindings.Capacity * 3);

        // Sphere knobs at each handle.
        for (var h = 0; h < handles.Length; h++)
        {
            var start = bindings.Count;
            AddSphere(bindings, h, radialSegments);
            StitchSphereFan(indices, start, radialSegments);
        }

        // Tubes along edges.
        foreach (var (a, b) in edges)
        {
            if ((uint)a >= (uint)handles.Length || (uint)b >= (uint)handles.Length)
                throw new ArgumentOutOfRangeException(nameof(edges), "Edge handle index out of range.");
            if (a == b)
                continue;

            var radiusA = handles[a].Radius;
            var radiusB = handles[b].Radius;
            var ringStarts = new int[ringsPerEdge + 2];
            for (var r = 0; r <= ringsPerEdge + 1; r++)
            {
                var t = r / (float)(ringsPerEdge + 1);
                var radius = radiusA + (radiusB - radiusA) * t;
                ringStarts[r] = bindings.Count;
                AddRing(bindings, a, b, t, radius, radialSegments);
            }

            for (var r = 0; r < ringStarts.Length - 1; r++)
                StitchRingStrip(indices, ringStarts[r], ringStarts[r + 1], radialSegments);
        }

        return new AdaptiveMesh(bindList, bindings, indices);
    }

    private static void AddSphere(List<AdaptiveVertexBinding> bindings, int handle, int segments)
    {
        // Latitude bands: poles + mid belt (simple low-poly sphere).
        bindings.Add(AdaptiveVertexBinding.ForSphere(handle, Vector3.UnitY));
        bindings.Add(AdaptiveVertexBinding.ForSphere(handle, -Vector3.UnitY));
        for (var i = 0; i < segments; i++)
        {
            var ang = i * (MathF.Tau / segments);
            var dir = Vector3.Normalize(new Vector3(MathF.Cos(ang), 0.15f, MathF.Sin(ang)));
            bindings.Add(AdaptiveVertexBinding.ForSphere(handle, dir));
            var dirLo = Vector3.Normalize(new Vector3(MathF.Cos(ang), -0.15f, MathF.Sin(ang)));
            bindings.Add(AdaptiveVertexBinding.ForSphere(handle, dirLo));
        }
    }

    private static void StitchSphereFan(List<int> indices, int start, int segments)
    {
        var top = start;
        var bottom = start + 1;
        var belt = start + 2;
        for (var i = 0; i < segments; i++)
        {
            var i0 = belt + i * 2;
            var i1 = belt + ((i + 1) % segments) * 2;
            var j0 = i0 + 1;
            var j1 = i1 + 1;
            indices.Add(top);
            indices.Add(i0);
            indices.Add(i1);
            indices.Add(bottom);
            indices.Add(j1);
            indices.Add(j0);
            indices.Add(i0);
            indices.Add(j0);
            indices.Add(j1);
            indices.Add(i0);
            indices.Add(j1);
            indices.Add(i1);
        }
    }

    private static void AddRing(
        List<AdaptiveVertexBinding> bindings,
        int handleA,
        int handleB,
        float t,
        float radius,
        int segments)
    {
        for (var i = 0; i < segments; i++)
        {
            var ang = i * (MathF.Tau / segments);
            var y = MathF.Cos(ang) * radius;
            var z = MathF.Sin(ang) * radius;
            bindings.Add(AdaptiveVertexBinding.ForCapsule(handleA, handleB, t, y, z));
        }
    }

    private static void StitchRingStrip(List<int> indices, int ringA, int ringB, int segments)
    {
        for (var i = 0; i < segments; i++)
        {
            var a0 = ringA + i;
            var a1 = ringA + (i + 1) % segments;
            var b0 = ringB + i;
            var b1 = ringB + (i + 1) % segments;
            indices.Add(a0);
            indices.Add(b0);
            indices.Add(b1);
            indices.Add(a0);
            indices.Add(b1);
            indices.Add(a1);
        }
    }
}
