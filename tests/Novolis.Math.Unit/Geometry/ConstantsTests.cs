using System.Numerics;
using Novolis.Math.Geometry;
using TUnit.Core;

namespace Novolis.Math.Geometry.Tests;

public class ConstantsTests
{
    [Test]
    public async Task TerrestrialAndUniversalConstants_HaveExpectedMagnitudes()
    {
        var gravity = Constants.TerrestrialConstants.EarthGravity;
        var earthRadius = Constants.TerrestrialConstants.EarthRadius;
        var orbitalSpeed = Constants.TerrestrialConstants.EarthOrbitalSpeed;
        var speedOfLight = Constants.UniversalConstants.SpeedOfLight;
        var pi = Constants.UniversalConstants.Pi;
        var gConst = Constants.UniversalConstants.GravitationalConstant;
        var planck = Constants.UniversalConstants.PlancksConstant;

        await Assert.That(gravity).IsEqualTo(9.80665f).Within(0.001f);
        await Assert.That(earthRadius).IsEqualTo(6371m);
        await Assert.That(orbitalSpeed).IsEqualTo(29.78m);
        await Assert.That(speedOfLight).IsEqualTo(299792458m);
        await Assert.That(pi).IsGreaterThan(3.14m);
        await Assert.That(gConst).IsGreaterThan(0m);
        await Assert.That(planck).IsEqualTo(Constants.UniversalConstants.PlancksConstant);
    }

    [Test]
    public async Task MathPhysicsAspectAndVectorConstants_AreUsable()
    {
        var piOver2 = Constants.MathConstants.PiOver2;
        var piOver4 = Constants.MathConstants.PiOver4;
        var twoPi = Constants.MathConstants.TwoPi;
        var epsilon = Constants.MathConstants.Epsilon;
        var deg2Rad = Constants.MathConstants.Deg2Rad;
        var rad2Deg = Constants.MathConstants.Rad2Deg;
        var inf = Constants.MathConstants.Infinity;
        var ninf = Constants.MathConstants.NegativeInfinity;
        var gunpowder = Constants.PhysicsConstants.GunpowderEnergyPerGramInJoules;
        var landscape = Constants.AspectRatioConstants.Landscape;
        var portrait = Constants.AspectRatioConstants.Portrait;
        var console = Constants.AspectRatioConstants.Console;
        var superUltrawide = Constants.AspectRatioConstants.SuperUltrawide;
        var up = Constants.VectorConstants.Up;
        var earthG = Constants.VectorConstants.PhysicsConstants.EarthGravity;

        await Assert.That(piOver2).IsEqualTo(MathF.PI / 2f).Within(1e-5f);
        await Assert.That(piOver4).IsGreaterThan(0.7f);
        await Assert.That(twoPi).IsGreaterThan(6.2f);
        await Assert.That(epsilon).IsGreaterThan(0f);
        await Assert.That(deg2Rad).IsGreaterThan(0f);
        await Assert.That(rad2Deg).IsEqualTo(180f / MathF.PI).Within(1e-4f);
        await Assert.That(inf).IsEqualTo(float.PositiveInfinity);
        await Assert.That(ninf).IsEqualTo(float.NegativeInfinity);
        await Assert.That(gunpowder).IsEqualTo(3000);
        await Assert.That(landscape).IsEqualTo(1.333f).Within(0.001f);
        await Assert.That(portrait).IsEqualTo(0.75f);
        await Assert.That(console).IsGreaterThan(1.7f);
        await Assert.That(superUltrawide).IsGreaterThan(3f);
        await Assert.That(up).IsEqualTo(new Vector3(0, 0, 1));
        await Assert.That(earthG.Y).IsLessThan(0f);
    }
}
