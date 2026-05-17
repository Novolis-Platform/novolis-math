# novolis-math

Novolis math libraries extracted from [Frank.GameEngine](https://github.com/frankhaugen/Frank.GameEngine) primitives (wave 7).

## Packages

| Package | Description |
|---------|-------------|
| `Novolis.Math.Arrays` | `Array2D<T>` and grid helpers |
| `Novolis.Math.Geometry` | Polygons, meshes, transforms, cameras |

## Build

```bash
dotnet build Novolis.Math.slnx
dotnet test Novolis.Math.slnx
```

## Source policy

Frank.GameEngine remains active on the author's GitHub; only renderer-agnostic primitives were migrated. See [gameengine-reference-policy](https://github.com/Novolis-Platform/novolis-governance/blob/main/docs/gameengine-reference-policy.md).
