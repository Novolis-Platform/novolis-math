# Getting started

`novolis-math` ships two NuGet packages on the Math layer of the Novolis stack (no time step, no cameras).

## Install

```bash
dotnet add package Novolis.Math.Arrays
dotnet add package Novolis.Math.Geometry
```

Restore from GitHub Packages (`2026.1.*`) per [novolis-governance package policy](https://github.com/Novolis-Platform/novolis-governance/blob/main/docs/package-policy.md).

## Packages

| Package | Role |
|---------|------|
| `Novolis.Math.Arrays` | `DenseGrid<T>` and `GridIndex` for volumetric data |
| `Novolis.Math.Geometry` | Primitives, meshes, transforms, ray tests, triangle BVH |

## Build and test

```bash
dotnet build Novolis.Math.slnx
dotnet test Novolis.Math.slnx
```

## Next steps

- Package READMEs under `src/Novolis.Math.*/README.md` for API entry points
- [Design](design.md) for boundaries vs Physics and Simulation
- [Release](release.md) for versioning and publishing
