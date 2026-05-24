namespace Novolis.Math.Geometry;

/// <summary>Cardinal direction on a grid or scene axis.</summary>
public enum Direction
{
    /// <summary>Positive vertical.</summary>
    Up,

    /// <summary>Negative vertical.</summary>
    Down,

    /// <summary>Negative horizontal (typically −X).</summary>
    Left,

    /// <summary>Positive horizontal (typically +X).</summary>
    Right,

    /// <summary>Positive depth (typically +Z).</summary>
    Forward,

    /// <summary>Negative depth (typically −Z).</summary>
    Backward
}
