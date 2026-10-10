using System.Numerics;

namespace Novolis.Math.Geometry;

/// <summary>XZ-plane hit: point, outward normal (Y = 0), and distance along the ray.</summary>
public readonly record struct PlanarHit(Vector3 Point, Vector3 Normal, float Distance);
