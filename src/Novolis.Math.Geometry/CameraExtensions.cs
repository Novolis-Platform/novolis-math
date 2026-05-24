using System.Numerics;

namespace Novolis.Math.Geometry;

/// <summary>Obsolete matrix and movement helpers for <see cref="Camera"/>.</summary>
[Obsolete("Use Novolis.Simulation.View.Camera and CameraExtensions.")]
public static class CameraExtensions
{
    /// <summary>Builds a perspective projection matrix from the camera fields.</summary>
    /// <param name="camera">Source camera.</param>
    /// <returns>Projection matrix.</returns>
    public static Matrix4x4 GetProjectionMatrix(this Camera camera)
    {
        var fovRadians = camera.FieldOfView * (MathF.PI / 180f);
        return Matrix4x4.CreatePerspectiveFieldOfView(
            fovRadians,
            camera.AspectRatio,
            camera.NearPlaneDistance,
            camera.FarPlaneDistance);
    }

    /// <summary>Builds a view matrix from position, target, and up.</summary>
    /// <param name="camera">Source camera.</param>
    /// <returns>View matrix.</returns>
    public static Matrix4x4 GetViewMatrix(this Camera camera) =>
        Matrix4x4.CreateLookAt(camera.Position, camera.Target, camera.Up);

    /// <summary>Moves position and target along the view forward axis.</summary>
    /// <param name="camera">Camera to move.</param>
    /// <param name="speed">Distance in world units.</param>
    public static void MoveForward(this Camera camera, float speed)
    {
        var direction = Vector3.Normalize(camera.Target - camera.Position);
        camera.Position += direction * speed;
        camera.Target += direction * speed;
    }

    /// <summary>Moves position and target opposite the view forward axis.</summary>
    /// <param name="camera">Camera to move.</param>
    /// <param name="speed">Distance in world units.</param>
    public static void MoveBackward(this Camera camera, float speed)
    {
        var direction = Vector3.Normalize(camera.Target - camera.Position);
        camera.Position -= direction * speed;
        camera.Target -= direction * speed;
    }

    /// <summary>Strafes position and target to the camera's left.</summary>
    /// <param name="camera">Camera to move.</param>
    /// <param name="speed">Distance in world units.</param>
    public static void MoveLeft(this Camera camera, float speed)
    {
        var direction = Vector3.Normalize(camera.Target - camera.Position);
        var left = Vector3.Normalize(Vector3.Cross(direction, camera.Up));
        camera.Position -= left * speed;
        camera.Target -= left * speed;
    }

    /// <summary>Strafes position and target to the camera's right.</summary>
    /// <param name="camera">Camera to move.</param>
    /// <param name="speed">Distance in world units.</param>
    public static void MoveRight(this Camera camera, float speed)
    {
        var direction = Vector3.Normalize(camera.Target - camera.Position);
        var left = Vector3.Normalize(Vector3.Cross(direction, camera.Up));
        camera.Position += left * speed;
        camera.Target += left * speed;
    }

    /// <summary>Moves position and target along the camera-relative up vector.</summary>
    /// <param name="camera">Camera to move.</param>
    /// <param name="speed">Distance in world units.</param>
    public static void MoveUp(this Camera camera, float speed)
    {
        var direction = Vector3.Normalize(camera.Target - camera.Position);
        var up = Vector3.Normalize(Vector3.Cross(direction, Vector3.Cross(camera.Up, direction)));
        camera.Position += up * speed;
        camera.Target += up * speed;
    }

    /// <summary>Moves position and target along the camera-relative down vector.</summary>
    /// <param name="camera">Camera to move.</param>
    /// <param name="speed">Distance in world units.</param>
    public static void MoveDown(this Camera camera, float speed)
    {
        var direction = Vector3.Normalize(camera.Target - camera.Position);
        var up = Vector3.Normalize(Vector3.Cross(direction, Vector3.Cross(camera.Up, direction)));
        camera.Position -= direction * speed;
        camera.Target -= up * speed;
    }
}
