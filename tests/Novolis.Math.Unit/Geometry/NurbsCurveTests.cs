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
}
