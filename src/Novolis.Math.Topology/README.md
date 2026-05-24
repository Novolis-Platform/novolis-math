# Novolis.Math.Topology

Connectivity primitives: `Polygon`, `Face`, `Edge`, and factories. Uses BCL `Vector3` for vertex positions.

## Install

```bash
dotnet add package Novolis.Math.Topology
```

**Prerequisites:** [.NET 10 SDK](https://dotnet.microsoft.com/download) (`net10.0`).

## Quick start

```csharp
using Novolis.Math.Topology;
using System.Numerics;

var quad = PolygonFactory.Quad(
    new Vector3(0, 0, 0),
    new Vector3(1, 0, 0),
    new Vector3(1, 0, 1),
    new Vector3(0, 0, 1));

foreach (var edge in quad.Edges)
    Console.WriteLine(edge);
```

## Related packages

| Package | When to use |
|---------|-------------|
| `Novolis.Math.Geometry` | Ray tests, meshes, BVH, transforms (references Topology transitively) |
| `Novolis.Math.Arrays` | Volumetric `DenseGrid<T>` |

## More documentation

- [Design](https://github.com/Novolis-Platform/novolis-math/blob/main/docs/design.md)
- [Getting started](https://github.com/Novolis-Platform/novolis-math/blob/main/docs/getting-started.md)
