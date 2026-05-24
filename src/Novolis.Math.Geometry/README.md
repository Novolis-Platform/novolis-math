# Novolis.Math.Geometry

Renderer-agnostic 3D primitives, meshes, transforms, intersections, and acceleration structures on BCL `System.Numerics`.

## Install

```bash
dotnet add package Novolis.Math.Geometry
```

**Prerequisites:** [.NET 10 SDK](https://dotnet.microsoft.com/download) (`net10.0`).

## Quick start

```csharp
using System.Numerics;
using Novolis.Math.Geometry;

var ray = new Ray3(Vector3.Zero, Vector3.UnitZ);
var v0 = Vector3.Zero;
var v1 = new Vector3(1, 0, 0);
var v2 = new Vector3(0, 1, 0);

if (TriangleRay.TryHit(ray, v0, v1, v2, float.MaxValue, out var distance, out var normal))
    Console.WriteLine($"Hit at {distance}, normal {normal}");
```

## Related packages

| Package | When to use |
|---------|-------------|
| `Novolis.Math.Arrays` | `DenseGrid<T>` volumetric storage instead of the obsolete `Grid<T>` type |

## More documentation

- [Getting started](https://github.com/Novolis-Platform/novolis-math/blob/main/docs/getting-started.md)
- [Design](https://github.com/Novolis-Platform/novolis-math/blob/main/docs/design.md)

## Support

Pre-release (`2026.1.*` on GitHub Packages). Uses `Vector3` with Y = 0 for planar XZ math. Cameras moved to `Novolis.Simulation.View` — types here are obsolete.
