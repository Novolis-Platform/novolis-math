namespace Novolis.Math.Geometry;

/// <summary>Per-vertex bone influence keyed by authoring name (Assimp / Mixamo).</summary>
/// <param name="BoneName">Authoring bone name.</param>
/// <param name="Weight">Influence in [0, 1] before normalization.</param>
public readonly record struct NamedBoneWeight(string BoneName, float Weight);
