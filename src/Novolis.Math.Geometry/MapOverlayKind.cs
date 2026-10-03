namespace Novolis.Math.Geometry;

/// <summary>Framework-neutral kinds of selectable map overlay.</summary>
public enum MapOverlayKind
{
    /// <summary>A point marker.</summary>
    Marker,

    /// <summary>A radius-based circle.</summary>
    Circle,

    /// <summary>An open connected path.</summary>
    Track,

    /// <summary>A closed geographic area.</summary>
    Polygon,
}
