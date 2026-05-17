using System.Numerics;
using FluentAssertions;
using Novolis.Math.Geometry;
using TUnit.Core;

namespace Novolis.Math.Geometry.Tests;

public class PolarCoordinatesTests
{
    [Test]
    public void ToCartesian_OnPositiveXAxis_ReturnsRadiusAlongX()
    {
        var polar = PolarPoint.FromDegrees(5f, 0f);

        polar.ToCartesian().Should().Be(new CartesianPoint(5f, 0f));
        polar.ToVector2().Should().Be(new Vector2(5f, 0f));
    }

    [Test]
    public void ToCartesian_OnPositiveYAxis_ReturnsRadiusAlongY()
    {
        var polar = PolarPoint.FromDegrees(3f, 90f);

        var cartesian = polar.ToCartesian();
        cartesian.X.Should().BeApproximately(0f, 1e-5f);
        cartesian.Y.Should().BeApproximately(3f, 1e-5f);
    }

    [Test]
    public void FromCartesian_RoundTrips_ThroughPolar()
    {
        var original = new CartesianPoint(4f, 3f);

        var roundTrip = original.ToPolar().ToCartesian();

        roundTrip.X.Should().BeApproximately(original.X, 1e-5f);
        roundTrip.Y.Should().BeApproximately(original.Y, 1e-5f);
    }

    [Test]
    public void Vector2Extension_MatchesPolarPointConversion()
    {
        var vector = new Vector2(-2f, 2f);

        vector.ToPolar().Radius.Should().BeApproximately(2.828427f, 1e-5f);
        vector.ToPolar().AngleDegrees.Should().BeApproximately(135f, 1e-4f);
        var roundTrip = vector.ToPolar().ToVector2();
        roundTrip.X.Should().BeApproximately(vector.X, 1e-5f);
        roundTrip.Y.Should().BeApproximately(vector.Y, 1e-5f);
    }

    [Test]
    public void Origin_HasZeroRadius()
    {
        PolarPoint.Origin.Radius.Should().Be(0f);
        PolarPoint.Origin.ToCartesian().Should().Be(CartesianPoint.Zero);
    }

    [Test]
    public void ToString_ShowsBothForms()
    {
        var text = PolarPoint.FromDegrees(2f, 45f).ToString();

        text.Should().Contain("r=2");
        text.Should().Contain("°");
        text.Should().Contain("→");
    }
}
