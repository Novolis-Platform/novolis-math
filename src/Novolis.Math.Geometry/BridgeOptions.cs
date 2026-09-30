using System.Numerics;

namespace Novolis.Math.Geometry;

public sealed record BridgeOptions(
    int Segments = 1,
    float Twist = 0f,
    bool ReverseSecondLoop = false);
