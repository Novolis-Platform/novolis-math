using System.Numerics;

namespace Novolis.Math.Topology;

/// <summary>Lofts a planar polygon along a vector.</summary>
public static class PolygonExtrude
{
    /// <summary>Builds side quads and a lifted cap. Empty footprints yield empty sides.</summary>
    public static PlanarPrism Extrude(this Polygon footprint, Vector3 offset)
    {
        ArgumentNullException.ThrowIfNull(footprint);
        var n = footprint.Length;
        if (n == 0)
        {
            return new PlanarPrism(footprint, new Polygon(Array.Empty<Vector3>()), offset, []);
        }

        var lifted = new Vector3[n];
        var sides = new PlanarPrismSide[n];
        for (var i = 0; i < n; i++)
        {
            var a = footprint[i];
            var b = footprint[(i + 1) % n];
            lifted[i] = a + offset;
            sides[i] = new PlanarPrismSide(a, b, offset);
        }

        return new PlanarPrism(footprint, new Polygon(lifted), offset, sides);
    }
}
