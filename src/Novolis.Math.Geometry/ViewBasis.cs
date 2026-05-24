using System.Numerics;

namespace Novolis.Math.Geometry;

/// <summary>Orthonormal view axes for ray generation (no FOV policy or observer lifecycle).</summary>
public readonly struct ViewBasis(Vector3 forward, Vector3 right, Vector3 up)
{
    /// <summary>Normalized view direction.</summary>
    public Vector3 Forward { get; } = forward;

    /// <summary>Normalized camera right axis.</summary>
    public Vector3 Right { get; } = right;

    /// <summary>Normalized camera up axis.</summary>
    public Vector3 Up { get; } = up;

    /// <summary>Builds an orthonormal basis from eye, look target, and up hint.</summary>
    /// <param name="eye">Eye position.</param>
    /// <param name="target">Look-at target.</param>
    /// <param name="upHint">World up hint (re-orthogonalized).</param>
    /// <returns>Orthonormal <see cref="ViewBasis"/>.</returns>
    public static ViewBasis FromLookAt(Vector3 eye, Vector3 target, Vector3 upHint)
    {
        var forward = target - eye;
        if (forward.LengthSquared() < 1e-12f)
        {
            forward = -Vector3.UnitZ;
        }

        forward = Vector3.Normalize(forward);
        var right = Vector3.Normalize(Vector3.Cross(forward, upHint));
        var up = Vector3.Normalize(Vector3.Cross(right, forward));
        return new ViewBasis(forward, right, up);
    }

    /// <summary>
    /// Primary ray direction for normalized device coordinates <paramref name="u"/> and <paramref name="v"/>
    /// in [-1, 1] with <paramref name="tanHalfFov"/> and <paramref name="aspect"/> (width / height).
    /// </summary>
    /// <param name="basis">View axes.</param>
    /// <param name="u">Horizontal NDC sample.</param>
    /// <param name="v">Vertical NDC sample.</param>
    /// <param name="tanHalfFov">Tangent of half vertical field of view.</param>
    /// <param name="aspect">Framebuffer width divided by height.</param>
    /// <returns>Normalized world-space ray direction.</returns>
    public static Vector3 PrimaryRayDirection(in ViewBasis basis, float u, float v, float tanHalfFov, float aspect)
    {
        var dir = basis.Forward + u * tanHalfFov * aspect * basis.Right + v * tanHalfFov * basis.Up;
        return Vector3.Normalize(dir);
    }
}
