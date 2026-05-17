using System.Numerics;

namespace Novolis.Math.Geometry;

public static partial class PolygonFactory
{
    public static Polygon CreateLine(Vector3 start, Vector3 end) => new(new[] { start, end });

    public static Polygon CreateCircle(int sides, float radius, Vector3 center)
    {
        var points = new List<Vector3>();
        var angle = 360f / sides;
        for (var i = 0; i < sides; i++)
        {
            var x = radius * MathF.Cos(DegreesToRadians(angle * i));
            var y = radius * MathF.Sin(DegreesToRadians(angle * i));
            points.Add(new Vector3(x, y, 0) + center);
        }

        return new Polygon(points);
    }

    public static Polygon CreateRectangle(float width, float height, Vector3 center)
    {
        var points = new List<Vector3>
        {
            new Vector3(-width / 2, -height / 2, 0) + center,
            new Vector3(width / 2, -height / 2, 0) + center,
            new Vector3(width / 2, height / 2, 0) + center,
            new Vector3(-width / 2, height / 2, 0) + center
        };
        return new Polygon(points);
    }

    public static Polygon CreateRectangle(float width, float height)
    {
        var polygon = new Vector3[4];
        polygon[0] = new Vector3(0, 0, 0);
        polygon[1] = new Vector3(width, 0, 0);
        polygon[2] = new Vector3(width, height, 0);
        polygon[3] = new Vector3(0, height, 0);
        return new Polygon(polygon);
    }

    public static Polygon CreateCircle(float radius, int sides)
    {
        var polygon = new Vector3[sides];
        var step = (float)(System.Math.PI * 2 / sides);
        for (var i = 0; i < sides; i++)
        {
            var angle = step * i;
            polygon[i] = new Vector3((float)System.Math.Cos(angle) * radius, (float)System.Math.Sin(angle) * radius, 0);
        }

        return new Polygon(polygon);
    }

    public static Polygon CreateTriangle(float width, float height)
    {
        var polygon = new Vector3[3];
        polygon[0] = new Vector3(0, 0, 0);
        polygon[1] = new Vector3(width, 0, 0);
        polygon[2] = new Vector3(width / 2, height, 0);
        return new Polygon(polygon);
    }

    public static Polygon CreateHexagon(float radius)
    {
        var polygon = new Vector3[6];
        const float step = (float)(System.Math.PI * 2 / 6);
        for (var i = 0; i < 6; i++)
        {
            var angle = step * i;
            polygon[i] = new Vector3((float)System.Math.Cos(angle) * radius, (float)System.Math.Sin(angle) * radius, 0);
        }

        return new Polygon(polygon);
    }

    private static float DegreesToRadians(float degrees) => degrees * MathF.PI / 180f;
}
