using System.Numerics;

namespace Novolis.Math.Geometry;

/// <summary>Transform, bounds, and intersection helpers for <see cref="Shape"/>.</summary>
public static class ShapeExtensions
{
    /// <summary>Applies <paramref name="transform"/> and returns a new shape.</summary>
    /// <param name="shape">Source shape.</param>
    /// <param name="transform">Transform to apply.</param>
    /// <returns>Transformed shape.</returns>
    public static Shape GetTransformedShape(this Shape shape, RigidTransform transform) => shape.Transform(transform);

    /// <summary>
    /// Axis-aligned bounds of <see cref="Polygon" /> and/or <see cref="Shape.TriangleMesh" /> (union when both).
    /// </summary>
    public static (Vector3 Min, Vector3 Max) GetAxisAlignedBoundingBox(this Shape shape)
    {
        var hasMesh = shape.TriangleMesh != null;
        var hasPoly = shape.Polygon.Length > 0;
        if (!hasMesh && !hasPoly)
            return (Vector3.Zero, Vector3.Zero);
        if (hasMesh && !hasPoly)
            return shape.TriangleMesh!.GetAxisAlignedBoundingBox();
        if (!hasMesh)
            return shape.Polygon.GetAxisAlignedBoundingBox();

        var (mMin, mMax) = shape.TriangleMesh!.GetAxisAlignedBoundingBox();
        var (pMin, pMax) = shape.Polygon.GetAxisAlignedBoundingBox();
        return (Vector3.Min(mMin, pMin), Vector3.Max(mMax, pMax));
    }

    /// <summary>Tests AABB overlap between two shapes (broad phase).</summary>
    /// <param name="a">First shape.</param>
    /// <param name="b">Second shape.</param>
    /// <returns><see langword="true"/> if axis-aligned boxes overlap.</returns>
    public static bool BoundingBoxesOverlap(this Shape a, Shape b)
    {
        var (aMin, aMax) = a.GetAxisAlignedBoundingBox();
        var (bMin, bMax) = b.GetAxisAlignedBoundingBox();
        return aMin.X <= bMax.X && aMax.X >= bMin.X
               && aMin.Y <= bMax.Y && aMax.Y >= bMin.Y
               && aMin.Z <= bMax.Z && aMax.Z >= bMin.Z;
    }

    /// <summary>Tests intersection using mesh AABB or polygon edge tests.</summary>
    /// <param name="shape">First shape.</param>
    /// <param name="otherShape">Second shape.</param>
    /// <returns><see langword="true"/> if shapes intersect.</returns>
    public static bool Intersect(this Shape shape, Shape otherShape)
    {
        if (shape.TriangleMesh != null || otherShape.TriangleMesh != null)
            return shape.BoundingBoxesOverlap(otherShape);

        return shape.Polygon.Intersect(otherShape.Polygon);
    }

    /// <summary>Returns intersection points or a coarse point when meshes use AABB overlap only.</summary>
    /// <param name="shape">First shape.</param>
    /// <param name="otherShape">Second shape.</param>
    /// <returns>Intersection points (may be approximate for mesh-only shapes).</returns>
    public static IEnumerable<Vector3> GetIntersectionPoints(this Shape shape, Shape otherShape)
    {
        if (shape.TriangleMesh != null || otherShape.TriangleMesh != null)
        {
            if (!shape.BoundingBoxesOverlap(otherShape))
                return [];

            var (aMin, aMax) = shape.GetAxisAlignedBoundingBox();
            var (bMin, bMax) = otherShape.GetAxisAlignedBoundingBox();
            var p = (aMin + aMax + bMin + bMax) * 0.25f;
            return [p];
        }

        return shape.Polygon.GetIntersectionPoints(otherShape.Polygon);
    }

    /// <summary>
    /// Applies scale (around the origin), rotation, then translation — typical for model vertices in local space.
    /// </summary>
    public static Shape Transform(this Shape shape, RigidTransform transform)
    {
        var result = shape.GetCopy();
        if (System.Math.Abs(transform.UniformScale - 1f) > 0.001f)
            result = result.Scale(transform.UniformScale);
        if (transform.Rotation != Quaternion.Identity)
            result = result.Rotate(transform.Rotation);
        result = result.Translate(transform.Position);
        return result;
    }

    /// <summary>Creates a deep copy of the shape.</summary>
    /// <param name="shape">Source shape.</param>
    /// <returns>Copied shape.</returns>
    public static Shape GetCopy(this Shape shape)
    {
        return new Shape
        {
            Polygon = shape.Polygon.GetCopy(),
            TriangleMesh = shape.TriangleMesh?.GetCopy(),
            Color = shape.Color
        };
    }

    /// <summary>Translates polygon and optional mesh.</summary>
    /// <param name="shape">Source shape.</param>
    /// <param name="position">Translation vector.</param>
    /// <returns>Translated shape.</returns>
    public static Shape Translate(this Shape shape, Vector3 position)
    {
        return new Shape
        {
            Polygon = shape.Polygon.Translate(position),
            TriangleMesh = shape.TriangleMesh?.Translate(position),
            Color = shape.Color
        };
    }

    /// <summary>Rotates polygon and optional mesh.</summary>
    /// <param name="shape">Source shape.</param>
    /// <param name="rotation">Rotation quaternion.</param>
    /// <returns>Rotated shape.</returns>
    public static Shape Rotate(this Shape shape, Quaternion rotation)
    {
        return new Shape
        {
            Polygon = shape.Polygon.Rotate(rotation),
            TriangleMesh = shape.TriangleMesh?.Rotate(rotation),
            Color = shape.Color
        };
    }

    /// <summary>Uniformly scales polygon and optional mesh.</summary>
    /// <param name="shape">Source shape.</param>
    /// <param name="scale">Scale factor.</param>
    /// <returns>Scaled shape.</returns>
    public static Shape Scale(this Shape shape, float scale)
    {
        return new Shape
        {
            Polygon = shape.Polygon.Scale(scale),
            TriangleMesh = shape.TriangleMesh?.Scale(scale),
            Color = shape.Color
        };
    }
}
