# Novolis.Math.Arrays

Dense volumetric grids with numeric and spreadsheet-style indexing.

## Install

```bash
dotnet add package Novolis.Math.Arrays
```

**Prerequisites:** [.NET 10 SDK](https://dotnet.microsoft.com/download) (`net10.0`).

## Quick start

```csharp
using Novolis.Math.Arrays;

var board = new DenseGrid<char>(8, 8);
board["a", "a"] = 'X';
board[3, 4] = 'O';

foreach (var cell in board.Find(c => c == 'X'))
    Console.WriteLine(cell);
```

## Related packages

| Package | When to use |
|---------|-------------|
| `Novolis.Math.Geometry` | Meshes, rays, BVH, and lattice types (`LatticePoint`) for world-space geometry |

## More documentation

- [Getting started](https://github.com/Novolis-Platform/novolis-math/blob/main/docs/getting-started.md)
- [Design](https://github.com/Novolis-Platform/novolis-math/blob/main/docs/design.md)

## Support

Pre-release (`2026.1.*` on GitHub Packages). No simulation time or camera APIs — those live in Physics and Simulation.
