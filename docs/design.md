# Design

## Stack position

Math is the bottom numerics/geometry layer: **Math → Physics → Simulation**. It must not reference Physics, Simulation, or Raylib.

Policy: [library-boundaries.md](https://github.com/Novolis-Platform/novolis-governance/blob/main/docs/library-boundaries.md).

## Type policy

- Use BCL `System.Numerics` (`Vector3`, `Quaternion`, `Matrix4x4`) — no parallel `Vector3d`-style types.
- Planar work uses `Vector3` with **Y = 0**; do not introduce `Vector2` in stack code.
- No `dt`, clocks, integrators, or cameras in this repo (cameras live in Simulation).

## Package split

| Package | Responsibility |
|---------|----------------|
| `Novolis.Math.Arrays` | Dense 3D grids, letter/numeric indexing |
| `Novolis.Math.Geometry` | Scene primitives, mesh topology, spatial queries |

`Grid<T>` in Geometry is obsolete; new code should use `DenseGrid<T>` from Arrays.

## Dependencies

- Geometry references Arrays for shared grid indexing patterns.
- Stack-boundary analyzers run at build time to block forbidden cross-layer references.

## Source lineage

Renderer-agnostic types were migrated from Frank.GameEngine under the [gameengine reference policy](https://github.com/Novolis-Platform/novolis-governance/blob/main/docs/gameengine-reference-policy.md).
