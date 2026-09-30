using System.Numerics;

namespace Novolis.Math.Geometry;

public sealed record WeldOptions(
    float Tolerance,
    WeldPositionMode PositionMode = WeldPositionMode.Average,
    WeldScope Scope = WeldScope.EntireMesh);
