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
