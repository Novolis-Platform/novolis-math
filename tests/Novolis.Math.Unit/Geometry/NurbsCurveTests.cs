using System.Numerics;
using Novolis.Math.Geometry;
using TUnit.Core;

namespace Novolis.Math.Unit.Geometry;

public sealed class NurbsCurveTests
{
    [Test]
    public async Task FromFitPoints_Produces_Valid_Knot_Length()
    {
        var fit = new[]
        {
            new Vector3(0, 0, 0),
            new Vector3(1, 0, 1),
            new Vector3(2, 0, 0),
            new Vector3(3, 0, 1),
        };
        var (degree, controls, knots, weights) = NurbsCurve.FromFitPoints(fit);
        await Assert.That(degree).IsEqualTo(3);
        await Assert.That(knots.Length).IsEqualTo(controls.Length + degree + 1);
        await Assert.That(weights.Length).IsEqualTo(controls.Length);
    }

    [Test]
    public async Task Tessellate_Endpoints_Near_Controls()
    {
        var fit = new[] { new Vector3(0, 0, 0), new Vector3(2, 0, 0), new Vector3(2, 0, 2), new Vector3(0, 0, 2) };
        var (degree, controls, knots, weights) = NurbsCurve.FromFitPoints(fit);
        var samples = NurbsCurve.Tessellate(degree, controls, knots, weights, 32);
        await Assert.That(samples.Length).IsEqualTo(32);
        await Assert.That(Vector3.Distance(samples[0], controls[0])).IsLessThan(1e-3f);
        await Assert.That(Vector3.Distance(samples[^1], controls[^1])).IsLessThan(1e-3f);
    }

    [Test]
    public async Task Evaluate_WithWeights_AndTwoPointCurve()
    {
        var fit = new[] { Vector3.Zero, Vector3.UnitX };
        var (degree, controls, knots, weights) = NurbsCurve.FromFitPoints(fit);
        await Assert.That(degree).IsEqualTo(1);
        var weighted = weights.Select((w, i) => i == 0 ? 2f : 1f).ToArray();
        var mid = NurbsCurve.Evaluate(degree, controls, knots, weighted, 0.5f);
        await Assert.That(mid.X).IsGreaterThan(0f);
        await Assert.That(mid.X).IsLessThan(1f);
    }

    [Test]
    public async Task CreateClampedUniformKnots_Validation()
    {
        var act = () => NurbsCurve.CreateClampedUniformKnots(2, 2);
        await Assert.That(act).Throws<ArgumentException>();
        var knots = NurbsCurve.CreateClampedUniformKnots(4, 2);
        await Assert.That(knots[^1]).IsEqualTo(1f);
    }

    [Test]
    public async Task FromFitPoints_And_Tessellate_Validation()
    {
        var act = () => NurbsCurve.FromFitPoints([Vector3.Zero]);
        await Assert.That(act).Throws<ArgumentException>();
        var act2 = () => NurbsCurve.Tessellate(2, [Vector3.Zero, Vector3.UnitX, Vector3.UnitY], [0f, 0f, 0f, 1f, 1f, 1f], null, 1);
        await Assert.That(act2).Throws<ArgumentOutOfRangeException>();
    }

    [Test]
    public async Task Evaluate_SamplesManyParameters_AndValidatesKnots()
    {
        var fit = Enumerable.Range(0, 8).Select(i => new Vector3(i * 0.5f, MathF.Sin(i), 0f)).ToArray();
        var (degree, controls, knots, weights) = NurbsCurve.FromFitPoints(fit);
        for (var i = 0; i <= 20; i++)
        {
            var u = knots[degree] + (knots[controls.Length] - knots[degree]) * (i / 20f);
            _ = NurbsCurve.Evaluate(degree, controls, knots, weights, u);
        }

        var act = () => NurbsCurve.Evaluate(degree, controls, [0f, 1f], null, 0.5f);
        await Assert.That(act).Throws<ArgumentException>();

        var actWeights = () => NurbsCurve.Evaluate(degree, controls, knots, [1f], 0.5f);
        await Assert.That(actWeights).Throws<ArgumentException>();
    }
}
