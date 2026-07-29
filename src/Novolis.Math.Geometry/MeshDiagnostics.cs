namespace Novolis.Math.Geometry;

public enum MeshDiagnosticSeverity
{
    Info,
    Warning,
    Error,
}

public sealed record MeshDiagnostic(
    MeshDiagnosticSeverity Severity,
    string Code,
    string Message,
    IReadOnlyList<int> ComponentIds);
