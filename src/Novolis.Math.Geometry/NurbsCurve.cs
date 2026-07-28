using System.Numerics;

namespace Novolis.Math.Geometry;

/// <summary>
/// Open clamped NURBS / B-spline curve evaluation and tessellation (no time, no cameras).
/// Knot vector length must be <c>controlPoints.Length + degree + 1</c>.
/// </summary>
public static class NurbsCurve
{
    /// <summary>Builds a clamped uniform open knot vector for the given control count and degree.</summary>
    public static float[] CreateClampedUniformKnots(int controlPointCount, int degree)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(controlPointCount, 2);
        ArgumentOutOfRangeException.ThrowIfLessThan(degree, 1);
        if (controlPointCount <= degree)
            throw new ArgumentException("Need more control points than the curve degree.", nameof(controlPointCount));

        var m = controlPointCount + degree + 1;
        var knots = new float[m];
        var internalCount = controlPointCount - degree;
        for (var i = 0; i <= degree; i++)
            knots[i] = 0f;
        for (var i = 1; i < internalCount; i++)
            knots[degree + i] = i / (float)internalCount;
        for (var i = m - degree - 1; i < m; i++)
            knots[i] = 1f;
        return knots;
    }

    /// <summary>
    /// Builds a cubic (degree 3) open B-spline through treating fit points as control points
    /// when there are enough samples; otherwise lowers degree to <c>fitPoints.Length - 1</c>.
    /// </summary>
    public static (int Degree, Vector3[] ControlPoints, float[] Knots, float[] Weights) FromFitPoints(
        IReadOnlyList<Vector3> fitPoints)
    {
        ArgumentNullException.ThrowIfNull(fitPoints);
        if (fitPoints.Count < 2)
            throw new ArgumentException("Need at least two fit points.", nameof(fitPoints));

        var controls = new Vector3[fitPoints.Count];
        for (var i = 0; i < fitPoints.Count; i++)
            controls[i] = fitPoints[i];

        var degree = System.Math.Min(3, fitPoints.Count - 1);
        var knots = CreateClampedUniformKnots(controls.Length, degree);
        var weights = new float[controls.Length];
        Array.Fill(weights, 1f);
        return (degree, controls, knots, weights);
    }

    /// <summary>Evaluates the curve at parameter <paramref name="u"/> in the knot domain.</summary>
    public static Vector3 Evaluate(
        int degree,
        IReadOnlyList<Vector3> controlPoints,
        IReadOnlyList<float> knots,
        IReadOnlyList<float>? weights,
        float u)
    {
        ArgumentNullException.ThrowIfNull(controlPoints);
        ArgumentNullException.ThrowIfNull(knots);
        Validate(degree, controlPoints.Count, knots.Count, weights?.Count);

        var n = controlPoints.Count - 1;
        var uMin = knots[degree];
        var uMax = knots[n + 1];
        if (uMax <= uMin)
            return controlPoints[0];

        u = System.Math.Clamp(u, uMin, uMax);
        // Stay slightly inside the last span for open clamped curves.
        if (u >= uMax)
            u = uMax - 1e-6f * (uMax - uMin);

        var span = FindSpan(n, degree, u, knots);
        var basis = BasisFunctions(span, u, degree, knots);

        var numerator = Vector3.Zero;
        var denominator = 0f;
        for (var j = 0; j <= degree; j++)
        {
            var w = weights is null ? 1f : weights[span - degree + j];
            var term = basis[j] * w;
            numerator += controlPoints[span - degree + j] * term;
            denominator += term;
        }

        return denominator > 1e-12f ? numerator / denominator : controlPoints[span - degree];
    }

    /// <summary>Samples the curve into a polyline (inclusive endpoints).</summary>
    public static Vector3[] Tessellate(
        int degree,
        IReadOnlyList<Vector3> controlPoints,
        IReadOnlyList<float> knots,
        IReadOnlyList<float>? weights,
        int sampleCount)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(sampleCount, 2);
        Validate(degree, controlPoints.Count, knots.Count, weights?.Count);

        var n = controlPoints.Count - 1;
        var uMin = knots[degree];
        var uMax = knots[n + 1];
        var samples = new Vector3[sampleCount];
        for (var i = 0; i < sampleCount; i++)
        {
            var t = i / (float)(sampleCount - 1);
            var u = uMin + (uMax - uMin) * t;
            samples[i] = Evaluate(degree, controlPoints, knots, weights, u);
        }

        return samples;
    }

    private static void Validate(int degree, int controlCount, int knotCount, int? weightCount)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(degree, 1);
        ArgumentOutOfRangeException.ThrowIfLessThan(controlCount, degree + 1);
        if (knotCount != controlCount + degree + 1)
            throw new ArgumentException($"Knot count must be controlPoints + degree + 1 ({controlCount + degree + 1}).");
        if (weightCount is { } w && w != controlCount)
            throw new ArgumentException("Weights length must match control point count.");
    }

    private static int FindSpan(int n, int degree, float u, IReadOnlyList<float> knots)
    {
        if (u >= knots[n + 1])
            return n;
        if (u <= knots[degree])
            return degree;

        var low = degree;
        var high = n + 1;
        var mid = (low + high) / 2;
        while (u < knots[mid] || u >= knots[mid + 1])
        {
            if (u < knots[mid])
                high = mid;
            else
                low = mid;
            mid = (low + high) / 2;
        }

        return mid;
    }

    private static float[] BasisFunctions(int span, float u, int degree, IReadOnlyList<float> knots)
    {
        var n = new float[degree + 1];
        var left = new float[degree + 1];
        var right = new float[degree + 1];
        n[0] = 1f;
        for (var j = 1; j <= degree; j++)
        {
            left[j] = u - knots[span + 1 - j];
            right[j] = knots[span + j] - u;
            var saved = 0f;
            for (var r = 0; r < j; r++)
            {
                var temp = n[r] / (right[r + 1] + left[j - r]);
                n[r] = saved + right[r + 1] * temp;
                saved = left[j - r] * temp;
            }

            n[j] = saved;
        }

        return n;
    }
}
