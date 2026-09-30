using System.Numerics;

namespace Novolis.Math.Geometry;

/// <summary>How a vertex follows adaptive handles.</summary>
public enum AdaptiveVertexKind : byte
{
    /// <summary>Offset from a single handle (sphere / joint knob).</summary>
    Sphere = 0,

    /// <summary>Offset in the frame of a capsule between two handles.</summary>
    Capsule = 1,
}
