namespace Novolis.Math.Geometry;

/// <summary>1D range helpers for center-aligned CAD placement.</summary>
public static class RangeExtensions
{
    /// <summary>Midpoint of an inclusive [min, max] span.</summary>
    public static float GetCenter(this (float Min, float Max) range) =>
        (range.Min + range.Max) * 0.5f;

    /// <summary>Signed length of the span (max − min).</summary>
    public static float GetLength(this (float Min, float Max) range) =>
        range.Max - range.Min;

    /// <summary>Half-length of the span.</summary>
    public static float GetHalfLength(this (float Min, float Max) range) =>
        (range.Max - range.Min) * 0.5f;

    /// <summary>
    /// Re-homes the span so its midpoint sits on <paramref name="center"/>
    /// (default ship spine / athwart zero).
    /// </summary>
    public static (float Min, float Max) CenterAligned(
        this (float Min, float Max) range,
        float center = 0f)
    {
        var half = range.GetHalfLength();
        return (center - half, center + half);
    }

    /// <summary>True when the span straddles <paramref name="center"/> (inclusive slack).</summary>
    public static bool ContainsCenter(this (float Min, float Max) range, float center = 0f, float slack = 1e-4f) =>
        range.Min <= center + slack && range.Max >= center - slack;
}
