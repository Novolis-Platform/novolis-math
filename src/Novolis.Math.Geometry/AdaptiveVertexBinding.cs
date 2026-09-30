using System.Numerics;

namespace Novolis.Math.Geometry;

/// <summary>Bind-time attachment of one mesh vertex to adaptive handles.</summary>
public readonly struct AdaptiveVertexBinding
{
    /// <summary>Sphere skin: one handle + unit direction in bind space.</summary>
    public static AdaptiveVertexBinding ForSphere(int handle, Vector3 bindUnitDirection) =>
        new(AdaptiveVertexKind.Sphere, handle, -1, 0f, Vector3.Normalize(bindUnitDirection), 0f);

    /// <summary>
    /// Capsule skin: point at fraction <paramref name="t"/> along A→B with radial offsets in the bind capsule frame
    /// (X along bone, Y/Z radial).
    /// </summary>
    public static AdaptiveVertexBinding ForCapsule(int handleA, int handleB, float t, float radialY, float radialZ) =>
        new(AdaptiveVertexKind.Capsule, handleA, handleB, t, new Vector3(0f, radialY, radialZ), 0f);

    private AdaptiveVertexBinding(
        AdaptiveVertexKind kind,
        int handleA,
        int handleB,
        float t,
        Vector3 radial,
        float unused)
    {
        _ = unused;
        Kind = kind;
        HandleA = handleA;
        HandleB = handleB;
        T = t;
        Radial = radial;
    }

    /// <summary>Binding kind.</summary>
    public AdaptiveVertexKind Kind { get; }

    /// <summary>Primary handle (sphere) or capsule start.</summary>
    public int HandleA { get; }

    /// <summary>Capsule end, or -1 for sphere.</summary>
    public int HandleB { get; }

    /// <summary>Fraction along capsule A→B (0..1).</summary>
    public float T { get; }

    /// <summary>
    /// Sphere: unit bind direction. Capsule: (0, radialY, radialZ) in bind capsule frame (meters at bind radius scale).
    /// </summary>
    public Vector3 Radial { get; }
}
