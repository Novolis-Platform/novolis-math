using System.Numerics;

namespace Novolis.Math.Topology;

/// <summary>One vertical side of a <see cref="PlanarPrism"/> (footprint edge lofted by <see cref="Offset"/>).</summary>
public readonly record struct PlanarPrismSide(Vector3 Start, Vector3 End, Vector3 Offset);
