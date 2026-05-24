using System.Numerics;
using Novolis.Math.Geometry;
using TUnit.Core;

namespace Novolis.Math.Geometry.Tests;

public class MeshTransformTests
{
    [Test]
    public async Task Polygon_Translate_DoesNotMutateOriginalVertices()
    {
        var original = PolygonFactory.CreateRectangle(2f, 2f, Vector3.Zero);
        var v0Before = original[0];

        _ = original.Translate(new Vector3(10f, 0f, 0f));

        await Assert.That(original[0]).IsEqualTo(v0Before);
    }

    [Test]
    public async Task Polygon_EdgeCount_EqualsVertexCount_ForClosedLoop()
    {
        var cube = PolygonFactory.CreateCube(1f);

        await Assert.That(cube.Edges.Count()).IsEqualTo(cube.Length);
    }

    [Test]
    public async Task Shape_Transform_AppliesScaleRotationThenTranslation()
    {
        var rect = PolygonFactory.CreateRectangle(2f, 2f, Vector3.Zero);
        var shape = new Shape { Polygon = rect, Color = Rgba32.White };
        var transform = RigidTransform.FromPose(new Vector3(0f, 5f, 0f), Quaternion.Identity);

        var world = shape.Transform(transform);

        await Assert.That(world.Polygon[0].Y).IsEqualTo(rect[0].Y + 5f).Within(0.001f);
    }

    [Test]
    public async Task Shape_GetTransformedShape_UsesFullTransform()
    {
        var rect = PolygonFactory.CreateRectangle(1f, 1f, Vector3.Zero);
        var shape = new Shape { Polygon = rect, Color = Rgba32.Red };
        var transform = RigidTransform.FromPosition(new Vector3(100f, 0f, 0f));

        var world = shape.GetTransformedShape(transform);

        await Assert.That(world.Polygon.GetCenter().X).IsEqualTo(100f).Within(1f);
    }

    [Test]
    public async Task Polygon_GetAxisAlignedBoundingBox_ContainsAllVertices()
    {
        var rect = PolygonFactory.CreateRectangle(4f, 2f, new Vector3(10f, 20f, 0f));

        var (min, max) = rect.GetAxisAlignedBoundingBox();

        foreach (var v in rect)
        {
            await Assert.That(v.X).IsGreaterThanOrEqualTo(min.X).And.IsLessThanOrEqualTo(max.X);
            await Assert.That(v.Y).IsGreaterThanOrEqualTo(min.Y).And.IsLessThanOrEqualTo(max.Y);
            await Assert.That(v.Z).IsGreaterThanOrEqualTo(min.Z).And.IsLessThanOrEqualTo(max.Z);
        }
    }

}
