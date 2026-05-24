# Design

## Stack position

Math is the bottom numerics/geometry layer: **Math → Physics → Simulation**. It must not reference Physics, Simulation, or Raylib.

Policy: [library-boundaries.md](https://github.com/Novolis-Platform/novolis-governance/blob/main/docs/library-boundaries.md).

## Type policy

- Use BCL `System.Numerics` (`Vector3`, `Quaternion`, `Matrix4x4`) — no parallel `Vector3d`-style types.
- Planar work uses `Vector3` with **Y = 0**; do not introduce `Vector2` in stack code.
- No `dt`, clocks, integrators, or cameras in this repo (cameras live in Simulation).

### Public API naming

- **No dimension suffixes** on public types (`Ray`, not `Ray3`; `Sphere`, not `Sphere3`; `AxisAlignedBox`, not `AxisAlignedBox3`).
- **No `2D` / `Vector2`** in public API names or types; the stack is 3D-only with planar XZ via `Vector3` and `Vector3PlanarExtensions`.
- Novolis types only where the BCL has no equivalent (`Ray`, `Sphere`, `TriangleMesh`, `DenseGrid<T>`, topology records).

| Legacy (removed) | Current |
|------------------|---------|
| `Ray3` | `Ray` |
| `Sphere3` | `Sphere` |
| `AxisAlignedBox3` | `AxisAlignedBox` |

## Package split

```text
Novolis.Math.Arrays
Novolis.Math.Topology  →  (BCL only)
Novolis.Math.Geometry  →  Arrays, Topology
```

| Package | Responsibility |
|---------|----------------|
| `Novolis.Math.Arrays` | Dense volumetric grids, letter/numeric indexing |
| `Novolis.Math.Topology` | Polygon, face, edge, shape connectivity |
| `Novolis.Math.Geometry` | Primitives, meshes, transforms, intersections, BVH, lattice helpers |

`Grid<T>` in Geometry is removed; use `DenseGrid<T>` from Arrays.

## Dependencies

- Topology: no project references (BCL only).
- Geometry references Arrays and Topology.
- Stack-boundary analyzers run at build time to block forbidden cross-layer references.

## Source lineage

Renderer-agnostic types were migrated from Frank.GameEngine under the [gameengine reference policy](https://github.com/Novolis-Platform/novolis-governance/blob/main/docs/gameengine-reference-policy.md).
