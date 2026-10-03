using Novolis.Math.Geometry;

namespace Novolis.Math.Unit.Geometry;

public sealed class MapOverlayContractTests
{
    [Test]
    public async Task Overlay_keys_are_type_qualified_and_reject_blank_ids()
    {
        var marker = new MapOverlayKey(MapOverlayKind.Marker, "shared-id");
        var polygon = new MapOverlayKey(MapOverlayKind.Polygon, "shared-id");

        await Assert.That(marker).IsNotEqualTo(polygon);
        await Assert.That(marker.ToString()).IsEqualTo("Marker:shared-id");
        await Assert.That(
                () => new MapOverlayKey(MapOverlayKind.Track, " "))
            .Throws<ArgumentException>();
    }

    [Test]
    public async Task Rectangle_drawing_exposes_closed_geometry_and_statistics()
    {
        var drawing = new GeoDrawing(
            GeoDrawingKind.Rectangle,
            [
                new GeoCoordinate(58.10, 7.90),
                new GeoCoordinate(58.20, 8.10),
            ]);

        await Assert.That(drawing.Rectangle).IsNotNull();
        await Assert.That(drawing.ClosedPoints).Count().IsEqualTo(5);
        await Assert.That(drawing.Statistics.VertexCount).IsEqualTo(4);
        await Assert.That(drawing.Statistics.LengthMeters).IsGreaterThan(1_000);
        await Assert.That(drawing.Statistics.AreaSquareMeters).IsGreaterThan(1_000_000);
    }

    [Test]
    public async Task Rectangle_requires_opposite_corners_and_formats_preview_measurement()
    {
        await Assert.That(
                () => new GeoDrawing(
                    GeoDrawingKind.Rectangle,
                    [new GeoCoordinate(58.10, 7.90)]))
            .Throws<ArgumentException>();

        var preview = GeoMeasurementText.ForDrawing(
            GeoDrawingKind.Rectangle,
            [
                new GeoCoordinate(58.10, 7.90),
                new GeoCoordinate(58.20, 8.10),
            ]);

        await Assert.That(preview).Contains("Rectangle");
        await Assert.That(preview).Contains("area");
    }

    [Test]
    public async Task Keyboard_contract_includes_overlay_erasure()
    {
        await Assert.That(Enum.GetValues<MapKeyboardCommand>())
            .Contains(MapKeyboardCommand.EraseSelectedOverlay);
    }
}
