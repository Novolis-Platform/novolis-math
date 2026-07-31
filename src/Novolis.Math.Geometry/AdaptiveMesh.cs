using System.Numerics;

namespace Novolis.Math.Geometry;

/// <summary>Control sphere for an <see cref="AdaptiveMesh"/> (position + influence radius).</summary>
public readonly struct AdaptiveMeshHandle
{
    /// <summary>Creates a handle at <paramref name="position"/> with <paramref name="radius"/>.</summary>
    public AdaptiveMeshHandle(Vector3 position, float radius)
    {
        if (radius <= 0f)
            throw new ArgumentOutOfRangeException(nameof(radius), radius, "Radius must be positive.");
        Position = position;
        Radius = radius;
    }

    /// <summary>World-space center.</summary>
    public Vector3 Position { get; }

    /// <summary>Influence / surface radius (meters).</summary>
    public float Radius { get; }

    /// <summary>Returns a copy with a new position.</summary>
    public AdaptiveMeshHandle WithPosition(Vector3 position) => new(position, Radius);

    /// <summary>Returns a copy with a new radius.</summary>
    public AdaptiveMeshHandle WithRadius(float radius) => new(Position, radius);
}

/// <summary>How a vertex follows adaptive handles.</summary>
public enum AdaptiveVertexKind : byte
{
    /// <summary>Offset from a single handle (sphere / joint knob).</summary>
    Sphere = 0,

    /// <summary>Offset in the frame of a capsule between two handles.</summary>
    Capsule = 1,
}

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

/// <summary>
/// Low-level deformable surface bound to moving handles (spheres / capsule graph).
/// One mesh follows the control rig — used as a person hull over a ragdoll, soft props, etc.
/// </summary>
public sealed class AdaptiveMesh
{
    private readonly AdaptiveMeshHandle[] _bindHandles;
    private readonly AdaptiveVertexBinding[] _bindings;
    private readonly int[] _indices;
    private readonly Vector3[] _scratch;

    /// <summary>Creates an adaptive mesh from bind handles, per-vertex bindings, and triangle indices.</summary>
    public AdaptiveMesh(
        IReadOnlyList<AdaptiveMeshHandle> bindHandles,
        IReadOnlyList<AdaptiveVertexBinding> bindings,
        IReadOnlyList<int> indices)
    {
        ArgumentNullException.ThrowIfNull(bindHandles);
        ArgumentNullException.ThrowIfNull(bindings);
        ArgumentNullException.ThrowIfNull(indices);
        if (bindHandles.Count == 0)
            throw new ArgumentException("At least one handle is required.", nameof(bindHandles));
        if (indices.Count % 3 != 0)
            throw new ArgumentException("Index count must be a multiple of 3.", nameof(indices));

        _bindHandles = new AdaptiveMeshHandle[bindHandles.Count];
        for (var i = 0; i < bindHandles.Count; i++)
            _bindHandles[i] = bindHandles[i];

        _bindings = new AdaptiveVertexBinding[bindings.Count];
        for (var i = 0; i < bindings.Count; i++)
        {
            var b = bindings[i];
            ValidateBinding(b, _bindHandles.Length);
            _bindings[i] = b;
        }

        _indices = new int[indices.Count];
        for (var i = 0; i < indices.Count; i++)
        {
            var idx = indices[i];
            if ((uint)idx >= (uint)_bindings.Length)
                throw new ArgumentOutOfRangeException(nameof(indices), $"Index {idx} out of range.");
            _indices[i] = idx;
        }

        _scratch = new Vector3[_bindings.Length];
    }

    /// <summary>Bind-pose handles.</summary>
    public ReadOnlySpan<AdaptiveMeshHandle> BindHandles => _bindHandles;

    /// <summary>Vertex count.</summary>
    public int VertexCount => _bindings.Length;

    /// <summary>Triangle count.</summary>
    public int TriangleCount => _indices.Length / 3;

    /// <summary>Triangle indices (stable across adapts).</summary>
    public ReadOnlySpan<int> Indices => _indices;

    /// <summary>Writes deformed positions for the given handle centers (radii stay at bind).</summary>
    public void Adapt(ReadOnlySpan<Vector3> handlePositions, Span<Vector3> destination)
    {
        if (handlePositions.Length != _bindHandles.Length)
            throw new ArgumentException($"Expected {_bindHandles.Length} handle positions.", nameof(handlePositions));
        if (destination.Length < _bindings.Length)
            throw new ArgumentException("Destination too short.", nameof(destination));

        for (var i = 0; i < _bindings.Length; i++)
            destination[i] = Evaluate(_bindings[i], handlePositions);
    }

