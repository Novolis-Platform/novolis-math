namespace Novolis.Math.Geometry;

public sealed record MeshDiagnostic(
    MeshDiagnosticSeverity Severity,
    string Code,
    string Message,
    IReadOnlyList<int> ComponentIds);
