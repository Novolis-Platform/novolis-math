using System.Numerics;

namespace Novolis.Math.Geometry;

public sealed record OptimizeResult(EditableMesh Mesh, IReadOnlyList<MeshDiagnostic> Diagnostics);
