using System.Numerics;

namespace Novolis.Math.Geometry;

public sealed record BridgeOptions(
    int Segments = 1,
    float Twist = 0f,
    bool ReverseSecondLoop = false);

/// <summary>Connect two equal-count boundary loops with triangle strips.</summary>
public static class MeshBridge
{
    public static EditableMesh Apply(
        EditableMesh mesh,
        IReadOnlyList<int> loopA,
        IReadOnlyList<int> loopB,
        BridgeOptions? options = null)
    {
        ArgumentNullException.ThrowIfNull(mesh);
        ArgumentNullException.ThrowIfNull(loopA);
        ArgumentNullException.ThrowIfNull(loopB);
        options ??= new BridgeOptions();

        if (loopA.Count < 3 || loopB.Count < 3)
            throw new ArgumentException("Loops must have at least 3 vertices.");
        if (loopA.Count != loopB.Count)
            throw new ArgumentException("Bridge v1 requires equal vertex counts on both loops.");

        var b = loopB.ToList();
        if (options.ReverseSecondLoop)
            b.Reverse();

        var n = loopA.Count;
        var twist = ((int)MathF.Round(options.Twist) % n + n) % n;
        if (twist != 0)
        {
            var rotated = new int[n];
            for (var i = 0; i < n; i++)
                rotated[i] = b[(i + twist) % n];
            b = [.. rotated];
        }

        var work = mesh.Clone();
        var segments = System.Math.Max(1, options.Segments);

        if (segments == 1)
        {
            for (var i = 0; i < n; i++)
            {
                var a0 = loopA[i];
                var a1 = loopA[(i + 1) % n];
                var b0 = b[i];
                var b1 = b[(i + 1) % n];
                work.AddTriangle(a0, b0, a1);
                work.AddTriangle(a1, b0, b1);
            }

            return work;
        }

        // Multi-segment: insert intermediate rings
        var rings = new int[segments + 1][];
        rings[0] = loopA.ToArray();
        rings[segments] = b.ToArray();
        for (var s = 1; s < segments; s++)
        {
            var t = s / (float)segments;
            rings[s] = new int[n];
            for (var i = 0; i < n; i++)
            {
                var p = Vector3.Lerp(work.Vertices[loopA[i]], work.Vertices[b[i]], t);
                rings[s][i] = work.AddVertex(p);
            }
        }

        for (var s = 0; s < segments; s++)
        {
            for (var i = 0; i < n; i++)
            {
                var a0 = rings[s][i];
                var a1 = rings[s][(i + 1) % n];
                var b0 = rings[s + 1][i];
                var b1 = rings[s + 1][(i + 1) % n];
                work.AddTriangle(a0, b0, a1);
                work.AddTriangle(a1, b0, b1);
            }
        }

        return work;
    }
}
