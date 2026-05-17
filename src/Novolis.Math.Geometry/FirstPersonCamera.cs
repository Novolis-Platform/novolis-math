using System.Numerics;

namespace Novolis.Math.Geometry;

/// <summary>
/// First-person orientation: yaw around world +Y, pitch for look up/down.
/// Movement uses yaw on the XZ plane; pitch affects view direction only.
/// </summary>
public sealed class FirstPersonCamera
{
    private const float PitchLimit = MathF.PI * 0.499f;

    /// <summary>Eye position in world space.</summary>
    public Vector3 Position { get; set; }

    /// <summary>Yaw in radians (rotation about +Y).</summary>
    public float Yaw { get; set; }

    /// <summary>Pitch in radians (look up/down).</summary>
    public float Pitch { get; set; }

    /// <summary>Horizontal movement direction from yaw (XZ plane, unit length).</summary>
    public Vector3 GetForwardXZ()
    {
        return new Vector3(MathF.Sin(Yaw), 0f, MathF.Cos(Yaw));
    }

    /// <summary>Strafe direction (right) on the XZ plane.</summary>
    public Vector3 GetRightXZ()
    {
        return new Vector3(MathF.Cos(Yaw), 0f, -MathF.Sin(Yaw));
    }

    /// <summary>Full look direction including pitch.</summary>
    public Vector3 GetLookDirection()
    {
        var cosP = MathF.Cos(Pitch);
        return Vector3.Normalize(new Vector3(MathF.Sin(Yaw) * cosP, MathF.Sin(Pitch), MathF.Cos(Yaw) * cosP));
    }

    /// <summary>Applies mouse deltas in radians (deltaYaw, deltaPitch).</summary>
    public void AddLookDelta(float deltaYaw, float deltaPitch)
    {
        Yaw += deltaYaw;
        Pitch += deltaPitch;
        Pitch = System.Math.Clamp(Pitch, -PitchLimit, PitchLimit);
    }

    /// <summary>Eye position with optional vertical offset.</summary>
    public Vector3 GetEyePosition(float eyeHeight = 0f) =>
        Position + new Vector3(0f, eyeHeight, 0f);

    /// <summary>Look-at target for a perspective camera.</summary>
    public Vector3 GetLookTarget(float eyeHeight = 0f, float lookDistance = 10f)
    {
        var eye = GetEyePosition(eyeHeight);
        return eye + GetLookDirection() * lookDistance;
    }
}
