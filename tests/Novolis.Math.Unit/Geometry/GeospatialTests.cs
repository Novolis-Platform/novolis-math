using Novolis.Math.Geometry;

namespace Novolis.Math.Unit.Geometry;

public sealed class GeospatialTests
{
    [Test]
    public async Task GeoCoordinate_validates_latitude_and_longitude()
    {
        await Assert.That(() => new GeoCoordinate(91, 0)).Throws<ArgumentOutOfRangeException>();
        await Assert.That(() => new GeoCoordinate(0, 181)).Throws<ArgumentOutOfRangeException>();
        await Assert.That(() => new GeoCoordinate(double.NaN, 0)).Throws<ArgumentOutOfRangeException>();

        var antimeridian = new GeoCoordinate(0, 180);
        await Assert.That(antimeridian.Longitude).IsEqualTo(180d);
    }

    [Test]
    public async Task GeoDistance_calculates_one_degree_and_crosses_antimeridian()
    {
        var oneDegree = GeoDistance.Between(
            new GeoCoordinate(0, 0),
            new GeoCoordinate(0, 1));
        var acrossAntimeridian = GeoDistance.Between(
            new GeoCoordinate(0, 179.5),
            new GeoCoordinate(0, -179.5));

        await Assert.That(oneDegree).IsEqualTo(111_194.9d).Within(100d);
        await Assert.That(acrossAntimeridian).IsEqualTo(111_194.9d).Within(100d);
    }

    [Test]
    public async Task GeoCircle_classifies_accuracy_aware_measurements()
    {
        var circle = new GeoCircle(new GeoCoordinate(0, 0), 1_000);

        var definitelyInside = circle.Classify(new GeoCoordinate(0, 0.005), 100);
        var probablyInside = circle.Classify(new GeoCoordinate(0, 0.008), 200);
        var uncertain = circle.Classify(new GeoCoordinate(0, 0.009), 100);
        var definitelyOutside = circle.Classify(new GeoCoordinate(0, 0.01), 10);

        await Assert.That(definitelyInside).IsEqualTo(GeoContainmentClassification.DefinitelyInside);
        await Assert.That(probablyInside).IsEqualTo(GeoContainmentClassification.ProbablyInside);
        await Assert.That(uncertain).IsEqualTo(GeoContainmentClassification.Uncertain);
        await Assert.That(definitelyOutside).IsEqualTo(GeoContainmentClassification.DefinitelyOutside);
    }

    [Test]
    public async Task GeoCircle_rejects_invalid_radius_and_accuracy()
    {
        await Assert.That(() => new GeoCircle(new GeoCoordinate(0, 0), -1)).Throws<ArgumentOutOfRangeException>();

        var circle = new GeoCircle(new GeoCoordinate(0, 0), 100);
        await Assert.That(() => circle.Classify(new GeoCoordinate(0, 0), -1))
            .Throws<ArgumentOutOfRangeException>();
    }

    [Test]
    public async Task WebMercator_round_trips_a_terrestrial_coordinate()
    {
        var original = new GeoCoordinate(58.14623, 7.99517);
        var projected = WebMercatorProjection.Project(original);
        var roundTrip = WebMercatorProjection.Unproject(projected);

        await Assert.That(roundTrip.Latitude).IsEqualTo(original.Latitude).Within(1e-9);
        await Assert.That(roundTrip.Longitude).IsEqualTo(original.Longitude).Within(1e-9);
    }

    [Test]
    public async Task WebMercator_clamps_polar_latitudes_and_validates_world_coordinates()
    {
        var projected = WebMercatorProjection.Project(new GeoCoordinate(90, 0));

        await Assert.That(projected.Y).IsEqualTo(0d).Within(1e-12);
        await Assert.That(() => WebMercatorProjection.Unproject(new GeoProjectedPoint(1.1, 0.5)))
            .Throws<ArgumentOutOfRangeException>();
    }

    [Test]
    public async Task MapTileKey_wraps_x_and_validates_y()
    {
        var key = new MapTileKey(3, -1, 2);

        await Assert.That(key.X).IsEqualTo(7);
        await Assert.That(key.Y).IsEqualTo(2);
        await Assert.That(() => new MapTileKey(3, 0, 8))
            .Throws<ArgumentOutOfRangeException>();
    }

    [Test]
    public async Task WebMercatorTiles_round_trip_a_kristiansand_coordinate()
    {
        var center = new GeoCoordinate(58.14623, 7.99517);
        var coordinate = new GeoCoordinate(58.15, 8.01);
        var pixel = WebMercatorTiles.GeoToPixel(
            center,
            zoom: 12,
            width: 800,
            height: 600,
            coordinate);
        var roundTrip = WebMercatorTiles.PixelToGeo(
            center,
            zoom: 12,
            width: 800,
            height: 600,
            pixel.X,
            pixel.Y);

        await Assert.That(pixel.X).IsGreaterThan(400d);
        await Assert.That(pixel.Y).IsLessThan(300d);
        await Assert.That(roundTrip.Latitude).IsEqualTo(coordinate.Latitude).Within(1e-9);
        await Assert.That(roundTrip.Longitude).IsEqualTo(coordinate.Longitude).Within(1e-9);
    }

    [Test]
    public async Task WebMercatorTiles_keeps_antimeridian_coordinates_nearby()
    {
        var pixel = WebMercatorTiles.GeoToPixel(
            new GeoCoordinate(0, 179.9),
            zoom: 8,
            width: 800,
            height: 600,
            new GeoCoordinate(0, -179.9));

        await Assert.That(pixel.X).IsGreaterThan(300d);
        await Assert.That(pixel.X).IsLessThan(500d);
    }

    [Test]
    public async Task WebMercatorTiles_lists_wrapped_tiles_and_clips_polar_rows()
    {
        var visible = WebMercatorTiles.VisibleTiles(
            new GeoCoordinate(0, 179),
            zoom: 3,
            width: 512,
            height: 512);

        await Assert.That(visible).IsNotEmpty();
        await Assert.That(visible.All(tile => tile.Zoom == 3 && tile.Y is >= 0 and < 8))
            .IsTrue();
    }
}
