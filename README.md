<!-- novolis-marketing:start -->
<p align="center">
  <a href="https://github.com/Novolis-Platform">
    <img src="https://raw.githubusercontent.com/Novolis-Platform/.github/main/brand/logo-brand-transparent.svg" width="360" alt="Novolis"/>
  </a>
</p>

<p align="center">
  <img src="https://raw.githubusercontent.com/Novolis-Platform/.github/main/brand/banners/novolis-math.svg" width="100%" alt="novolis-math"/>
</p>

<p align="center">
  <strong>Geometry on BCL numerics</strong><br/>
  Renderer-agnostic math: arrays, geometry, topology — System.Numerics first.
</p>

<p align="center">
  <a href="https://github.com/Novolis-Platform/novolis-math/actions"><img src="https://img.shields.io/github/actions/workflow/status/Novolis-Platform/novolis-math/merge.yml?branch=main&label=merge&logo=github" alt="merge"/></a>
  <a href="https://github.com/orgs/Novolis-Platform/packages?repo_name=novolis-math"><img src="https://img.shields.io/badge/packages-GitHub%20Packages-0a7ea3?logo=nuget" alt="packages"/></a>
  <a href="https://github.com/Novolis-Platform"><img src="https://img.shields.io/badge/org-Novolis--Platform-111827" alt="org"/></a>
</p>

<p align="center">
  <a href="https://nuget.pkg.github.com/Novolis-Platform/index.json"><code>https://nuget.pkg.github.com/Novolis-Platform/index.json</code></a>
  ·
  <a href="https://github.com/Novolis-Platform/.github/blob/main/profile/README.md">Org landing</a>
  ·
  <a href="https://github.com/Novolis-Platform/novolis-governance">Governance</a>
</p>

---
<!-- novolis-marketing:end -->
<!-- novolis-package-index:start -->
> **GitHub Packages shows this repository README on every package page** (upstream limitation).
> Open the **package README** for install and quick start — embedded in each .nupkg and linked below.

## Published packages

| Package | Install | Package README |
|---------|---------|----------------|
| `Novolis.Math.Arrays` | `dotnet add package Novolis.Math.Arrays` | [README](https://github.com/Novolis-Platform/novolis-math/blob/main/src/Novolis.Math.Arrays/README.md) |
| `Novolis.Math.Geometry` | `dotnet add package Novolis.Math.Geometry` | [README](https://github.com/Novolis-Platform/novolis-math/blob/main/src/Novolis.Math.Geometry/README.md) |
| `Novolis.Math.Topology` | `dotnet add package Novolis.Math.Topology` | [README](https://github.com/Novolis-Platform/novolis-math/blob/main/src/Novolis.Math.Topology/README.md) |
| `Novolis.Math.Measure` | `dotnet add package Novolis.Math.Measure` | [README](https://github.com/Novolis-Platform/novolis-math/blob/main/src/Novolis.Math.Measure/README.md) |

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
| `Novolis.Math.Topology` | Polygon, face, edge connectivity |
| `Novolis.Math.Measure` | Scalar length/size/thickness/rect (points) |

## Build

```bash
dotnet build Novolis.Math.slnx
dotnet test Novolis.Math.slnx
```

## Source policy

Frank.GameEngine remains active on the author's GitHub; only renderer-agnostic primitives were migrated. See [gameengine-reference-policy](https://github.com/Novolis-Platform/novolis-governance/blob/main/docs/gameengine-reference-policy.md).

