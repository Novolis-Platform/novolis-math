using System.Numerics;
using Novolis.Math.Geometry;
using TUnit.Core;

namespace Novolis.Math.Geometry.Tests;

public class CylindricalCoordinatesTests
{
    [Test]
    public async Task ToCartesian_OnPositiveXAxis_ReturnsRadiusAlongX()
    {
        var cylindrical = CylindricalPoint.FromDegrees(5f, 0f, 0f);

        await Assert.That(cylindrical.ToCartesian()).IsEqualTo(new Vector3(5f, 0f, 0f));
    }

    [Test]
    public async Task ToCartesian_OnPositiveYAxis_ReturnsRadiusAlongY()
    {
        var cylindrical = CylindricalPoint.FromDegrees(3f, 90f, 0f);

        var cartesian = cylindrical.ToCartesian();
        await Assert.That(cartesian.X).IsEqualTo(0f).Within(1e-5f);
        await Assert.That(cartesian.Y).IsEqualTo(3f).Within(1e-5f);
        await Assert.That(cartesian.Z).IsEqualTo(0f);
    }

    [Test]
    public async Task FromCartesian_RoundTrips_ThroughCylindrical()
    {
        var original = new Vector3(4f, 3f, 2f);

        var roundTrip = original.ToCylindrical().ToCartesian();

        await Assert.That(roundTrip.X).IsEqualTo(original.X).Within(1e-5f);
        await Assert.That(roundTrip.Y).IsEqualTo(original.Y).Within(1e-5f);
        await Assert.That(roundTrip.Z).IsEqualTo(original.Z).Within(1e-5f);
    }

    [Test]
    public async Task Vector3Extension_MatchesCylindricalPointConversion()
    {
        var vector = new Vector3(-2f, 2f, 1f);

        await Assert.That(vector.ToCylindrical().Radius).IsEqualTo(2.828427f).Within(1e-5f);
        await Assert.That(vector.ToCylindrical().AzimuthDegrees).IsEqualTo(135f).Within(1e-4f);
        var roundTrip = vector.ToCylindrical().ToCartesian();
        await Assert.That(roundTrip.X).IsEqualTo(vector.X).Within(1e-5f);
        await Assert.That(roundTrip.Y).IsEqualTo(vector.Y).Within(1e-5f);
        await Assert.That(roundTrip.Z).IsEqualTo(vector.Z).Within(1e-5f);
    }

    [Test]
    public async Task Origin_HasZeroRadius()
    {
        await Assert.That(CylindricalPoint.Origin.Radius).IsEqualTo(0f);
        await Assert.That(CylindricalPoint.Origin.ToCartesian()).IsEqualTo(Vector3.Zero);
    }

    [Test]
    public async Task ToString_ShowsBothForms()
    {
        var text = CylindricalPoint.FromDegrees(2f, 45f, 1f).ToString();

        await Assert.That(text).Contains("r=2");
        await Assert.That(text).Contains("°");
        await Assert.That(text).Contains("→");
    }
}
