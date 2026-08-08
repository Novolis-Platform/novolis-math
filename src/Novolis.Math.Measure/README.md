<!-- novolis-pkg-brand:start -->
<p align="center">
  <a href="https://github.com/Novolis-Platform/novolis-math">
    <img src="https://raw.githubusercontent.com/Novolis-Platform/.github/main/brand/logo-icon.svg" width="72" alt="Novolis"/>
  </a>
</p>
<!-- novolis-pkg-brand:end -->

# Novolis.Math.Measure

Scalar measure types for lengths, sizes, thicknesses, and rects. Canonical unit is the typographic **point** (1/72 inch). No `Vector2` — extents use named Width/Height/`Length` fields.

## Install

```bash
dotnet add package Novolis.Math.Measure
```

**Prerequisites:** [.NET 10 SDK](https://dotnet.microsoft.com/download) (`net10.0`).

## Quick start

```csharp
using Novolis.Math.Measure;

var trim = new Size(LengthUnits.FromInches(6f), LengthUnits.FromInches(9f));
var margin = Thickness.Symmetric(LengthUnits.FromInches(0.65f), LengthUnits.FromInches(0.75f));
var page = Rect.FromSize(LengthUnits.FromPoints(0), LengthUnits.FromPoints(0), trim);
var content = page.Inset(margin);
```

## Related packages

| Package | When to use |
|---------|-------------|
| `Novolis.Math.Geometry` | 3D meshes, rays, BVH on BCL `System.Numerics` |
| `Novolis.Documents` | Paged document page setup built on these measures |

## Support

- Docs: [novolis-math](https://github.com/Novolis-Platform/novolis-math)
- Issues: [GitHub Issues](https://github.com/Novolis-Platform/novolis-math/issues)
