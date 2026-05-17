using System.Numerics;
using Novolis.Math.Arrays;

namespace Novolis.Math.Geometry;

/// <summary>
/// Circle-vs-grid collision on the XZ plane. Cell value <c>0</c> is walkable; any other value blocks.
/// </summary>
public static class GridCollision2D
{
    /// <summary>
    /// Moves <paramref name="position"/> by <paramref name="delta"/> (XZ), sliding along walls.
    /// </summary>
    public static Vector2 TryMove(
        DenseGrid<byte> map,
        Vector2 position,
        Vector2 delta,
        float radius,
        float cellSize = 1f)
    {
        if (delta.LengthSquared() < 1e-12f)
            return position;

        var next = position + new Vector2(delta.X, 0f);
        if (!OverlapsWall(map, next, radius, cellSize))
            position = next;

        next = position + new Vector2(0f, delta.Y);
        if (!OverlapsWall(map, next, radius, cellSize))
            position = next;

        return position;
    }

    /// <summary>Returns true when the circle at <paramref name="position"/> overlaps a blocked cell.</summary>
    public static bool OverlapsWall(DenseGrid<byte> map, Vector2 position, float radius, float cellSize = 1f)
    {
        var minX = (int)MathF.Floor((position.X - radius) / cellSize);
        var maxX = (int)MathF.Floor((position.X + radius) / cellSize);
        var minZ = (int)MathF.Floor((position.Y - radius) / cellSize);
        var maxZ = (int)MathF.Floor((position.Y + radius) / cellSize);

        for (var z = minZ; z <= maxZ; z++)
        for (var x = minX; x <= maxX; x++)
        {
            if (x < 0 || z < 0 || x >= map.Width || z >= map.Height)
                return true;

            if (!IsBlocked(map, (uint)x, (uint)z))
                continue;

            var cellMinX = x * cellSize;
            var cellMinZ = z * cellSize;
            var cellMaxX = cellMinX + cellSize;
            var cellMaxZ = cellMinZ + cellSize;

            var closestX = System.Math.Clamp(position.X, cellMinX, cellMaxX);
            var closestZ = System.Math.Clamp(position.Y, cellMinZ, cellMaxZ);
            var dx = position.X - closestX;
            var dz = position.Y - closestZ;
            if (dx * dx + dz * dz < radius * radius)
                return true;
        }

        return false;
    }

    private static bool IsBlocked(DenseGrid<byte> map, uint x, uint z)
    {
        return map.Get(x, z) != 0;
    }
}
