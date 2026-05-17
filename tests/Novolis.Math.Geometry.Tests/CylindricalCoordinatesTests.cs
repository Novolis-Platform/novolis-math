using System.Numerics;
using FluentAssertions;
using Novolis.Math.Geometry;
using TUnit.Core;

namespace Novolis.Math.Geometry.Tests;

public class CylindricalCoordinatesTests
{
    [Test]
    public void ToCartesian_OnPositiveXAxis_ReturnsRadiusAlongX()
    {
        var cylindrical = CylindricalPoint.FromDegrees(5f, 0f, 0f);

        cylindrical.ToCartesian().Should().Be(new Vector3(5f, 0f, 0f));
    }

    [Test]
    public void ToCartesian_OnPositiveYAxis_ReturnsRadiusAlongY()
    {
        var cylindrical = CylindricalPoint.FromDegrees(3f, 90f, 0f);

        var cartesian = cylindrical.ToCartesian();
        cartesian.X.Should().BeApproximately(0f, 1e-5f);
        cartesian.Y.Should().BeApproximately(3f, 1e-5f);
        cartesian.Z.Should().Be(0f);
    }

    [Test]
    public void FromCartesian_RoundTrips_ThroughCylindrical()
    {
        var original = new Vector3(4f, 3f, 2f);

        var roundTrip = original.ToCylindrical().ToCartesian();

        roundTrip.X.Should().BeApproximately(original.X, 1e-5f);
        roundTrip.Y.Should().BeApproximately(original.Y, 1e-5f);
        roundTrip.Z.Should().BeApproximately(original.Z, 1e-5f);
    }

    [Test]
    public void Vector3Extension_MatchesCylindricalPointConversion()
    {
        var vector = new Vector3(-2f, 2f, 1f);

        vector.ToCylindrical().Radius.Should().BeApproximately(2.828427f, 1e-5f);
        vector.ToCylindrical().AzimuthDegrees.Should().BeApproximately(135f, 1e-4f);
        var roundTrip = vector.ToCylindrical().ToCartesian();
        roundTrip.X.Should().BeApproximately(vector.X, 1e-5f);
        roundTrip.Y.Should().BeApproximately(vector.Y, 1e-5f);
        roundTrip.Z.Should().BeApproximately(vector.Z, 1e-5f);
    }

    [Test]
    public void Origin_HasZeroRadius()
    {
        CylindricalPoint.Origin.Radius.Should().Be(0f);
        CylindricalPoint.Origin.ToCartesian().Should().Be(Vector3.Zero);
    }

    [Test]
    public void ToString_ShowsBothForms()
    {
        var text = CylindricalPoint.FromDegrees(2f, 45f, 1f).ToString();

        text.Should().Contain("r=2");
        text.Should().Contain("°");
        text.Should().Contain("→");
    }
}
