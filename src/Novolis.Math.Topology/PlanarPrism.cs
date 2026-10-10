using System.Numerics;

namespace Novolis.Math.Topology;

/// <summary>Footprint lofted along <see cref="Offset"/>: side quads and a lifted cap.</summary>
public sealed class PlanarPrism
{
    /// <summary>Creates a prism from an already-built footprint, cap, and sides.</summary>
    public PlanarPrism(Polygon footprint, Polygon cap, Vector3 offset, IReadOnlyList<PlanarPrismSide> sides)
    {
        Footprint = footprint;
        Cap = cap;
        Offset = offset;
        Sides = sides;
    }

    /// <summary>Original planar outline.</summary>
    public Polygon Footprint { get; }

    /// <summary><see cref="Footprint"/> vertices plus <see cref="Offset"/>.</summary>
    public Polygon Cap { get; }

    /// <summary>Loft vector (typically a 2.5D height direction).</summary>
    public Vector3 Offset { get; }

    /// <summary>One side per footprint edge.</summary>
    public IReadOnlyList<PlanarPrismSide> Sides { get; }
}
