namespace Novolis.Math.Geometry;

/// <summary>
/// Lon/lat ↔ planar map meters and optional E/W raster column policy.
/// World X increases east; world Z increases north. Never flip lon→world;
/// only <see cref="WorldToDataU"/> may mirror when column 0 is geographic east.
/// </summary>
public static class MapRasterBasis
{
    /// <summary>Lon → world X (east-positive). Never mirrors.</summary>
    public static float LonToWorldX(double longitudeDegrees, double westLon, double eastLon, float eastMeters) =>
        (float)((longitudeDegrees - westLon) / (eastLon - westLon) * eastMeters);

    /// <summary>Lat → world Z (north-positive). Never mirrors.</summary>
    public static float LatToWorldZ(double latitudeDegrees, double southLat, double northLat, float northMeters) =>
        (float)((latitudeDegrees - southLat) / (northLat - southLat) * northMeters);

    /// <summary>
    /// World X → unit raster U in [0,1] (0 = left column).
    /// Set <paramref name="rasterColumn0IsEast"/> when the raster is mirrored.
    /// </summary>
    public static float WorldToDataU(float worldXMeters, float eastMeters, bool rasterColumn0IsEast)
    {
        var u = worldXMeters / eastMeters;
        return rasterColumn0IsEast ? 1f - u : u;
    }

    /// <summary>World X → data-space meters along the raster (left = 0).</summary>
    public static float WorldToDataX(float worldXMeters, float eastMeters, bool rasterColumn0IsEast) =>
        WorldToDataU(worldXMeters, eastMeters, rasterColumn0IsEast) * eastMeters;
}