    /// <summary>Writes deformed positions using full handles (position + optional radius scale).</summary>
    public void Adapt(ReadOnlySpan<AdaptiveMeshHandle> handles, Span<Vector3> destination)
    {
        if (handles.Length != _bindHandles.Length)
            throw new ArgumentException($"Expected {_bindHandles.Length} handles.", nameof(handles));
        if (destination.Length < _bindings.Length)
            throw new ArgumentException("Destination too short.", nameof(destination));

        Span<Vector3> positions = stackalloc Vector3[handles.Length];
        Span<float> radiusScale = stackalloc float[handles.Length];
        for (var i = 0; i < handles.Length; i++)
        {
            positions[i] = handles[i].Position;
            radiusScale[i] = handles[i].Radius / _bindHandles[i].Radius;
        }

        for (var i = 0; i < _bindings.Length; i++)
            destination[i] = EvaluateScaled(_bindings[i], positions, radiusScale);
    }

    /// <summary>Returns a new <see cref="TriangleMesh"/> for the current handle layout.</summary>
    public TriangleMesh AdaptToMesh(ReadOnlySpan<Vector3> handlePositions)
    {
        Adapt(handlePositions, _scratch);
        return new TriangleMesh(_scratch, _indices);
    }

    /// <summary>Evaluates bind pose (identity adapt).</summary>
    public TriangleMesh BindMesh()
    {
        Span<Vector3> positions = stackalloc Vector3[_bindHandles.Length];
        for (var i = 0; i < _bindHandles.Length; i++)
            positions[i] = _bindHandles[i].Position;
        return AdaptToMesh(positions);
    }

    private Vector3 Evaluate(in AdaptiveVertexBinding binding, ReadOnlySpan<Vector3> positions)
    {
        Span<float> ones = stackalloc float[_bindHandles.Length];
        ones.Fill(1f);
        return EvaluateScaled(binding, positions, ones);
    }

    private Vector3 EvaluateScaled(
        in AdaptiveVertexBinding binding,
        ReadOnlySpan<Vector3> positions,
        ReadOnlySpan<float> radiusScale)
    {
        if (binding.Kind == AdaptiveVertexKind.Sphere)
        {
            var h = positions[binding.HandleA];
            var sphereScale = radiusScale[binding.HandleA];
            return h + binding.Radial * (_bindHandles[binding.HandleA].Radius * sphereScale);
        }

        var a = positions[binding.HandleA];
        var b = positions[binding.HandleB];
        var axis = b - a;
        var lenSq = axis.LengthSquared();
        Vector3 xAxis;
        if (lenSq < 1e-10f)
            xAxis = Vector3.UnitY;
        else
            xAxis = axis / MathF.Sqrt(lenSq);

        BuildFrame(xAxis, out var yAxis, out var zAxis);
        var scaleA = radiusScale[binding.HandleA];
        var scaleB = radiusScale[binding.HandleB];
        var blendScale = scaleA + (scaleB - scaleA) * binding.T;
        var center = Vector3.Lerp(a, b, binding.T);
        var radial = yAxis * binding.Radial.Y + zAxis * binding.Radial.Z;
        return center + radial * blendScale;
    }

    private static void BuildFrame(Vector3 xAxis, out Vector3 yAxis, out Vector3 zAxis)
    {
        var up = MathF.Abs(Vector3.Dot(xAxis, Vector3.UnitY)) > 0.92f ? Vector3.UnitZ : Vector3.UnitY;
        zAxis = Vector3.Normalize(Vector3.Cross(xAxis, up));
        yAxis = Vector3.Normalize(Vector3.Cross(zAxis, xAxis));
    }

    private static void ValidateBinding(in AdaptiveVertexBinding b, int handleCount)
    {
        if ((uint)b.HandleA >= (uint)handleCount)
            throw new ArgumentOutOfRangeException(nameof(b), "HandleA out of range.");
        if (b.Kind == AdaptiveVertexKind.Capsule)
        {
            if ((uint)b.HandleB >= (uint)handleCount)
                throw new ArgumentOutOfRangeException(nameof(b), "HandleB out of range.");
            if (b.HandleA == b.HandleB)
                throw new ArgumentException("Capsule binding requires distinct handles.");
        }
    }
}
