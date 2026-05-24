<!-- novolis-package-index:start -->
> **GitHub Packages shows this repository README on every package page** (upstream limitation).
> Open the **package README** for install and quick start — embedded in each .nupkg and linked below.

## Published packages

| Package | Install | Package README |
|---------|---------|----------------|
| `Novolis.Math.Arrays` | `dotnet add package Novolis.Math.Arrays` | [README](https://github.com/Novolis-Platform/novolis-math/blob/main/src/Novolis.Math.Arrays/README.md) |
| `Novolis.Math.Geometry` | `dotnet add package Novolis.Math.Geometry` | [README](https://github.com/Novolis-Platform/novolis-math/blob/main/src/Novolis.Math.Geometry/README.md) |
| `Novolis.Math.Topology` | `dotnet add package Novolis.Math.Topology` | [README](https://github.com/Novolis-Platform/novolis-math/blob/main/src/Novolis.Math.Topology/README.md) |

For NuGet.org and Visual Studio, the **embedded** README.md inside each package is authoritative.

<!-- novolis-package-index:end -->

# novolis-math

Novolis **Math** (wave 7): numerics, geometry, and topology facets. **No time, no cameras** — those belong to Physics and Simulation.

Policy: [library-boundaries.md](https://github.com/Novolis-Platform/novolis-governance/blob/main/docs/library-boundaries.md).

## Packages

| Package | Description |
|---------|-------------|
| `Novolis.Math.Arrays` | `DenseGrid<T>` volumetric storage and `GridIndex` |
| `Novolis.Math.Geometry` | Meshes, primitives, transforms, intersections, BVH |
| `Novolis.Math.Topology` | *(planned)* connectivity / topological structure |

## Build

```bash
dotnet build Novolis.Math.slnx
dotnet test Novolis.Math.slnx
```

## Source policy

Frank.GameEngine remains active on the author's GitHub; only renderer-agnostic primitives were migrated. See [gameengine-reference-policy](https://github.com/Novolis-Platform/novolis-governance/blob/main/docs/gameengine-reference-policy.md).

